using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace WeCollect.App.Models
{
    [JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy))]
    public class CardCreatedDto
    {
        public int Id { get; set; }
        
        //public CardDto Card { get; set; }
    }
}
