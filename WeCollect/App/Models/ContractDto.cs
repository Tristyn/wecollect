using Nethereum.RPC.Eth.DTOs;
using Newtonsoft.Json;

namespace WeCollect.App.Models
{
    public class ContractDto : Document
    {
        public override string name { get; set; }

        [JsonProperty(PropertyName = "address")]
        public string Address { get; set; }

        [JsonProperty(PropertyName = "abi")]
        public string Abi { get; set; }

        public override string type => nameof(ContractDto);

        public TransactionReceipt TransactionReceipt { get; set; }

        public static string GetId(string name)
        {
            return nameof(ContractDto) + name;
        }
    }
}
