using System.Collections.Generic;
using Contracts.Contracts.Cards;
using WeCollect.App.Bll;
using WeCollect.App.Documents;
using WeCollect.App.Models;
using WeCollect.App.Web3;

namespace WeCollect.App
{
    public class Container
    {
        public Nethereum.Web3.Web3 Web3 { get; set; }

        public Web3Db Web3Db { get; set; }
        
        public CardDocumentDb Documents { get; set; }

        public ContractArtifacts ContractArtifacts { get; set; }

        public ContractsInitializer ContractsInitializer { get; set; }

        public Dictionary<string, ContractDto> ContractDocuments { get; set; }

        public CardsService CardsContractMethods { get; set; }

        public CardEventsController CardEventsController { get; set; }
    }
}
