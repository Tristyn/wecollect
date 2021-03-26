using System.Collections.Generic;
using System.Numerics;
using Nethereum.ABI.FunctionEncoding.Attributes;
using Nethereum.Contracts;
namespace Contracts.Contracts.Cards.ContractDefinition
{


    public partial class CardsDeployment:CardsDeploymentBase
    {
        public CardsDeployment():base(BYTECODE) { }
        
        public CardsDeployment(string byteCode):base(byteCode) { }
    }

    public class CardsDeploymentBase:ContractDeploymentMessage
    {
        
        public static string BYTECODE = "608060405234801561001057600080fd5b50600180547fffffffffffffffff0000000000000000000000000000000000000000ffffffff163364010000000002179055610a33806100516000396000f30060806040526004361061006c5763ffffffff7c0100000000000000000000000000000000000000000000000000000000600035041663351d7289811461007157806370d2aefe1461008e57806370d67cdc1461009c57806387dcff6d146100b7578063c6650b08146100c5575b600080fd5b34801561007d57600080fd5b5061008c60043560030b61012b565b005b61008c60043560030b610262565b3480156100a857600080fd5b5061008c60043560030b61044d565b61008c60043560030b610450565b3480156100d157600080fd5b506040805160e081810190925261008c91600160a060020a036004803582169360243590921692604435926064359260843560030b9236926101849160a49060079083908390808284375093965061066095505050505050565b6101336108d3565b600382810b810b600090815260208181526040808320815160c0810183528154600160a060020a039081168252600183015416938101939093526002810154838301528085015460608401526004810154850b850b90940b6080830152805160e081019182905291939260a0850192916005850191600791908390855b82829054906101000a900460030b60030b815260200190600401906020826003010492830192600103820291508084116101b05790505050505050815250509050437fabd13b97ffe909cc9cc67f05ec77abb114ba7610375bcfd0d25654dc629331478383606001518460800151426000604051808660030b60030b81526020018581526020018460030b60030b81526020018381526020018281526020019550505050505060405180910390a25050565b61026a6108d3565b600382810b810b600090815260208181526040808320815160c0810183528154600160a060020a039081168252600183015416938101939093526002810154838301528085015460608401526004810154850b850b90940b6080830152805160e081019182905291939260a0850192916005850191600791908390855b82829054906101000a900460030b60030b815260200190600401906020826003010492830192600103820291508084116102e757505050929093525050506040820151919250503414610384576040805160e560020a62461bcd02815260206004820152601960248201527f45323a206d73672e76616c7565213d636172642e707269636500000000000000604482015290519081900360640190fd5b33815261039081610883565b9050437ff01644836e8d77e957deed84dd744a1e82d83169574d25da045963e3a743f6d5838360000151846040015185606001518660800151346103d3896108af565b6103dc8a6108af565b6000604051808a60030b60030b815260200189600160a060020a0316600160a060020a031681526020018881526020018781526020018660030b60030b8152602001858152602001848152602001838152602001828152602001995050505050505050505060405180910390a25050565b50565b6104586108d3565b600382810b810b600090815260208181526040808320815160c0810183528154600160a060020a039081168252600183015416938101939093526002810154838301528085015460608401526004810154850b850b90940b6080830152805160e081019182905292938493909160a08401919060058401906007908288855b82829054906101000a900460030b60030b815260200190600401906020826003010492830192600103820291508084116104d7579050505050505081525050925082604001519150610528836108af565b8351909150600160a060020a0316331461058c576040805160e560020a62461bcd02815260206004820152601a60248201527f45333a206d73672e73656e646572213d636172642e6f776e6572000000000000604482015290519081900360640190fd5b3481146105e3576040805160e560020a62461bcd02815260206004820152601e60248201527f45343a6d73672e76616c7565213d6d696e696e674c6576656c50726963650000604482015290519081900360640190fd5b6105ec836108b5565b6060808201516080808401516040805160038b810b810b8252602082019590955291840b90930b81840152928301869052600090830181905260a08301525191945043917fc6671c37811c03498f38e9e77b1cd96e81b7ddd4c7d244c365ca6ed72b89e3769181900360c00190a250505050565b6001546000906401000000009004600160a060020a031633146106cd576040805160e560020a62461bcd02815260206004820152601560248201527f45313a206d73672e73656e646572213d6f776e65720000000000000000000000604482015290519081900360640190fd5b5060018054600381810b8301810b63ffffffff90811663ffffffff19938416178085556040805160c081018252600160a060020a03808e1682528c811660208084019182528385018e8152606085018e81528d8a0b6080870190815260a087018e8152988b0b808c0b8c0b600090815294859052979093208651815490871673ffffffffffffffffffffffffffffffffffffffff1991821617825594519c810180549d9096169c9094169b909b1790935591516002820155975188870155516004880180549190960b909416939095169290921790925590519092906107b99060058301906007610925565b50905050437faa42459329700fdd87ef3c5867d7537110bed0db0a6cdbce11cc9227ee7936ee82898989898989604051808860030b60030b815260200187600160a060020a0316600160a060020a0316815260200186600160a060020a0316600160a060020a031681526020018581526020018481526020018360030b60030b815260200182600760200280838360005b8381101561086257818101518382015260200161084a565b5050505090500197505050505050505060405180910390a250505050505050565b61088b6108d3565b608082018051600390810b900b90526108a3826108af565b60409092019190915290565b50600090565b6108bd6108d3565b60809091018051600101600390810b900b905290565b610180604051908101604052806000600160a060020a031681526020016000600160a060020a031681526020016000815260200160008152602001600060030b81526020016109206109c4565b905290565b6001830191839082156109b45791602002820160005b8382111561098257835183826101000a81548163ffffffff021916908360030b63ffffffff160217905550926020019260040160208160030104928301926001030261093b565b80156109b25782816101000a81549063ffffffff0219169055600401602081600301049283019260010302610982565b505b506109c09291506109e3565b5090565b60e0604051908101604052806007906020820280388339509192915050565b610a0491905b808211156109c057805463ffffffff191681556001016109e9565b905600a165627a7a72305820c3860623f4b5e22a62d82af1b83b44cfdcc333b877bc8bad9c7acc9f85b30e1c0029";
        
