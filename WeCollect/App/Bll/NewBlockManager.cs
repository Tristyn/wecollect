using Contracts.Contracts.Cards.ContractDefinition;
using Nethereum.Contracts;
using Nethereum.Hex.HexTypes;
using Nethereum.RPC.Eth.DTOs;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using System.Threading;
using System.Threading.Tasks;

namespace WeCollect.App.Bll
{
    public class NewBlockManager
    {
        private readonly Event<OnCardCreatedEventDTO> _cardCreatedEvent;
        private readonly Event<OnBoughtCardEventDTO> _boughtCardEvent;
        private readonly Event<OnBoughtMiningLevelEventDTO> _boughtMiningLevelEvent;
        private readonly Event<OnCardMiningCollectedEventDTO> _cardMiningCollectedEvent;

        private readonly CardEventsController _cardsController;

        public NewBlockManager(Container container, CardEventsController cardEventsController)
        {
            var cards = container.Web3.Eth.GetContract(container.ContractArtifacts.Cards.Abi, container.Web3Db.Cards.ContractHandler.ContractAddress);

            _cardCreatedEvent = cards.GetEvent<OnCardCreatedEventDTO>("OnCardCreated");
            _boughtCardEvent = cards.GetEvent<OnBoughtCardEventDTO>("OnBoughtCard");
            _boughtMiningLevelEvent = cards.GetEvent<OnBoughtMiningLevelEventDTO>("OnBoughtMiningLevel");
            _cardMiningCollectedEvent = cards.GetEvent<OnCardMiningCollectedEventDTO>("OnCardMiningCollected");
            _cardsController = cardEventsController;
        }
    
        public async Task OnBlock(BigInteger blockId)
        {
            var cardCreatedFilter = _cardCreatedEvent.CreateFilterInput(blockId);
            var boughtCardFilter = _boughtCardEvent.CreateFilterInput(blockId);
            var boughtMiningLevelFilter = _boughtMiningLevelEvent.CreateFilterInput(blockId);
            var miningCollectedFilter = _cardMiningCollectedEvent.CreateFilterInput(blockId);
            
            while (true)
            {
                try
                {
                    var cardsCreated = await _cardCreatedEvent.GetAllChanges(cardCreatedFilter);
                    var boughtCards = await _boughtCardEvent.GetAllChanges(boughtCardFilter);
                    var boughtMiningLevels = await _boughtMiningLevelEvent.GetAllChanges(boughtMiningLevelFilter);
                    var cardsMiningCollected = await _cardMiningCollectedEvent.GetAllChanges(miningCollectedFilter);


                    foreach (var cardCreated in cardsCreated)
                    {
                        await _cardsController.OnCardCreated(cardCreated);
                    }
                    foreach (var boughtCard in boughtCards)
                    {
                        await _cardsController.OnCardBought(boughtCard);
                    }
                    foreach (var boughtMiningLevel in boughtMiningLevels)
                    {
                        await _cardsController.OnBoughtMiningLevel(boughtMiningLevel);
                    }
                    foreach (var cardMiningCollected in cardsMiningCollected)
                    {
                        await _cardsController.OnCardMiningCollected(cardsMiningCollected);
                    }
                    break;
                }
#pragma warning disable CS0168 // Variable is declared but never used
                catch (Exception ex)
#pragma warning restore CS0168 // Variable is declared but never used
                {
                    if (Debugger.IsAttached)
                    {
                        Debugger.Break();
                    }
                    else
                    {
                        Thread.Sleep(5*1024);
                    }
                }
            }
        }
    }
}
