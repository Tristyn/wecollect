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
using WeCollect.App.Web3;

namespace WeCollect.App.Bll
{
    public class NewBlockManager
    {

        private readonly ContractEventsController _cardsController;
        private readonly Web3Db _web3;

        public NewBlockManager(Container container, ContractEventsController cardEventsController)
        {
            _web3 = container.Web3Db;
            _cardsController = cardEventsController;
        }
    
        public async Task OnBlock(BigInteger blockId)
        {
            var cardCreatedFilter = _web3.CardCreatedEvent.CreateFilterInput(blockId);
            var boughtCardFilter = _web3.BoughtCardEvent.CreateFilterInput(blockId);
            var boughtMiningLevelFilter = _web3.BoughtMiningLevelEvent.CreateFilterInput(blockId);
            var miningCollectedFilter = _web3.CardMiningCollectedEvent.CreateFilterInput(blockId);
            
            while (true)
            {
                try
                {
                    var cardsCreated = await _web3.CardCreatedEvent.GetAllChanges(cardCreatedFilter);
                    var boughtCards = await _web3.BoughtCardEvent.GetAllChanges(boughtCardFilter);
                    var boughtMiningLevels = await _web3.BoughtMiningLevelEvent.GetAllChanges(boughtMiningLevelFilter);
                    var cardsMiningCollected = await _web3.CardMiningCollectedEvent.GetAllChanges(miningCollectedFilter);


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
                        await _cardsController.OnCardMiningCollected(cardMiningCollected);
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