        public CardsDeploymentBase():base(BYTECODE) { }
        
        public CardsDeploymentBase(string byteCode):base(byteCode) { }
        

    }    
    
    public partial class ClaimMiningFunction:ClaimMiningFunctionBase{}

    [Function("claimMining")]
    public class ClaimMiningFunctionBase:FunctionMessage
    {
        [Parameter("int32", "cardId", 1)]
        public virtual int CardId {get; set;}
    }    
    
    public partial class BuyCardFunction:BuyCardFunctionBase{}

    [Function("buyCard")]
    public class BuyCardFunctionBase:FunctionMessage
    {
        [Parameter("int32", "cardId", 1)]
        public virtual int CardId {get; set;}
    }    
    
    public partial class BuyCard2Function:BuyCard2FunctionBase{}

    [Function("buyCard2")]
    public class BuyCard2FunctionBase:FunctionMessage
    {
        [Parameter("int32", "cardId", 1)]
        public virtual int CardId {get; set;}
    }    
    
    public partial class BuyCardMiningLevelFunction:BuyCardMiningLevelFunctionBase{}

    [Function("buyCardMiningLevel")]
    public class BuyCardMiningLevelFunctionBase:FunctionMessage
    {
        [Parameter("int32", "cardId", 1)]
        public virtual int CardId {get; set;}
    }    
    
    public partial class MintCardFunction:MintCardFunctionBase{}

    [Function("mintCard")]
    public class MintCardFunctionBase:FunctionMessage
    {
        [Parameter("address", "owner", 1)]
        public virtual string Owner {get; set;}
        [Parameter("address", "firstOwner", 2)]
        public virtual string FirstOwner {get; set;}
        [Parameter("uint256", "price", 3)]
        public virtual BigInteger Price {get; set;}
        [Parameter("uint256", "miningLastCollectedDate", 4)]
        public virtual BigInteger MiningLastCollectedDate {get; set;}
        [Parameter("int32", "miningLevel", 5)]
        public virtual int MiningLevel {get; set;}
        [Parameter("int32[7]", "parentCards", 6)]
        public virtual List<int> ParentCards {get; set;}
    }    
    
    public partial class OnCardCreatedEventDTO:OnCardCreatedEventDTOBase{}

