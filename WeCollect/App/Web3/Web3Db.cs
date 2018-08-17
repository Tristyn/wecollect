using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Contracts.Contracts.Cards;
using Contracts.Contracts.Test;
using Nethereum.Web3;

namespace WeCollect.App.Web3
{
    public class Web3Db
    {
        private Nethereum.Web3.Web3 _web3 { get; }
        public ContractArtifacts Contracts { get; }
        public CardsService Cards { get; }
        public string ServerAddress { get; }
        public string ServerPrivateKey { get; }



        public Web3Db(
            Nethereum.Web3.Web3 web3,
            ContractArtifacts contracts,
            CardsService cardsService,
            string serverAddress,
            string serverPrivateKey)
        {
            _web3 = web3;
            Contracts = contracts;
            Cards = cardsService;
            ServerAddress = serverAddress;
            ServerPrivateKey = serverPrivateKey;
        }
    }
}
