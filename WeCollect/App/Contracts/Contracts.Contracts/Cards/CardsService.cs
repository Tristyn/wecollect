using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Numerics;
using Nethereum.Hex.HexTypes;
using Nethereum.ABI.FunctionEncoding.Attributes;
using Nethereum.Web3;
using Nethereum.RPC.Eth.DTOs;
using Nethereum.Contracts.CQS;
using Nethereum.Contracts.ContractHandlers;
using Nethereum.Contracts;
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
    
        public Task<string> MintCardRequestAsync(MintCardFunction mintCardFunction)
        {
             return ContractHandler.SendRequestAsync(mintCardFunction);
        }

        public Task<TransactionReceipt> MintCardRequestAndWaitForReceiptAsync(MintCardFunction mintCardFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(mintCardFunction, cancellationToken);
        }

        public Task<string> MintCardRequestAsync(ContractCardDto card)
        {
            var mintCardFunction = new MintCardFunction();
                mintCardFunction.Card = card;
            
             return ContractHandler.SendRequestAsync(mintCardFunction);
        }

        public Task<TransactionReceipt> MintCardRequestAndWaitForReceiptAsync(ContractCardDto card, CancellationTokenSource cancellationToken = null)
        {
            var mintCardFunction = new MintCardFunction();
                mintCardFunction.Card = card;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(mintCardFunction, cancellationToken);
        }

        public Task<string> BuyCardMiningLevelRequestAsync(BuyCardMiningLevelFunction buyCardMiningLevelFunction)
        {
             return ContractHandler.SendRequestAsync(buyCardMiningLevelFunction);
        }

        public Task<TransactionReceipt> BuyCardMiningLevelRequestAndWaitForReceiptAsync(BuyCardMiningLevelFunction buyCardMiningLevelFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(buyCardMiningLevelFunction, cancellationToken);
        }

        public Task<string> BuyCardMiningLevelRequestAsync(uint cardId)
        {
            var buyCardMiningLevelFunction = new BuyCardMiningLevelFunction();
                buyCardMiningLevelFunction.CardId = cardId;
            
             return ContractHandler.SendRequestAsync(buyCardMiningLevelFunction);
        }

        public Task<TransactionReceipt> BuyCardMiningLevelRequestAndWaitForReceiptAsync(uint cardId, CancellationTokenSource cancellationToken = null)
        {
            var buyCardMiningLevelFunction = new BuyCardMiningLevelFunction();
                buyCardMiningLevelFunction.CardId = cardId;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(buyCardMiningLevelFunction, cancellationToken);
        }

        public Task<string> ClaimMiningRequestAsync(ClaimMiningFunction claimMiningFunction)
        {
             return ContractHandler.SendRequestAsync(claimMiningFunction);
        }

        public Task<TransactionReceipt> ClaimMiningRequestAndWaitForReceiptAsync(ClaimMiningFunction claimMiningFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(claimMiningFunction, cancellationToken);
        }

        public Task<string> ClaimMiningRequestAsync(uint cardId)
        {
            var claimMiningFunction = new ClaimMiningFunction();
                claimMiningFunction.CardId = cardId;
            
             return ContractHandler.SendRequestAsync(claimMiningFunction);
        }

        public Task<TransactionReceipt> ClaimMiningRequestAndWaitForReceiptAsync(uint cardId, CancellationTokenSource cancellationToken = null)
        {
            var claimMiningFunction = new ClaimMiningFunction();
                claimMiningFunction.CardId = cardId;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(claimMiningFunction, cancellationToken);
        }

        public Task<string> BuyCardRequestAsync(BuyCardFunction buyCardFunction)
        {
             return ContractHandler.SendRequestAsync(buyCardFunction);
        }

        public Task<TransactionReceipt> BuyCardRequestAndWaitForReceiptAsync(BuyCardFunction buyCardFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(buyCardFunction, cancellationToken);
        }

        public Task<string> BuyCardRequestAsync(uint cardId)
        {
            var buyCardFunction = new BuyCardFunction();
                buyCardFunction.CardId = cardId;
            
             return ContractHandler.SendRequestAsync(buyCardFunction);
        }

        public Task<TransactionReceipt> BuyCardRequestAndWaitForReceiptAsync(uint cardId, CancellationTokenSource cancellationToken = null)
        {
            var buyCardFunction = new BuyCardFunction();
                buyCardFunction.CardId = cardId;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(buyCardFunction, cancellationToken);
        }
    }
}
