using Nethereum.ABI.FunctionEncoding.Attributes;
using Nethereum.Contracts;
using Nethereum.RPC.Eth.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;
using WeCollect.App.Web3;

namespace WeCollect.App.Data
{
    public class TestContractWeb3
    {
        private readonly Web3Db _web3;
        private readonly Contract _contract;
        private readonly ContractArtifact _artifact;
        private readonly Function _multiply;
        private readonly Event _onMultiplied;

        public TestContractWeb3(Web3Db web3, Contract contract, ContractArtifact artifact) {
            _web3 = web3;
            _contract = contract;
            _artifact = artifact;

            _multiply = contract.GetFunction("multiply");

            _onMultiplied = contract.GetEvent("onMultiplied");
        }

        public void Multiply(int value)
        {
            //_multiply.SendTransactionAsync(new TransactionInput();
            //_web3.Web3.Eth.TransactionManager.TransactionReceiptService.PollForReceiptAsync()
        }

        public async Task<List<EventLog<MultipliedEvent>>> OnMultiplied(string address, BlockParameter fromBlock)
        {
            var filter = await _onMultiplied.CreateFilterAsync(fromBlock);
            var log = await _onMultiplied.GetFilterChanges<MultipliedEvent>(filter);
            return log;
        }

        public class MultipliedEvent
        {
            [Parameter("int", "a", 1, true)]
            public int Input { get; set; }

            [Parameter("int", "sender", 2, true)]
            public string Sender { get; set; }

            [Parameter("int", "result", 3, false)]
            public int Result { get; set; }
        }
    }
}
