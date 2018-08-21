using Contracts.Contracts.Cards.ContractDefinition;
using Nethereum.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WeCollect.App.Bll
{
    public class ContractEventsController
    {
        private readonly Container _container;

        public ContractEventsController(Container container)
        {
            _container = container;
        }

        public async Task OnCardMiningCollected(List<EventLog<OnCardMiningCollectedEventDTO>> cardsMiningCollected)
        {
            await Task.Delay(0);
        }

        public async Task OnBoughtMiningLevel(EventLog<OnBoughtMiningLevelEventDTO> boughtMiningLevel)
        {
            await Task.Delay(0);
        }

        public async Task OnCardBought(EventLog<OnBoughtCardEventDTO> boughtCard)
        {
            await Task.Delay(0);
        }

        public async Task OnCardCreated(EventLog<OnCardCreatedEventDTO> eventLog)
        {
            await Task.Delay(0);
        }
    }
}
