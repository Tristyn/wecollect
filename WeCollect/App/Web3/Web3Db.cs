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
            string serverAddress,
            string serverPrivateKey)
        {
            Web3 = web3;
            ServerAddress = serverAddress;
            ServerPrivateKey = serverPrivateKey;
        }

        public Nethereum.Web3.Web3 Web3 { get; }
        public string ServerAddress { get; }
        public string ServerPrivateKey { get; }
    }
}
