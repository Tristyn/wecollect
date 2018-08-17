using Nethereum.Contracts;
using Nethereum.Hex.HexTypes;
using Nethereum.RPC.Eth.DTOs;
using Nethereum.Web3;
using System.Threading.Tasks;
using WeCollect.App.Models;

namespace WeCollect.App.Web3
{
    public class ContractPublisher
    {
        private readonly ContractArtifact _contractArtifact;
        private readonly Container _container;

        public ContractPublisher(ContractArtifact contractArtifact, Container container)
        {
            _contractArtifact = contractArtifact;
            _container = container;
        }

        public async Task<(Contract, TransactionReceipt)> Deploy()
        {
            string txnHash = await _container.Web3.Eth.DeployContract.SendRequestAsync(
                _contractArtifact.Abi,
                _contractArtifact.Bin,
                _container.Config.Web3ServerAddress,
                new HexBigInteger(4712388));

            TransactionReceipt receipt;
            do
            {
                receipt = await _container.Web3.Eth.Transactions.GetTransactionReceipt.SendRequestAsync(txnHash);
                if (receipt == null)
                {
                    await Task.Delay(1000);
                }
            } while (receipt == null);

            Contract contract = _container.Web3.Eth.GetContract(_contractArtifact.Abi, receipt.ContractAddress);

            return (contract, receipt);
        }

        public async Task CreateContractDocument(Contract contract, TransactionReceipt receipt)
        {
            var contractDocument = new ContractDto
            {
                Id = _contractArtifact.Id,
                Name = _contractArtifact.Name,
                Abi = _contractArtifact.Abi,
                Address = contract.Address,
                
                TransactionReceipt = receipt
            };
            
            await _container.Documents.Contracts.Create(contractDocument);
        }

        public async Task UpdateContractDocument(Contract etherContract, ContractDto contract, TransactionReceipt receipt)
        {
            contract.Id = _contractArtifact.Id;
            contract.Name = _contractArtifact.Name;
            contract.Abi = _contractArtifact.Abi;
            contract.Address = etherContract.Address;

            contract.TransactionReceipt = receipt;

            await _container.Documents.Contracts.Upsert(contract);
        }
    }
}
