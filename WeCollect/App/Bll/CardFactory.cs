using Contracts.Contracts.Cards;
using Contracts.Contracts.Cards.ContractDefinition;
using ImageMagick;
using Nethereum.Hex.HexTypes;
using Nethereum.RPC.Eth.DTOs;
using Nethereum.Web3;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using WeCollect.App.Blob;
using WeCollect.App.Documents;
using WeCollect.App.Extensions;
using WeCollect.App.Models;
using WeCollect.App.Web3;
using WeCollect.Server.Models;

namespace WeCollect.App.Bll
{
    public class CardFactory
    {
        private readonly Web3Db _web3Db;
        private readonly Nethereum.Web3.Web3 _web3;
        private readonly CardDocumentDb _documentDb;
        private readonly BlobService _blobService;
        private readonly string _serverAddress;
        private readonly string _serverPrivateKey;
        private readonly CardsService _cardMethods;

        public CardFactory(Web3Db web3Db, Nethereum.Web3.Web3 web3, CardDocumentDb documentDb, ServerConfiguration config, BlobService blobService)
        {
            _web3Db = web3Db;
            _web3 = web3;
            _documentDb = documentDb;
            _blobService = blobService;
            _serverAddress = config.Web3ServerAddress;
            _serverPrivateKey = config.Web3ServerPrivateKey;
            _cardMethods = web3Db.Cards;
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
                case MintingStatus.Start:
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

                    Stream imageStream;
                    if (mintingCard.ImageUploadUri != null)
                    {
                        using (var client = new WebClient())
                        {
                            imageStream = new MemoryStream(client.DownloadData(mintingCard.ImageUploadUri));
                        }
                    }
                    else
                    {
                        imageStream = mintingCard.ImageFormUploadUri.OpenReadStream();
                    }

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

                    await _documentDb.Cards.Upsert(card);

                    if (exists)
                        await _documentDb.EntityIds.Replace(id);
                    else await _documentDb.EntityIds.Create(id);

                    goto case MintingStatus.TransactionHashSaved;
                case MintingStatus.TransactionHashSaved:

                    card = card ?? await _documentDb.Cards.Get(CardDto.GetId(mintingCard.Name));
                    parentCard = null;
                    if (mintingCard.ParentCardsContractId != null)
                    {
                        parentCard = await _documentDb.GetCardWithCardsContractId(mintingCard.ParentCardsContractId.Value);
                    }

                    await _web3.UnlockServerAccount(_serverAddress, _serverPrivateKey, 100);

                    var mintFunction = mintingCard.ToMintCardFunction(parentCard);
                    mintFunction.Gas = 750000;
                    mintFunction.FromAddress = _serverAddress;
                    card.mintTransactionHash = await _cardMethods.MintCardRequestAsync(mintFunction);

                    await _documentDb.Cards.Replace(card);

                    goto case MintingStatus.TransactionInMempool;
                case MintingStatus.TransactionInMempool:
                    card = card ?? await _documentDb.Cards.Get(CardDto.GetId(mintingCard.Name));

                    bool sleeping = false;
                    TransactionReceipt mintTransactionReceipt = null;
                    do
                    {
                        mintTransactionReceipt = await _web3.Eth.Transactions.GetTransactionReceipt.SendRequestAsync(card.mintTransactionHash);
                        if (sleeping)
                            await Task.Delay(1000);
                        sleeping = true;
                    } while (mintTransactionReceipt?.BlockNumber == null);

                    await _documentDb.Cards.Replace(card);

                    goto case MintingStatus.TransactionProcessedUpdatingDocument;
                case MintingStatus.TransactionProcessedUpdatingDocument:
                    card = card ?? await _documentDb.Cards.Get(CardDto.GetId(mintingCard.Name));

                    card.mintingStatus = CardDto.MintStatus.Complete;
                    await _documentDb.Cards.Replace(card);

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
                return MintingStatus.Start;
            }

            var cardDoc = await _documentDb.Cards.Get(CardDto.GetId(name));

            // Transaction not sent?
            if (cardDoc.mintTransactionHash == null)
            {
                return MintingStatus.TransactionHashSaved;
            }
            var mempoolTransactionStatus = await _web3.Eth.Transactions.GetTransactionByHash.SendRequestAsync(cardDoc.mintTransactionHash);
            if (mempoolTransactionStatus == null)
            {
                return MintingStatus.TransactionHashSaved;
            }

            // Transaction queued in ethereum?
            if (mempoolTransactionStatus.BlockNumber == null)
            {
                return MintingStatus.TransactionInMempool;
            }

            // Transaction execution failed (gas == gasUsed) note ALWAYS OVERGAS to use this to detect errors
            var receipt = await _web3.Eth.Transactions.GetTransactionReceipt.SendRequestAsync(cardDoc.mintTransactionHash);
            if (mempoolTransactionStatus.Gas.Value >= receipt.GasUsed.Value)
            {
                return MintingStatus.TransactionFailed;
            }
            //transaction failed? https://ethereum.stackexchange.com/questions/6007/how-can-the-transaction-status-from-a-thrown-error-be-detected-when-gas-can-be-e

            if (cardDoc.mintingStatus == CardDto.MintStatus.MintingTransaction)
            {
                return MintingStatus.TransactionProcessedUpdatingDocument;
            }

            return MintingStatus.Complete;

        }

        public enum MintingStatus
        {
            Start = 0,
            TransactionHashSaved = 1,
            TransactionInMempool = 2,
            TransactionFailed = -1,
            TransactionProcessedUpdatingDocument = 3,
            Complete = 4
        }

        public class CardOptions
        {
            public bool SkipContract { get; set; }

            public bool CreateDocument { get; set; }
        }
    }
}
