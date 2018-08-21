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
        public int CardsContractId { get; set; }

        [JsonProperty(PropertyName = "owner")]
        public string OwnerAddress { get; set; }

        [JsonProperty("firstOwner")]
        public string InitialOwnerAddress { get; set; }

        public BigInteger PriceWei { get; set; }

        public DateTimeOffset LastMiningCollectedDate { get; set; }

        public int MiningLevel { get; set; } = 1;

        /// <summary>
        /// Max Length 7
        /// </summary>
        public CardSpecDto[] Parents
        {
            get
            {
                return _parents;
            }
            set
            {
                if (Parents == null)
                    throw new ArgumentNullException(nameof(Parents));

                if (Parents.Length > 7)
                    throw new ArgumentOutOfRangeException(nameof(Parents), 7, null);

                _parents = value;
            }
        }

        //
        // Contract Events 
        //

        public BigInteger NextPriceEth { get; set; }

        public BigInteger MiningRatePerBlockWcc { get; set; }

        public BigInteger TotalMiningCollectedWcc { get; set; }

        public BigInteger GetCurrentMiningCollectableWcc() { throw new NotImplementedException(); }

        //
        // CosmosDB Data
        //

        public override string Name { get; set; }

        public override string Type => nameof(CardDto);

        public string Set { get; set; }

        public string OwnerName { get; set; }

        public string FirstOwnerName { get; set; }

        public string FirstOwnerAddress { get; set; }

        public MintStatus MintingStatus { get; set; }

        public enum MintStatus
        {
            /// <summary>
            /// Doc exists in Db, MintTransactionHash is set and MAY OR MAY NOT
            /// BE sent to the eth network.
            /// </summary>
            MintingTransaction = 1,
            /// <summary>
            /// Querying transaction receipt and updating DB
            /// </summary>
            UpdatingDocument = 2,
            /// <summary>
            /// Complete
            /// </summary>
            Complete
        }


        public MintCardFunction MintTransactionPayload { get; set; }

        public TransactionReceipt MintTransactionReceipt { get; set; }
        public string MintTransactionHash { get; set; }

        public CardSpecDto ToCardSpec()
        {
            return new CardSpecDto
            {
                Id = CardsContractId,
                Name = Name
            };
        }

        public static string GetId(string name)
        {
            return nameof(CardDto) + name;
        }
    }
}
