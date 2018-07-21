using WeCollect.App.Documents;
using WeCollect.App.Web3;

namespace WeCollect.App
{
    public class Container
    {
        public Web3Db Web3 { get; set; }
        
        public DocumentDb Documents { get; set; }

        public ContractArtifacts ContractArtifacts { get; set; }

        public ContractsInitializer ContractsInitializer { get; set; }
    }
}
