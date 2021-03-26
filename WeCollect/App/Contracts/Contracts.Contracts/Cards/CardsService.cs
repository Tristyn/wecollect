using System.Threading.Tasks;
using System.Collections.Generic;
using System.Numerics;
using Nethereum.RPC.Eth.DTOs;
using Nethereum.Contracts.ContractHandlers;
using System.Threading;
using Contracts.Contracts.Cards.ContractDefinition;
namespace Contracts.Contracts.Cards
{

    public partial class CardsService
    {
    
        public static Task<TransactionReceipt> DeployContractAndWaitForReceiptAsync(Nethereum.Web3.Web3 web3, CardsDeployment cardsDeployment, CancellationTokenSource cancellationTokenSource = null)
        {
            return web3.Eth.GetContractDeploymentHandler<CardsDeployment>().SendRequestAndWaitForReceiptAsync(cardsDeployment, cancellationTokenSource);
        }
        public static Task<string> DeployContractAsync(Nethereum.Web3.Web3 web3, CardsDeployment cardsDeployment)
        {
            return web3.Eth.GetContractDeploymentHandler<CardsDeployment>().SendRequestAsync(cardsDeployment);
        }
        public static async Task<CardsService> DeployContractAndGetServiceAsync(Nethereum.Web3.Web3 web3, CardsDeployment cardsDeployment, CancellationTokenSource cancellationTokenSource = null)
        {
            var receipt = await DeployContractAndWaitForReceiptAsync(web3, cardsDeployment, cancellationTokenSource);
            return new CardsService(web3, receipt.ContractAddress);
        }
    
        protected Nethereum.Web3.Web3 Web3{ get; }
        
        public ContractHandler ContractHandler { get; }
        
        public CardsService(Nethereum.Web3.Web3 web3, string contractAddress)
        {
            Web3 = web3;
            ContractHandler = web3.Eth.GetContractHandler(contractAddress);
        }
    
        public async Task<string> ClaimMiningRequestAsync(ClaimMiningFunction claimMiningFunction)
        {
             return await ContractHandler.SendRequestAsync(claimMiningFunction);
        }

        public async Task<TransactionReceipt> ClaimMiningRequestAndWaitForReceiptAsync(ClaimMiningFunction claimMiningFunction, CancellationTokenSource cancellationToken = null)
        {
             return await ContractHandler.SendRequestAndWaitForReceiptAsync(claimMiningFunction, cancellationToken);
        }

        public async Task<string> ClaimMiningRequestAsync(int cardId)
        {
            var claimMiningFunction = new ClaimMiningFunction();
                claimMiningFunction.CardId = cardId;
            
             return await ContractHandler.SendRequestAsync(claimMiningFunction);
        }

        public async Task<TransactionReceipt> ClaimMiningRequestAndWaitForReceiptAsync(int cardId, CancellationTokenSource cancellationToken = null)
        {
            var claimMiningFunction = new ClaimMiningFunction();
                claimMiningFunction.CardId = cardId;
            
             return await ContractHandler.SendRequestAndWaitForReceiptAsync(claimMiningFunction, cancellationToken);
        }

        public async Task<string> BuyCardRequestAsync(BuyCardFunction buyCardFunction)
        {
             return await ContractHandler.SendRequestAsync(buyCardFunction);
        }

        public async Task<TransactionReceipt> BuyCardRequestAndWaitForReceiptAsync(BuyCardFunction buyCardFunction, CancellationTokenSource cancellationToken = null)
        {
             return await ContractHandler.SendRequestAndWaitForReceiptAsync(buyCardFunction, cancellationToken);
        }

        public async Task<string> BuyCardRequestAsync(int cardId)
        {
            var buyCardFunction = new BuyCardFunction();
                buyCardFunction.CardId = cardId;
            
             return await ContractHandler.SendRequestAsync(buyCardFunction);
        }

        public async Task<TransactionReceipt> BuyCardRequestAndWaitForReceiptAsync(int cardId, CancellationTokenSource cancellationToken = null)
        {
            var buyCardFunction = new BuyCardFunction();
                buyCardFunction.CardId = cardId;
            
             return await ContractHandler.SendRequestAndWaitForReceiptAsync(buyCardFunction, cancellationToken);
        }

        public async Task<string> BuyCard2RequestAsync(BuyCard2Function buyCard2Function)
        {
             return await ContractHandler.SendRequestAsync(buyCard2Function);
        }

