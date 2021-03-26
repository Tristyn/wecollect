using Contracts.Contracts.Cards.ContractDefinition;
using Nethereum.Contracts;
using System.Threading.Tasks;
using WeCollect.App.Documents;
using WeCollect.Server.Hubs;
using WeCollect.Server.Models;

namespace WeCollect.App.Bll
{
    public class ContractEventsHandler
    {
        private readonly Container _container;
        private readonly CardDb _documents;

        public ContractEventsHandler(Container container)
        {
            _container = container;
            _documents = container.Documents;
        }

        public async Task OnCardCreated(EventLog<OnCardCreatedEventDTO> eventLog)
        {
            var log = new CardEventLog<OnCardCreatedEventDTO>(await _documents.GetCardWithCardsContractId(eventLog.Event.Id), eventLog);
            await GlobalHubContext.CardHub.Clients.All.OnCardCreated(log);
        }

        public async Task OnCardBought(EventLog<OnBoughtCardEventDTO> eventLog)
        {
            var log = new CardEventLog<OnBoughtCardEventDTO>(await _documents.GetCardWithCardsContractId(eventLog.Event.Id), eventLog);
            await GlobalHubContext.CardHub.Clients.All.OnBoughtCard(log);
        }

        public async Task OnBoughtMiningLevel(EventLog<OnBoughtMiningLevelEventDTO> eventLog)
        {
            var log = new CardEventLog<OnBoughtMiningLevelEventDTO>(await _documents.GetCardWithCardsContractId(eventLog.Event.Id), eventLog);
            await GlobalHubContext.CardHub.Clients.All.OnBoughtMiningLevel(log);
        }

        public async Task OnCardMiningCollected(EventLog<OnCardMiningCollectedEventDTO> eventLog)
        {
            var log = new CardEventLog<OnCardMiningCollectedEventDTO>(await _documents.GetCardWithCardsContractId(eventLog.Event.Id), eventLog);
            await GlobalHubContext.CardHub.Clients.All.OnCardMiningCollected(log);
        }
    }
}
