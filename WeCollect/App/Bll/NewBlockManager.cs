using System;
using System.Diagnostics;
using System.Numerics;
using System.Threading;
using System.Threading.Tasks;
using WeCollect.App.Web3;

namespace WeCollect.App.Bll
{
    public class NewBlockManager
    {

        private readonly ContractEventsHandler _cardsController;
        private readonly Web3Db _web3;

        public NewBlockManager(Container container, ContractEventsHandler cardEventsController)
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
                        continue;
                    }
                    else
                    {
                        Thread.Sleep(5000);
                    }
                }
            }
        }
    }
}
