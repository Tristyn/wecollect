using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Contracts.Contracts;
using Contracts.Contracts.Cards;
using Nethereum.Hex.HexTypes;
using WeCollect.App.Documents;
using WeCollect.App.Web3;
using WeCollect.Server.Models;

namespace WeCollect.App.Bll
{
    public class CardFactory
    {
        private readonly Web3Db _web3Db;
        private readonly CardDocumentDb _documentDb;
        private readonly string _serverAddress;
        private readonly string _serverPrivateKey;
        private readonly CardsService _cardMethods;

        public CardFactory(Web3Db web3Db, CardDocumentDb documentDb, ServerConfiguration config)
        {
            _web3Db = web3Db;
            _documentDb = documentDb;
            _serverAddress = config.Web3ServerAddress;
            _serverPrivateKey = config.Web3ServerPrivateKey;
            _cardMethods = web3Db.Cards;
        }

        /// <summary>
        /// Mints a new card into existance.
        /// </summary>
        public async Task CreateCard(CardDto card, CardOptions options)
        {
            if (!options.SkipContract)
            {
                var contractCard = new ContractCardDto
                {
                    Price = new HexBigInteger(card.PriceEth),
                    MiningLastCollectedDate = new HexBigInteger(card.LastMiningCollectedDate.ToUnixTimeSeconds()),
                    MiningLevel = card.MiningLevel,
                    ParentCards = card.Parents.Select(parent => parent.Id).ToArray(),
                    FirstOwner = card.FirstOwnerAddress
                };

                

                await _cardMethods.MintCardRequestAndWaitForReceiptAsync(new Contracts.Contracts.Cards.ContractDefinition.MintCardFunction
                {
                    Card = contractCard,
                    FromAddress = _serverAddress
                });
            }
        }

        public class CardOptions
        {
            public bool SkipContract { get; set; }

            public bool CreateDocument { get; set; }
        }
    }
}
