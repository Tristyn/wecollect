using Nethereum.Contracts;
using System.Linq;
using System.Threading.Tasks;
using WeCollect.App.Documents;
using WeCollect.App.Models;
using WeCollect.App.Web3;

namespace WeCollect.App
{
    public class ContractsInitializer
    {
        private readonly Container _container;
        private readonly Collection<ContractDto> _contractDocuments;

        public ContractsInitializer(Container container)
        {
            _container = container;

            _contractDocuments = _container.Documents.Contracts;
        }

        public async Task Initialize()
        {
            await Task.WhenAll(_container.ContractArtifacts.All
                .Select(async contract =>
                {
                    if(!await _contractDocuments.Exists(contract.Name))
                    {
                        ContractPublisher publisher = new ContractPublisher(contract, _container);

                        Contract deployed = await publisher.Deploy();
                        await publisher.CreateContractDocument(deployed);
                    }
                }));
        }

        public async Task Redeploy()
        {
            foreach (ContractArtifact contractArtifact in _container.ContractArtifacts.All)
            {
                var contract = await _container.Documents.Contracts.Get(contractArtifact.Id);
                ContractPublisher publisher = new ContractPublisher(contractArtifact, _container);

                Contract deployed = await publisher.Deploy();
                await publisher.UpdateContractDocument(deployed, contract);

            }
        }
    }
}
