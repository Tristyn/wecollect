using Contracts.Contracts.Cards.ContractDefinition;
using Nethereum.Contracts;
using Nethereum.Hex.HexTypes;
using Nethereum.RPC.Eth.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;

namespace WeCollect.App.Bll
{
    public class NewBlockManager
    {
        private readonly Event<OnCardCreatedEventDTO> _cardCreatedEvent;
        private readonly Event<OnBoughtCardEventDTO> _boughtCardEvent;
        private readonly Event<OnCardMiningCollectedEventDTO> _cardMiningCollected;
        private readonly Event<OnBoughtMiningLevelEventDTO> _boughtMiningLevel;

        private readonly CardEventsController _cardsController;

        public NewBlockManager(Container container, CardEventsController cardEventsController)
        {
            var cards = container.Web3.Eth.GetContract(container.ContractArtifacts.Cards.Abi, container.Web3Db.Cards.ContractHandler.ContractAddress);

            _cardCreatedEvent = cards.GetEvent<OnCardCreatedEventDTO>("OnCardCreated");
            _boughtCardEvent = cards.GetEvent<OnBoughtCardEventDTO>("OnBoughtCard");
            _boughtMiningLevel = cards.GetEvent<OnBoughtMiningLevelEventDTO>("OnBoughtMiningLevel");
            _cardMiningCollected = cards.GetEvent<OnCardMiningCollectedEventDTO>("OnCardMiningCollected");
            _cardsController = cardEventsController;
        }
    
        public async Task OnBlock(BigInteger blockId)
        {
            var blockParam = new BlockParameter(new HexBigInteger(blockId));
            var blockFilter = new NewFilterInput { FromBlock = blockParam, ToBlock = blockParam };

            var cardsCreated = await _cardCreatedEvent.GetAllChanges(blockFilter);
            foreach (var cardCreated in cardsCreated)
            {
                await _cardsController.OnCardCreated(cardCreated);
            }

            var boughtCards = await _boughtCardEvent.GetAllChanges(blockFilter);
            foreach(var boughtCard in boughtCards)
            {
                await _cardsController.OnCardBought(boughtCard);
            }

            var boughtMiningLevels = await _boughtMiningLevel.GetAllChanges(blockFilter);
            foreach(var boughtMiningLevel in boughtMiningLevels)
            {
                await _cardsController.OnBoughtMiningLevel(boughtMiningLevel);
            }

            var cardsMiningCollected = await _cardMiningCollected.GetAllChanges(blockFilter);
            foreach(var cardMiningCollected in cardsMiningCollected)
            {
                await _cardsController.OnCardMiningCollected(cardsMiningCollected);
            }
        }
    }
}
