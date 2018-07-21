using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Nethereum.Web3;

namespace WeCollect.App.Web3
{
    public class Web3Db
    {
        public Web3Db(
            Nethereum.Web3.Web3 web3,
            ContractArtifacts contracts,
            string serverAddress,
            string serverPrivateKey)
        {
            Web3 = web3;
            Contracts = contracts;
            ServerAddress = serverAddress;
            ServerPrivateKey = serverPrivateKey;
        }

        public Nethereum.Web3.Web3 Web3 { get; }
        public ContractArtifacts Contracts { get; }
        public string ServerAddress { get; }
        public string ServerPrivateKey { get; }

        public async Task UnlockServerAccount(ulong? durationInSeconds)
        {
            bool unlockAccountResult = await Web3.Personal.UnlockAccount.SendRequestAsync(
                ServerAddress,
                ServerPrivateKey,
                durationInSeconds);
            if (!unlockAccountResult)
            {
                throw new Exception();
            }
        }
    }
}