    [Event("OnCardCreated")]
    public class OnCardCreatedEventDTOBase: IEventDTO
    {
        [Parameter("uint256", "blockId", 1, true )]
        public virtual BigInteger BlockId {get; set;}
        [Parameter("int32", "id", 2, false )]
        public virtual int Id {get; set;}
        [Parameter("address", "owner", 3, false )]
        public virtual string Owner {get; set;}
        [Parameter("address", "firstOwner", 4, false )]
        public virtual string FirstOwner {get; set;}
        [Parameter("uint256", "price", 5, false )]
        public virtual BigInteger Price {get; set;}
        [Parameter("uint256", "miningLastCollectedDate", 6, false )]
        public virtual BigInteger MiningLastCollectedDate {get; set;}
        [Parameter("int32", "miningLevel", 7, false )]
        public virtual int MiningLevel {get; set;}
        [Parameter("int32[7]", "parentCards", 8, false )]
        public virtual List<int> ParentCards {get; set;}
    }    
    
    public partial class OnBoughtCardEventDTO:OnBoughtCardEventDTOBase{}

    [Event("OnBoughtCard")]
    public class OnBoughtCardEventDTOBase: IEventDTO
    {
        [Parameter("uint256", "blockId", 1, true )]
        public virtual BigInteger BlockId {get; set;}
        [Parameter("int32", "id", 2, false )]
        public virtual int Id {get; set;}
        [Parameter("address", "owner", 3, false )]
        public virtual string Owner {get; set;}
        [Parameter("uint256", "price", 4, false )]
        public virtual BigInteger Price {get; set;}
        [Parameter("uint256", "miningLastCollectedDate", 5, false )]
        public virtual BigInteger MiningLastCollectedDate {get; set;}
        [Parameter("int32", "miningLevel", 6, false )]
        public virtual int MiningLevel {get; set;}
        [Parameter("uint256", "buyingPrice", 7, false )]
        public virtual BigInteger BuyingPrice {get; set;}
        [Parameter("uint256", "nextAskingPrice", 8, false )]
        public virtual BigInteger NextAskingPrice {get; set;}
        [Parameter("uint256", "miningRatePerBlock", 9, false )]
        public virtual BigInteger MiningRatePerBlock {get; set;}
        [Parameter("uint256", "miningCollected", 10, false )]
        public virtual BigInteger MiningCollected {get; set;}
    }    
    
    public partial class OnBoughtMiningLevelEventDTO:OnBoughtMiningLevelEventDTOBase{}

    [Event("OnBoughtMiningLevel")]
    public class OnBoughtMiningLevelEventDTOBase: IEventDTO
    {
        [Parameter("uint256", "blockId", 1, true )]
        public virtual BigInteger BlockId {get; set;}
        [Parameter("int32", "id", 2, false )]
        public virtual int Id {get; set;}
        [Parameter("uint256", "miningLastCollectedDate", 3, false )]
        public virtual BigInteger MiningLastCollectedDate {get; set;}
        [Parameter("int32", "miningLevel", 4, false )]
        public virtual int MiningLevel {get; set;}
        [Parameter("uint256", "wccPrice", 5, false )]
        public virtual BigInteger WccPrice {get; set;}
        [Parameter("uint256", "miningCollected", 6, false )]
        public virtual BigInteger MiningCollected {get; set;}
        [Parameter("uint256", "spentOnUpgrade", 7, false )]
        public virtual BigInteger SpentOnUpgrade {get; set;}
    }    
    
    public partial class OnCardMiningCollectedEventDTO:OnCardMiningCollectedEventDTOBase{}

    [Event("OnCardMiningCollected")]
    public class OnCardMiningCollectedEventDTOBase: IEventDTO
    {
        [Parameter("uint256", "blockId", 1, true )]
        public virtual BigInteger BlockId {get; set;}
        [Parameter("int32", "id", 2, false )]
        public virtual int Id {get; set;}
        [Parameter("uint256", "miningLastCollectedDate", 3, false )]
        public virtual BigInteger MiningLastCollectedDate {get; set;}
        [Parameter("int32", "miningLevel", 4, false )]
        public virtual int MiningLevel {get; set;}
        [Parameter("uint256", "date", 5, false )]
        public virtual BigInteger Date {get; set;}
        [Parameter("uint256", "amount", 6, false )]
        public virtual BigInteger Amount {get; set;}
    }    
    
    
    
    
    
    
    
    
    

}
