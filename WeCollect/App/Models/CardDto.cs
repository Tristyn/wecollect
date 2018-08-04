using Nethereum.Hex.HexTypes;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.ComponentModel.DataAnnotations;
using System.Numerics;

namespace WeCollect.App.Models
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

        public BigInteger PriceEth { get; set; }

        public DateTimeOffset LastMiningCollectedDate { get; set; }

        public int MiningLevel { get; set; } = 1;

        [MaxLength(7)]
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

        public BigInteger CurrentMiningCollectableWcc => throw new NotImplementedException();

        //
        // CosmosDB Data
        //

        public override string Name { get; set; }

        public string Set { get; set; }

        public string OwnerName { get; set; }

        public string FirstOwnerName { get; set; }
        
        public CardSpecDto ToCardSpec()
        {
            return new CardSpecDto
            {
                Id = Id,
                Name = Name
            };
        }

        public static string GetId(string name)
        {
            return nameof(CardDto) + name;
        }
    }
}
