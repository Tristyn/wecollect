using Nethereum.Hex.HexTypes;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using WeCollect.Server.Models;

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
