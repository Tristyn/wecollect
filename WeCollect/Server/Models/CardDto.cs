using Contracts.Contracts;
using Contracts.Contracts.Cards.ContractDefinition;
using Nethereum.Hex.HexTypes;
using Nethereum.RPC.Eth.DTOs;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Linq;
using System.Numerics;
using WeCollect.App.Models;

namespace WeCollect.Server.Models
{
    [JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy))]
    public class CardDto : Document
    {
        private CardSpecDto[] _parents = Array.Empty<CardSpecDto>();

        //
        // Contract Data
        //

        /// <summary>
        /// Id in the Cards contract
        /// </summary>
        public int cardsContractId { get; set; }

        [JsonProperty(PropertyName = "owner")]
        public string ownerAddress { get; set; }

        [JsonProperty("firstOwner")]
        public string initialOwnerAddress { get; set; }

        public BigInteger priceWei { get; set; }

        public DateTimeOffset lastMiningCollectedDate { get; set; }

        public int miningLevel { get; set; } = 1;

        /// <summary>
        /// Max Length 7
        /// </summary>
        public CardSpecDto[] parents
        {
            get
            {
                return _parents;
            }
            set
            {
                if (parents == null)
                    throw new ArgumentNullException(nameof(parents));

                if (parents.Length > 7)
                    throw new ArgumentOutOfRangeException(nameof(parents), 7, null);

                _parents = value;
            }
        }

        //
        // Contract Events 
        //

        public BigInteger nextPriceEth { get; set; }

        public BigInteger miningRatePerBlockWcc { get; set; }

        public BigInteger totalMiningCollectedWcc { get; set; }

        public BigInteger getCurrentMiningCollectableWcc() { throw new NotImplementedException(); }

        //
        // CosmosDB Data
        //

        public override string name { get; set; }

        public string uriName { get; set; }

        public override string type => nameof(CardDto);

        public string set { get; set; }

        public string uriSet { get; set; }

        public string ownerName { get; set; }

        public string firstOwnerName { get; set; }

        public string firstOwnerAddress { get; set; }

        public MintStatus mintingStatus { get; set; }

        public enum MintStatus
        {
            /// <summary>
            /// Doc exists in Db, MintTransactionHash is set and MAY OR MAY NOT
            /// BE sent to the eth network.
            /// </summary>
            MintingTransaction = 1,
            /// <summary>
            /// Complete
            /// </summary>
            Complete = 2
        }

        public string mintTransactionHash { get; set; }

        public CardSpecDto ToCardSpec()
        {
            return new CardSpecDto
            {
                Id = cardsContractId,
                Name = name
            };
        }

        public static string GetId(string name)
        {
            return nameof(CardDto) + name;
        }
    }
}
