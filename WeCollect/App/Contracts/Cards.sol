pragma solidity ^0.4.24;
pragma experimental ABIEncoderV2;

contract Cards {
    event OnCardCreated(
        uint32 indexed id,
        Card card
    );
    event OnBoughtCard(
        uint32 indexed id,
        Card card,
        uint256 buyingPrice,
        uint256 nextAskingPrice,
        uint256 miningRatePerBlock,
        uint256 miningCollected
    );
    event OnBoughtMiningLevel(
        uint32 indexed id,
        Card card,
        uint256 wccPrice,
        uint256 miningCollected,
        uint256 spentOnUpgrade
    );
    
    event OnCardMiningCollected(
        uint32 indexed id,
        Card card,
        uint256 amount 
    );


    struct Card {
        address owner;
        address firstOwner;
        uint256 price;
        uint256 miningLastCollectedDate;
        uint32 miningLevel;
        uint32[7] parentCards;
    }


    mapping(uint32 => Card) cards;
    uint32 cardsLength;

    address owner;

    constructor () public {
        owner = msg.sender;
    }

    function mintCard(Card card) public {
        require(msg.sender == owner);

        uint32 cardId = cardsLength;
        cardsLength++;

        cards[cardId] = card;

        emit OnCardCreated(cardId, card);
    }

    function buyCard(uint32 cardId) public payable {
        Card memory card = cards[cardId];

        require(msg.value == card.price);
        

        card.owner = msg.sender;
        card = upgradeCard(card);


        emit OnBoughtCard({
            id: cardId,
            card: card,
            buyingPrice: msg.value,
            nextAskingPrice: getNextPrice(card),
            miningRate: getMiningRate(card),
            miningCollected: 0
        });
    }

    function buyCardMiningLevel(uint32 cardId) public payable {
        
        Card memory card = cards[cardId];

        uint256 oldPrice = card.price;

        uint256 miningLevelPrice = getMiningLevelPrice(card);

        require(msg.sender == card.owner);
        require(msg.value == miningLevelPrice);
        // actually require owner has WCC
        
        card = upgradeMiningLevel(card);

        // getMiningRate(card);
        emit OnBoughtMiningLevel({
            id: cardId,
            card: card,
            wccPrice: oldPrice,
            miningCollected: 0,
            spentOnUpgrade: 0
        });
    }

    function claimMining(uint32 cardId) public {

        Card memory card = cards[cardId];

        emit OnCardMiningCollected({
            id: cardId,
            card: card,
            amount: 0
        });
    }




    function upgradeCard(Card memory card) private returns (Card memory) {
        card.miningLevel = card.miningLevel++;
        card.price = getNextPrice(card);
    }

    function getNextPrice(Card memory card) private returns (uint256) {
        return 0;
    }


    function upgradeMiningLevel(Card memory card) private returns (Card memory)  {
        card.miningLevel++;
    }



    function getMiningRate(Card memory card) private returns (uint256) {
        return 0;
    }

    function getMiningLevelPrice(Card memory card) private returns (uint256) {
        return 0;
    }

    function getCollectionAmount(Card card) private returns (uint256) {
        return 0;
    }


}