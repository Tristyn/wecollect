using System.Threading.Tasks;
using System.Numerics;
using Nethereum.RPC.Eth.DTOs;
using Nethereum.Contracts.ContractHandlers;
using System.Threading;
using Contracts.Contracts.Test.ContractDefinition;
namespace Contracts.Contracts.Test
{

    public partial class TestService
    {
    
        public static Task<TransactionReceipt> DeployContractAndWaitForReceiptAsync(Nethereum.Web3.Web3 web3, TestDeployment testDeployment, CancellationTokenSource cancellationTokenSource = null)
        {
            return web3.Eth.GetContractDeploymentHandler<TestDeployment>().SendRequestAndWaitForReceiptAsync(testDeployment, cancellationTokenSource);
        }
        public static Task<string> DeployContractAsync(Nethereum.Web3.Web3 web3, TestDeployment testDeployment)
        {
            return web3.Eth.GetContractDeploymentHandler<TestDeployment>().SendRequestAsync(testDeployment);
        }
        public static async Task<TestService> DeployContractAndGetServiceAsync(Nethereum.Web3.Web3 web3, TestDeployment testDeployment, CancellationTokenSource cancellationTokenSource = null)
        {
            var receipt = await DeployContractAndWaitForReceiptAsync(web3, testDeployment, cancellationTokenSource);
            return new TestService(web3, receipt.ContractAddress);
        }
    
        protected Nethereum.Web3.Web3 Web3{ get; }
        
        public ContractHandler ContractHandler { get; }
        
        public TestService(Nethereum.Web3.Web3 web3, string contractAddress)
        {
            Web3 = web3;
            ContractHandler = web3.Eth.GetContractHandler(contractAddress);
        }
    
        public Task<string> MulitplyRequestAsync(MulitplyFunction mulitplyFunction)
        {
             return ContractHandler.SendRequestAsync(mulitplyFunction);
        }

        public Task<TransactionReceipt> MulitplyRequestAndWaitForReceiptAsync(MulitplyFunction mulitplyFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(mulitplyFunction, cancellationToken);
        }

        public Task<string> MulitplyRequestAsync(BigInteger val)
        {
            var mulitplyFunction = new MulitplyFunction();
                mulitplyFunction.Val = val;
            
             return ContractHandler.SendRequestAsync(mulitplyFunction);
        }

        public Task<TransactionReceipt> MulitplyRequestAndWaitForReceiptAsync(BigInteger val, CancellationTokenSource cancellationToken = null)
        {
            var mulitplyFunction = new MulitplyFunction();
                mulitplyFunction.Val = val;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(mulitplyFunction, cancellationToken);
        }
    }
}
