using Contracts.Contracts.Cards.ContractDefinition;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WeCollect.Server.Hubs
{
    // IHubContext<CardHub>
    public class CardHub : Hub<ICardHubClient>
    {
        public const string AllGroup = "All";

        public async Task OnCardCreated(OnCardCreatedEventDTO cardCreated)
        {
            await Clients.Group(AllGroup)
                .OnCardCreated(cardCreated);
        }

        public async Task OnBoughtCard(OnBoughtCardEventDTO cardBought)
        {
            await Clients.Group(AllGroup)
                .OnBoughtCard(cardBought);
        }

        public async Task OnBoughtMiningLevel(OnBoughtMiningLevelEventDTO cardMiningLevelBought)
        {
            await Clients.Group(AllGroup)
                .OnBoughtMiningLevel(cardMiningLevelBought);
        }

        public async Task OnCardMiningCollected(OnCardMiningCollectedEventDTO cardMiningCollected)
        {
            await Clients.Group(AllGroup)
                .OnCardMiningCollected(cardMiningCollected);
        }
    }

    public interface ICardHubClient
    {
        Task OnCardCreated(OnCardCreatedEventDTO onCardCreated);

        Task OnBoughtCard(OnBoughtCardEventDTO onBoughtCard);

        Task OnBoughtMiningLevel(OnBoughtMiningLevelEventDTO onBoughtMiningLevel);

        Task OnCardMiningCollected(OnCardMiningCollectedEventDTO onCardMiningCollected);
    }
}
