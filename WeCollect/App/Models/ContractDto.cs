using Newtonsoft.Json;

namespace WeCollect.App.Models
{
    public class ContractDto : Document
    {
        public override string Name { get; set; }

        protected override string Type => nameof(ContractDto);

        [JsonProperty(PropertyName = "address")]
        public string Address { get; set; }

        [JsonProperty(PropertyName = "abi")]
        public string Abi { get; set; }
    }
}
