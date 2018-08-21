using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Numerics;
using Nethereum.Hex.HexTypes;
using Nethereum.ABI.FunctionEncoding.Attributes;
using Nethereum.Web3;
using Nethereum.RPC.Eth.DTOs;
using Nethereum.Contracts.CQS;
using Nethereum.Contracts;
using System.Threading;
namespace Contracts.Contracts.Cards.ContractDefinition
{
    
    
    public partial class CardsDeployment:CardsDeploymentBase
    {
        public CardsDeployment():base(BYTECODE) { }
        
        public CardsDeployment(string byteCode):base(byteCode) { }
    }

    public class CardsDeploymentBase:ContractDeploymentMessage
    {
        
        public static string BYTECODE = "608060405234801561001057600080fd5b506001805463ffffffff7fffffffffffffffff0000000000000000000000000000000000000000ffffffff90911633640100000000021763ffffffff19161790556109fb806100606000396000f30060806040526004361061006c5763ffffffff7c0100000000000000000000000000000000000000000000000000000000600035041663351d7289811461007157806370d2aefe1461008e57806370d67cdc1461009c57806387dcff6d146100b7578063c6650b08146100c5575b600080fd5b34801561007d57600080fd5b5061008c60043560030b61012b565b005b61008c60043560030b610257565b3480156100a857600080fd5b5061008c60043560030b610425565b61008c60043560030b610428565b3480156100d157600080fd5b506040805160e081810190925261008c91600160a060020a036004803582169360243590921692604435926064359260843560030b9236926101849160a49060079083908390808284375093965061063695505050505050565b61013361089b565b600382810b810b600090815260208181526040808320815160c0810183528154600160a060020a039081168252600183015416938101939093526002810154838301528085015460608401526004810154850b850b90940b6080830152805160e081019182905291939260a0850192916005850191600791908390855b82829054906101000a900460030b60030b815260200190600401906020826003010492830192600103820291508084116101b057905050505050508152505090508160030b7fe44d8bdea58df6adff9825a4e9f52161b6624b2be54dddfc5c4b9386beba872582606001518360800151426000604051808581526020018460030b60030b815260200183815260200182815260200194505050505060405180910390a25050565b61025f61089b565b600382810b810b600090815260208181526040808320815160c0810183528154600160a060020a039081168252600183015416938101939093526002810154838301528085015460608401526004810154850b850b90940b6080830152805160e081019182905291939260a0850192916005850191600791908390855b82829054906101000a900460030b60030b815260200190600401906020826003010492830192600103820291508084116102dc57505050929093525050506040820151919250503414610379576040805160e560020a62461bcd02815260206004820152601960248201527f45323a206d73672e76616c7565213d636172642e707269636500000000000000604482015290519081900360640190fd5b3381526103858161084b565b90508160030b7f099a3659ae40de2cf659ec9a1df70a125a44a4000d653b992f175b8ddf93f1b38260000151836040015184606001518560800151346103ca88610877565b6103d389610877565b60408051600160a060020a039098168852602088019690965286860194909452600392830b90920b6060860152608085015260a084015260c0830152600060e083015251908190036101000190a25050565b50565b61043061089b565b600382810b810b600090815260208181526040808320815160c0810183528154600160a060020a039081168252600183015416938101939093526002810154838301528085015460608401526004810154850b850b90940b6080830152805160e081019182905292938493909160a08401919060058401906007908288855b82829054906101000a900460030b60030b815260200190600401906020826003010492830192600103820291508084116104af57905050505050508152505092508260400151915061050083610877565b8351909150600160a060020a03163314610564576040805160e560020a62461bcd02815260206004820152601a60248201527f45333a206d73672e73656e646572213d636172642e6f776e6572000000000000604482015290519081900360640190fd5b3481146105bb576040805160e560020a62461bcd02815260206004820152601e60248201527f45343a6d73672e76616c7565213d6d696e696e674c6576656c50726963650000604482015290519081900360640190fd5b6105c48361087d565b92508360030b7f80261eb512cbe3d0548b8a4086106ff07445b40f0e0e1f0096341aad2fe60a9a8460600151856080015185600080604051808681526020018560030b60030b81526020018481526020018381526020018281526020019550505050505060405180910390a250505050565b6001546000906401000000009004600160a060020a031633146106a3576040805160e560020a62461bcd02815260206004820152601560248201527f45313a206d73672e73656e646572213d6f776e65720000000000000000000000604482015290519081900360640190fd5b5060018054600381810b808401820b63ffffffff90811663ffffffff199485161785556040805160c081018252600160a060020a03808e1682528c811660208084019182528385018e8152606085018e81528d8a0b6080870190815260a087018e81528a8c0b8c0b600090815294859052979093208651815490871673ffffffffffffffffffffffffffffffffffffffff1991821617825594519c810180549d9096169c9094169b909b1790935591516002820155975188870155516004880180549190960b9093169290951691909117909255905190929061078c90600583019060076108ed565b509050508060030b7f64a64d4965e9b05c5ad99c540f101a3775c09d8ecbbe49cf7c647f6f0fd7a55b8888888888886040518087600160a060020a0316600160a060020a0316815260200186600160a060020a0316600160a060020a031681526020018581526020018481526020018360030b60030b815260200182600760200280838360005b8381101561082b578181015183820152602001610813565b50505050905001965050505050505060405180910390a250505050505050565b61085361089b565b608082018051600390810b900b905261086b82610877565b60409092019190915290565b50600090565b61088561089b565b60809091018051600101600390810b900b905290565b610180604051908101604052806000600160a060020a031681526020016000600160a060020a031681526020016000815260200160008152602001600060030b81526020016108e861098c565b905290565b60018301918390821561097c5791602002820160005b8382111561094a57835183826101000a81548163ffffffff021916908360030b63ffffffff1602179055509260200192600401602081600301049283019260010302610903565b801561097a5782816101000a81549063ffffffff021916905560040160208160030104928301926001030261094a565b505b506109889291506109ab565b5090565b60e0604051908101604052806007906020820280388339509192915050565b6109cc91905b8082111561098857805463ffffffff191681556001016109b1565b905600a165627a7a723058204353ba99550a610bdc75f1fc781e83487a36854836c9818705ea11d53a1362480029";
        
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
        [Parameter("int32", "id", 1, true )]
        public virtual int Id {get; set;}
        [Parameter("address", "owner", 2, false )]
        public virtual string Owner {get; set;}
        [Parameter("address", "firstOwner", 3, false )]
        public virtual string FirstOwner {get; set;}
        [Parameter("uint256", "price", 4, false )]
        public virtual BigInteger Price {get; set;}
        [Parameter("uint256", "miningLastCollectedDate", 5, false )]
        public virtual BigInteger MiningLastCollectedDate {get; set;}
        [Parameter("int32", "miningLevel", 6, false )]
        public virtual int MiningLevel {get; set;}
        [Parameter("int32[7]", "parentCards", 7, false )]
        public virtual List<int> ParentCards {get; set;}
    }    
    
