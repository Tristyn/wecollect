using Contracts.Contracts.Cards.ContractDefinition;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WeCollect.App.Web3
{
    public class ContractEvents
    {
        private readonly Web3Db _web3;

        public IObservable<OnCardCreatedEventDTO> OnCardCreated { get; }
        public IObservable<OnBoughtCardEventDTO> OnBoughtCard { get; }
        public IObservable<OnBoughtMiningLevelEventDTO> OnBoughtMiningLevel { get; }
        public IObservable<OnCardMiningCollectedEventDTO> OnCardMiningCollected { get; }

        public ContractEvents(ContractArtifacts contracts, Web3Db web3)
        {
            _web3 = web3;
        }
    }
}
