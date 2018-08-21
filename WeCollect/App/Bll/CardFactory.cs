using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Contracts.Contracts.Cards;
using Contracts.Contracts.Cards.ContractDefinition;
using Nethereum.Hex.HexTypes;
using Nethereum.RPC.Eth.DTOs;
using Nethereum.Web3;
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
        private readonly string _serverAddress;
        private readonly string _serverPrivateKey;
        private readonly CardsService _cardMethods;

        public CardFactory(Web3Db web3Db, Nethereum.Web3.Web3 web3, CardDocumentDb documentDb, ServerConfiguration config)
        {
            _web3Db = web3Db;
            _web3 = web3;
            _documentDb = documentDb;
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
                    
                    card = new CardDto
                    {
                        name = mintingCard.Name,
                        uriName = mintingCard.Name.ToUriSafeString(),
                        lastMiningCollectedDate = mintingCard.LastMiningCollectedDate,
                        miningLevel = mintingCard.MiningLevel,
                        priceWei = mintingCard.PriceWei,
                        mintingStatus = CardDto.MintStatus.MintingTransaction,
                    };

                    await _documentDb.Cards.Upsert(card);

                    goto case MintingStatus.TransactionHashSaved;
                case MintingStatus.TransactionHashSaved:

                    card = card ?? await _documentDb.Cards.Get(CardDto.GetId(mintingCard.Name));

                    await _web3.UnlockServerAccount(_serverAddress, _serverPrivateKey, 100);

                    var mintFunction = mintingCard.ToMintCardFunction();
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