    public partial class OnBoughtCardEventDTO:OnBoughtCardEventDTOBase{}

    [Event("OnBoughtCard")]
    public class OnBoughtCardEventDTOBase: IEventDTO
    {
        [Parameter("int32", "id", 1, true )]
        public virtual int Id {get; set;}
        [Parameter("address", "owner", 2, false )]
        public virtual string Owner {get; set;}
        [Parameter("uint256", "price", 3, false )]
        public virtual BigInteger Price {get; set;}
        [Parameter("uint256", "miningLastCollectedDate", 4, false )]
        public virtual BigInteger MiningLastCollectedDate {get; set;}
        [Parameter("int32", "miningLevel", 5, false )]
        public virtual int MiningLevel {get; set;}
        [Parameter("uint256", "buyingPrice", 6, false )]
        public virtual BigInteger BuyingPrice {get; set;}
        [Parameter("uint256", "nextAskingPrice", 7, false )]
        public virtual BigInteger NextAskingPrice {get; set;}
        [Parameter("uint256", "miningRatePerBlock", 8, false )]
        public virtual BigInteger MiningRatePerBlock {get; set;}
        [Parameter("uint256", "miningCollected", 9, false )]
        public virtual BigInteger MiningCollected {get; set;}
    }    
    
    public partial class OnBoughtMiningLevelEventDTO:OnBoughtMiningLevelEventDTOBase{}

    [Event("OnBoughtMiningLevel")]
    public class OnBoughtMiningLevelEventDTOBase: IEventDTO
    {
        [Parameter("int32", "id", 1, true )]
        public virtual int Id {get; set;}
        [Parameter("uint256", "miningLastCollectedDate", 2, false )]
        public virtual BigInteger MiningLastCollectedDate {get; set;}
        [Parameter("int32", "miningLevel", 3, false )]
        public virtual int MiningLevel {get; set;}
        [Parameter("uint256", "wccPrice", 4, false )]
        public virtual BigInteger WccPrice {get; set;}
        [Parameter("uint256", "miningCollected", 5, false )]
        public virtual BigInteger MiningCollected {get; set;}
        [Parameter("uint256", "spentOnUpgrade", 6, false )]
        public virtual BigInteger SpentOnUpgrade {get; set;}
    }    
    
    public partial class OnCardMiningCollectedEventDTO:OnCardMiningCollectedEventDTOBase{}

    [Event("OnCardMiningCollected")]
    public class OnCardMiningCollectedEventDTOBase: IEventDTO
    {
        [Parameter("int32", "id", 1, true )]
        public virtual int Id {get; set;}
        [Parameter("uint256", "miningLastCollectedDate", 2, false )]
        public virtual BigInteger MiningLastCollectedDate {get; set;}
        [Parameter("int32", "miningLevel", 3, false )]
        public virtual int MiningLevel {get; set;}
        [Parameter("uint256", "date", 4, false )]
        public virtual BigInteger Date {get; set;}
        [Parameter("uint256", "amount", 5, false )]
        public virtual BigInteger Amount {get; set;}
    }    
    
    
    
    
    
    
    
    
    

}
