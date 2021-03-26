using Contracts.Contracts.Cards;
using Contracts.Contracts.Cards.ContractDefinition;
using ImageMagick;
using Nethereum.RPC.Eth.DTOs;
using Nethereum.Web3;
using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using WeCollect.App.Blob;
using WeCollect.App.Documents;
using WeCollect.App.Models;
using WeCollect.App.Web3;
using WeCollect.Server.Models;

namespace WeCollect.App.Bll
{
    public class CardFactory
    {
        private readonly Web3Db _web3Db = Web3Db.web3Db;
        private readonly Nethereum.Web3.Web3 _web3 = Web3Db.web3;
        private readonly CardDb _documentDb;
        private readonly BlobService _blobService;
        private readonly string _hotAddress;
        private readonly string _hotPrivateKey;
        private readonly CardsService _cardMethods;

        public CardFactory(CardDb documentDb, ServerConfiguration config, BlobService blobService)
        {
            _documentDb = documentDb;
            _blobService = blobService;
            _hotAddress = config.Web3HotAddress;
            _hotPrivateKey = config.Web3HotPrivateKey;
            _cardMethods = _web3Db.Cards;
        }

        /// <summary>
        /// Mints a new card into existance.
        /// </summary>
        public async Task MintCard(CardMintingDto mintingCard, CardOptions options)
        {
            CardDto card = null;
            var mintingStatus = await GetMintingStatus(mintingCard.Name);

            switch (mintingStatus)
            {
                case MintingStatus.TransactionFailed:



                    break;
                case MintingStatus.SavingDocumentWithTransactionHash:
                    IdCounterDto id;
                    bool exists;
                    if (exists = await _documentDb.EntityIds.Exists(IdCounterDto.GetId("cardsContractId")))
                    {
                        id = await _documentDb.EntityIds.Get(IdCounterDto.GetId("cardsContractId"));
                    }
                    else
                    {
                        id = new IdCounterDto
                        {
                            name = "cardsContractId",
                            currentId = 0
                        };
                    }
                    id.currentId++;

                    CardDto parentCard = null;
                    if (mintingCard.ParentCardsContractId != null)
                    {
                        parentCard = await _documentDb.GetCardWithCardsContractId(mintingCard.ParentCardsContractId.Value);
                    }

                    card = new CardDto
                    {
                        name = mintingCard.Name,
                        cardsContractId = id.currentId,
                        uriName = mintingCard.Name.ToUriSafeString(),
                        imageBlobName = BlobService.CardImage + mintingCard.Name.ToUriSafeString() + ".jpeg",
                        parents = parentCard?.ToCardSpecsForChildCard() ?? Array.Empty<CardSpecDto>(),
                        lastMiningCollectedDate = DateTimeOffset.FromUnixTimeSeconds(mintingCard.LastMiningCollectedDate),
                        miningLevel = mintingCard.MiningLevel,
                        priceWei = mintingCard.PriceWei,
                        mintingStatus = CardDto.MintStatus.MintingTransaction,
                    };

                    Stream imageStream = null;
                    if (mintingCard.ImageUploadUri != null)
                    {
                        using (var client = new WebClient())
                        {
                            imageStream = new MemoryStream(client.DownloadData(mintingCard.ImageUploadUri));
                        }
                    }
                    else if (mintingCard.ImageFormUploadUri != null)
                    {
                        imageStream = mintingCard.ImageFormUploadUri.OpenReadStream();
                    }
                    else
                    {
                        Log.LogError($"[{nameof(CardFactory)}] No image provided for card {card.name}.");
                    }

                    if (imageStream != null)
                    {
                        using (imageStream)
                        {
                            using (var image = new MagickImage(imageStream))
                            {
                                image.Format = MagickFormat.Jpeg;
                                image.Scale(new Percentage(210d / image.Height * 100));
                                var editedStream = new MemoryStream();
                                image.Write(editedStream);
                                editedStream.Position = 0;
                                var optimizer = new ImageOptimizer();
                                optimizer.LosslessCompress(editedStream);
                                editedStream.Position = 0;
                                await _blobService.Save(editedStream, card.imageBlobName);
                            }
                        }
                    }

                    await _documentDb.Cards.Upsert(card);

                    if (exists)
                        await _documentDb.EntityIds.Replace(id);
                    else await _documentDb.EntityIds.Create(id);

                    goto case MintingStatus.ProcessingTransaction;
                case MintingStatus.ProcessingTransaction:

                    card = card ?? await _documentDb.Cards.Get(CardDto.GetId(mintingCard.Name));
                    parentCard = null;
                    if (mintingCard.ParentCardsContractId != null)
                    {
                        parentCard = await _documentDb.GetCardWithCardsContractId(mintingCard.ParentCardsContractId.Value);
                    }

                    await _web3.UnlockServerAccount(_hotAddress, _hotPrivateKey, 100);

                    MintCardFunction mintFunction = mintingCard.ToMintCardFunction(parentCard);
                    mintFunction.Gas = 750000;
                    mintFunction.FromAddress = _hotAddress;

                    bool sleeping = false;
                    TransactionReceipt mintTransactionReceipt = null;
                    do
                    {
                        if (card.mintTransactionHash == null)
                        {
                            card.mintTransactionHash = await _cardMethods.MintCardRequestAsync(mintFunction);
                            card = await _documentDb.Cards.ReplaceGet(card);
                        }
                        
                        mintTransactionReceipt = card.mintTransactionHash != null ?
                            await _web3.Eth.Transactions.GetTransactionReceipt.SendRequestAsync(card.mintTransactionHash) : null;
                        
                        if (mintTransactionReceipt == null || mintTransactionReceipt?.HasErrors() == true)
                        {
                            card.mintTransactionHash = await _cardMethods.MintCardRequestAsync(mintFunction);
                            card = await _documentDb.Cards.ReplaceGet(card);
                        }

                        if (sleeping)
                            await Task.Delay(1000);
                        sleeping = true;


                        if (mintTransactionReceipt?.BlockNumber != null && mintTransactionReceipt?.Status.Value.IsOne == true)
                        {
                            break;
                        }
                    } while (true);

                    card = await _documentDb.Cards.ReplaceGet(card);
                    
                    goto case MintingStatus.UpdatingDocument;
                case MintingStatus.UpdatingDocument:
                    card = card ?? await _documentDb.Cards.Get(CardDto.GetId(mintingCard.Name));

                    // This marks the card as minted when we save it
                    card.mintingStatus = CardDto.MintStatus.Complete;
                    card = await _documentDb.Cards.ReplaceGet(card);

                    goto case MintingStatus.Complete;
                case MintingStatus.Complete:


                    break;
            };





        }