        public async Task<TransactionReceipt> BuyCard2RequestAndWaitForReceiptAsync(BuyCard2Function buyCard2Function, CancellationTokenSource cancellationToken = null)
        {
             return await ContractHandler.SendRequestAndWaitForReceiptAsync(buyCard2Function, cancellationToken);
        }

        public async Task<string> BuyCard2RequestAsync(int cardId)
        {
            var buyCard2Function = new BuyCard2Function();
                buyCard2Function.CardId = cardId;
            
             return await ContractHandler.SendRequestAsync(buyCard2Function);
        }

        public async Task<TransactionReceipt> BuyCard2RequestAndWaitForReceiptAsync(int cardId, CancellationTokenSource cancellationToken = null)
        {
            var buyCard2Function = new BuyCard2Function();
                buyCard2Function.CardId = cardId;
            
             return await ContractHandler.SendRequestAndWaitForReceiptAsync(buyCard2Function, cancellationToken);
        }

        public async Task<string> BuyCardMiningLevelRequestAsync(BuyCardMiningLevelFunction buyCardMiningLevelFunction)
        {
             return await ContractHandler.SendRequestAsync(buyCardMiningLevelFunction);
        }

        public async Task<TransactionReceipt> BuyCardMiningLevelRequestAndWaitForReceiptAsync(BuyCardMiningLevelFunction buyCardMiningLevelFunction, CancellationTokenSource cancellationToken = null)
        {
             return await ContractHandler.SendRequestAndWaitForReceiptAsync(buyCardMiningLevelFunction, cancellationToken);
        }

        public async Task<string> BuyCardMiningLevelRequestAsync(int cardId)
        {
            var buyCardMiningLevelFunction = new BuyCardMiningLevelFunction();
                buyCardMiningLevelFunction.CardId = cardId;
            
             return await ContractHandler.SendRequestAsync(buyCardMiningLevelFunction);
        }

        public async Task<TransactionReceipt> BuyCardMiningLevelRequestAndWaitForReceiptAsync(int cardId, CancellationTokenSource cancellationToken = null)
        {
            var buyCardMiningLevelFunction = new BuyCardMiningLevelFunction();
                buyCardMiningLevelFunction.CardId = cardId;
            
             return await ContractHandler.SendRequestAndWaitForReceiptAsync(buyCardMiningLevelFunction, cancellationToken);
        }

        public async Task<string> MintCardRequestAsync(MintCardFunction mintCardFunction)
        {
             return await ContractHandler.SendRequestAsync(mintCardFunction);
        }

        public async Task<TransactionReceipt> MintCardRequestAndWaitForReceiptAsync(MintCardFunction mintCardFunction, CancellationTokenSource cancellationToken = null)
        {
             return await ContractHandler.SendRequestAndWaitForReceiptAsync(mintCardFunction, cancellationToken);
        }

        public async Task<string> MintCardRequestAsync(string owner, string firstOwner, BigInteger price, BigInteger miningLastCollectedDate, int miningLevel, List<int> parentCards)
        {
            var mintCardFunction = new MintCardFunction();
                mintCardFunction.Owner = owner;
                mintCardFunction.FirstOwner = firstOwner;
                mintCardFunction.Price = price;
                mintCardFunction.MiningLastCollectedDate = miningLastCollectedDate;
                mintCardFunction.MiningLevel = miningLevel;
                mintCardFunction.ParentCards = parentCards;
            
             return await ContractHandler.SendRequestAsync(mintCardFunction);
        }

        public async Task<TransactionReceipt> MintCardRequestAndWaitForReceiptAsync(string owner, string firstOwner, BigInteger price, BigInteger miningLastCollectedDate, int miningLevel, List<int> parentCards, CancellationTokenSource cancellationToken = null)
        {
            var mintCardFunction = new MintCardFunction();
                mintCardFunction.Owner = owner;
                mintCardFunction.FirstOwner = firstOwner;
                mintCardFunction.Price = price;
                mintCardFunction.MiningLastCollectedDate = miningLastCollectedDate;
                mintCardFunction.MiningLevel = miningLevel;
                mintCardFunction.ParentCards = parentCards;
            
             return await ContractHandler.SendRequestAndWaitForReceiptAsync(mintCardFunction, cancellationToken);
        }
    }
}
