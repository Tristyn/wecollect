using Nethereum.Hex.HexTypes;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WeCollect.App.Models
{
    [JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy))]
    public class BoughtCardDto
    {
        public int Id { get; set; }

        public CardDto Card { get; set; }

        public HexBigInteger BuyingPrice { get; set; }

        public HexBigInteger NextAskingPrice { get; set; }

        public HexBigInteger MiningRate { get; set; }

        public HexBigInteger MiningCollected { get; set; }
    }
}
