using Nethereum.Hex.HexTypes;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace WeCollect.App.Models
{
    [JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy))]
    public class CardMiningCollectedDto
    {
        public int Id { get; set; }

        //public CardDto Card { get; set; }
        
        public HexBigInteger amount { get; set; }
    }
}
