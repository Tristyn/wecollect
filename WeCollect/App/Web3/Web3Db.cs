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
        public Nethereum.Web3.Web3 Web3 { get; }
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
            Web3 = web3;
            Contracts = contracts;
            Cards = cardsService;
            ServerAddress = serverAddress;
            ServerPrivateKey = serverPrivateKey;
        }




        public async Task UnlockServerAccount(ulong? durationInSeconds)
        {
            bool unlockAccountResult = await Web3.Personal.UnlockAccount.SendRequestAsync(
                ServerAddress,
                ServerPrivateKey,
                durationInSeconds);
            if (!unlockAccountResult)
            {
                throw new Exception("Failed to unlock server account");
            }
        }
        
    }
}
