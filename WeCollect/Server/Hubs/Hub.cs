using Contracts.Contracts.Cards.ContractDefinition;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WeCollect.Server.Models;

namespace WeCollect.Server.Hubs
{
    // IHubContext<CardHub>
    public class CardHub : Hub<ICardHubClient>
    {
        public const string AllGroup = "All";

        public async Task OnCardCreated(CardEventLog<OnCardCreatedEventDTO> cardCreated)
        {
            await Clients.Group(AllGroup)
                .OnCardCreated(cardCreated);
        }

        public async Task OnBoughtCard(CardEventLog<OnBoughtCardEventDTO> cardBought)
        {
            await Clients.Group(AllGroup)
                .OnBoughtCard(cardBought);
        }

        public async Task OnBoughtMiningLevel(CardEventLog<OnBoughtMiningLevelEventDTO> cardMiningLevelBought)
        {
            await Clients.Group(AllGroup)
                .OnBoughtMiningLevel(cardMiningLevelBought);
        }

        public async Task OnCardMiningCollected(CardEventLog<OnCardMiningCollectedEventDTO> cardMiningCollected)
        {
            await Clients.Group(AllGroup)
                .OnCardMiningCollected(cardMiningCollected);
        }
    }

    public interface ICardHubClient
    {
        Task OnCardCreated(CardEventLog<OnCardCreatedEventDTO> onCardCreated);

        Task OnBoughtCard(CardEventLog<OnBoughtCardEventDTO> onBoughtCard);

        Task OnBoughtMiningLevel(CardEventLog<OnBoughtMiningLevelEventDTO> onBoughtMiningLevel);

        Task OnCardMiningCollected(CardEventLog<OnCardMiningCollectedEventDTO> onCardMiningCollected);
    }
}
