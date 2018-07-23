using Newtonsoft.Json;

namespace WeCollect.App.Models
{
    public class ContractDto : Document
    {
        public override string Name { get; set; }

        [JsonProperty(PropertyName = "address")]
        public string Address { get; set; }

        [JsonProperty(PropertyName = "abi")]
        public string Abi { get; set; }
        
        public static string GetId(string name)
        {
            return nameof(ContractDto) + name;
        }
    }
}
