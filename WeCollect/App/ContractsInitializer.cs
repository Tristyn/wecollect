using System.Threading.Tasks;
using WeCollect.App.Models;
using WeCollect.App.Web3;

namespace WeCollect.App
{
    public class ContractsInitializer
    {
        private readonly Container _container;

        public ContractsInitializer(Container container)
        {
            _container = container;
        }

        public async Task Initialize()
        {
            foreach (ContractArtifact contract in _container.ContractArtifacts.All)
            {
                if(!await _container.Documents.Contracts.Exists(contract.Name, ensureStatusCode: false))
                {
                    ContractPublisher publisher = new ContractPublisher(contract, _container);

                    Nethereum.Contracts.Contract deployed = await publisher.Deploy();
                    await publisher.CreateContractDocument(deployed);
                }
            }
        }

        public async Task Redeploy()
        {

            foreach (ContractArtifact contractArtifact in _container.ContractArtifacts.All)
            {
                var contract = await _container.Documents.GetContractWithName(contractArtifact.Name);
                ContractPublisher publisher = new ContractPublisher(contractArtifact, _container);

                Nethereum.Contracts.Contract deployed = await publisher.Deploy();
                await publisher.UpdateContractDocument(deployed, contract);

            }
        }
    }
}