        /// <summary>
        /// Note: may block to mine the transaction and get a receipt
        /// </summary>
        public async Task<MintingStatus> GetMintingStatus(string name)
        {
            // Not Started?
            if (!await _documentDb.Cards.Exists(CardDto.GetId(name)))
            {
                return MintingStatus.SavingDocumentWithTransactionHash;
            }

            var cardDoc = await _documentDb.Cards.Get(CardDto.GetId(name));

            // Transaction not sent?
            if (cardDoc.mintTransactionHash == null)
            {
                return MintingStatus.ProcessingTransaction;
            }

            // Transaction dropped out of network mempool?
            var mempoolTransactionStatus = await _web3.Eth.Transactions.GetTransactionByHash.SendRequestAsync(cardDoc.mintTransactionHash);
            if (mempoolTransactionStatus == null)
            {
                return MintingStatus.ProcessingTransaction;
            }

            // Transaction queued in network mempool?
            if (mempoolTransactionStatus.BlockNumber == null)
            {
                return MintingStatus.ProcessingTransaction;
            }

            // Transaction execution failed (gas == gasUsed) note ALWAYS OVERGAS to use this to detect errors
            var receipt = await _web3.Eth.Transactions.GetTransactionReceipt.SendRequestAsync(cardDoc.mintTransactionHash);
            if (receipt == null || receipt.HasErrors() == true)
            {
                return MintingStatus.ProcessingTransaction;
            }
            //transaction failed? https://ethereum.stackexchange.com/questions/6007/how-can-the-transaction-status-from-a-thrown-error-be-detected-when-gas-can-be-e

            if (cardDoc.mintingStatus == CardDto.MintStatus.MintingTransaction)
            {
                return MintingStatus.UpdatingDocument;
            }

            return MintingStatus.Complete;

        }

        public enum MintingStatus
        {
            TransactionFailed = -1,
            SavingDocumentWithTransactionHash = 0,
            ProcessingTransaction = 1,
            UpdatingDocument = 2,
            Complete = 3
        }

        public class CardOptions
        {
            public bool SkipContract { get; set; }

            public bool CreateDocument { get; set; }
        }
    }
}
