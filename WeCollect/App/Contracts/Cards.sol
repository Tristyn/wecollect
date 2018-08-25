pragma solidity ^0.4.24;

contract Cards {
    event OnCardCreated(
        uint indexed blockId,
        int32 id,

        address owner,
        address firstOwner,
        uint256 price,
        uint256 miningLastCollectedDate,
        int32 miningLevel,
        int32[7] parentCards
    );
    event OnBoughtCard(
        uint indexed blockId,
        int32 id,

        address owner,
        uint256 price,
        uint256 miningLastCollectedDate,
        int32 miningLevel,

        uint256 buyingPrice,
        uint256 nextAskingPrice,
        uint256 miningRatePerBlock,
        uint256 miningCollected
    );
    event OnBoughtMiningLevel(
        uint indexed blockId,
        int32 id,
        
        uint256 miningLastCollectedDate,
        int32 miningLevel,

        uint256 wccPrice,
        uint256 miningCollected,
        uint256 spentOnUpgrade
    );
    
    event OnCardMiningCollected(
        uint indexed blockId,
        int32 id,

        uint256 miningLastCollectedDate,
        int32 miningLevel,

        uint256 date,
        uint256 amount 
    );


    struct Card {
        address owner;
        address firstOwner;
        uint256 price;
        uint256 miningLastCollectedDate;
        int32 miningLevel;
        int32[7] parentCards;
    }


    mapping(int32 => Card) cards;
    int32 cardsLength;

    address _owner;

    constructor () public {
        _owner = msg.sender;
    }

    function mintCard(
        address owner,
        address firstOwner,
        uint256 price,
        uint256 miningLastCollectedDate,
        int32 miningLevel,
        int32[7] parentCards
    ) public {
        require(msg.sender == _owner, "E1: msg.sender!=owner");

        cardsLength++;
        int32 cardId = cardsLength;

        cards[cardId] = Card({
            owner:owner,
            firstOwner:firstOwner,
            price:price,
            miningLastCollectedDate:miningLastCollectedDate,
            miningLevel:miningLevel,
            parentCards:parentCards
        });

        emit OnCardCreated(
            block.number,
            cardId,
            owner,
            firstOwner,
            price,
            miningLastCollectedDate,
            miningLevel,
            parentCards
        );
    }

    function buyCard2(int32 cardId) public {
        
    }

    function buyCard(int32 cardId) public payable {
        Card memory card = cards[cardId];

        require(msg.value == card.price, "E2: msg.value!=card.price");
        

        card.owner = msg.sender;
        card = upgradeCard(card);


        emit OnBoughtCard({
            blockId: block.number,
            id: cardId,

            owner:card.owner,
            price:card.price,
            miningLastCollectedDate:card.miningLastCollectedDate,
            miningLevel:card.miningLevel,

            buyingPrice: msg.value,
            nextAskingPrice: getNextPrice(card),
            miningRatePerBlock: getMiningRate(card),
            miningCollected: 0
        });
    }

    function buyCardMiningLevel(int32 cardId) public payable {
        
        Card memory card = cards[cardId];

        uint256 oldPrice = card.price;

        uint256 miningLevelPrice = getMiningLevelPrice(card);

        require(msg.sender == card.owner, "E3: msg.sender!=card.owner");
        require(msg.value == miningLevelPrice, "E4:msg.value!=miningLevelPrice");
        // actually require owner has WCC
        
        card = upgradeMiningLevel(card);

        // getMiningRate(card);
        emit OnBoughtMiningLevel({
            blockId: block.number,
            id: cardId,
            
            //card
            miningLastCollectedDate:card.miningLastCollectedDate,
            miningLevel:card.miningLevel,

            wccPrice: oldPrice,
            miningCollected: 0,
            spentOnUpgrade: 0
        });
    }

    function claimMining(int32 cardId) public {

        Card memory card = cards[cardId];

        emit OnCardMiningCollected({
            blockId: block.number,
            id: cardId,

            //card
            miningLastCollectedDate:card.miningLastCollectedDate,
            miningLevel:card.miningLevel,

            date:block.timestamp,
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

    function mul(uint256 a, uint256 b) internal pure returns (uint256) {
        if (a == 0) {
            return 0;
        }
        uint256 c = a * b;
        assert(c / a == b);
        return c;
    }

    function div(uint256 a, uint256 b) internal pure returns (uint256) {
        uint256 c = a / b;
        return c;
    }

    function sub(uint256 a, uint256 b) internal pure returns (uint256) {
        assert(b <= a);
        return a - b;
    }

    function add(uint256 a, uint256 b) internal pure returns (uint256) {
        uint256 c = a + b;
        assert(c >= a);
        return c;
    }
}