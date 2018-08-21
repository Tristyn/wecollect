using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Contracts.Contracts.Cards;
using Contracts.Contracts.Cards.ContractDefinition;
using Nethereum.Hex.HexTypes;
using Nethereum.Web3;
using WeCollect.App.Documents;
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


                    var mintCardFunction = new MintCardFunction
                    {
                        Card = mintingCard.ToContractCardDto(),
                        FromAddress = _serverAddress
                    };
                    card = new CardDto
                    {
                        Name = mintingCard.Name,
                        LastMiningCollectedDate = mintingCard.LastMiningCollectedDate,
                        MiningLevel = mintingCard.MiningLevel,
                        PriceWei = mintingCard.PriceWei,
                        MintingStatus = CardDto.MintStatus.MintingTransaction,
                        MintTransactionPayload = mintCardFunction,
                    };

                    await _documentDb.Cards.Upsert(card);

                    goto case MintingStatus.TransactionHashSaved;
                case MintingStatus.TransactionHashSaved:

                    card = card ?? await _documentDb.Cards.Get(CardDto.GetId(mintingCard.Name));

                    await _web3.UnlockServerAccount(_serverAddress, _serverPrivateKey, 100);

                    card.MintTransactionHash = await _cardMethods.MintCardRequestAsync(mintingCard.ToContractCardDto());

                    await _documentDb.Cards.Replace(card);

                    goto case MintingStatus.TransactionInMempool;
                case MintingStatus.TransactionInMempool:
                    card = card ?? await _documentDb.Cards.Get(CardDto.GetId(mintingCard.Name));

                    bool sleeping = false;
                    do
                    {
                        card.MintTransactionReceipt = await _web3.Eth.Transactions.GetTransactionReceipt.SendRequestAsync(card.MintTransactionHash);
                        if (sleeping)
                            await Task.Delay(1000);
                        sleeping = true;
                    } while (card.MintTransactionReceipt?.BlockNumber == null);

                    card.MintingStatus = CardDto.MintStatus.UpdatingDocument;
                    await _documentDb.Cards.Replace(card);

                    goto case MintingStatus.TransactionProcessedUpdatingDocument;
                case MintingStatus.TransactionProcessedUpdatingDocument:

                    var logs = card.MintTransactionReceipt.Logs;
                    Debugger.Break();

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
            Debug.Assert(cardDoc.MintTransactionHash != null);

            // Transaction not sent?
            var mempoolTransactionStatus = await _web3.Eth.Transactions.GetTransactionByHash.SendRequestAsync(cardDoc.MintTransactionHash);
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
            var receipt = await _web3.Eth.Transactions.GetTransactionReceipt.SendRequestAsync(cardDoc.MintTransactionHash);
            if (mempoolTransactionStatus.Gas.Value >= receipt.GasUsed.Value)
            {
                return MintingStatus.TransactionFailed;
            }
            //transaction failed? https://ethereum.stackexchange.com/questions/6007/how-can-the-transaction-status-from-a-thrown-error-be-detected-when-gas-can-be-e

            if (cardDoc.MintingStatus == CardDto.MintStatus.MintingTransaction)
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
