using Contracts.Contracts.Cards;
using Contracts.Contracts.Cards.ContractDefinition;
using Nethereum.Contracts;

namespace WeCollect.App.Web3
{
    public class Web3Db
    {
        public static Web3Db web3Db;
        public static Nethereum.Web3.Web3 web3;

        private Nethereum.Web3.Web3 _web3 { get; }
        public ContractArtifacts Contracts { get; }
        public CardsService Cards { get; }
        public string ServerAddress { get; }
        public string ServerPrivateKey { get; }

        public Event<OnCardCreatedEventDTO> CardCreatedEvent { get; set; }
        public Event<OnBoughtCardEventDTO> BoughtCardEvent { get; set; }
        public Event<OnBoughtMiningLevelEventDTO> BoughtMiningLevelEvent { get; set; }
        public Event<OnCardMiningCollectedEventDTO> CardMiningCollectedEvent { get; set; }

        public Web3Db(
            Nethereum.Web3.Web3 web3,
            ContractArtifacts contracts,
            CardsService cardsService,
            string serverAddress,
            string serverPrivateKey)
        {
            _web3 = web3;
            Contracts = contracts;
            Cards = cardsService;
            ServerAddress = serverAddress;
            ServerPrivateKey = serverPrivateKey;


            var cards = web3.Eth.GetContract(contracts.Cards.Abi, cardsService.ContractHandler.ContractAddress);
            
            CardCreatedEvent = cards.GetEvent<OnCardCreatedEventDTO>();
            BoughtCardEvent = cards.GetEvent<OnBoughtCardEventDTO>();
            BoughtMiningLevelEvent = cards.GetEvent<OnBoughtMiningLevelEventDTO>();
            CardMiningCollectedEvent = cards.GetEvent<OnCardMiningCollectedEventDTO>();
        }

    }
}
