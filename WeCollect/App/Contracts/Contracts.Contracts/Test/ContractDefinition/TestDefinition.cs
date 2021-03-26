using System.Numerics;
using Nethereum.ABI.FunctionEncoding.Attributes;
using Nethereum.Contracts;
namespace Contracts.Contracts.Test.ContractDefinition
{


    public partial class TestDeployment:TestDeploymentBase
    {
        public TestDeployment():base(BYTECODE) { }
        
        public TestDeployment(string byteCode):base(byteCode) { }
    }

    public class TestDeploymentBase:ContractDeploymentMessage
    {
        
        public static string BYTECODE = "6080604052348015600f57600080fd5b50604051602080610101833981016040525160005560cf806100326000396000f300608060405260043610603e5763ffffffff7c0100000000000000000000000000000000000000000000000000000000600035041663942c253581146043575b600080fd5b604c600435605e565b60408051918252519081900360200190f35b6000546040805191830280835290519091339184917fae689c88bddb7f9f71bb50fb9d610a340aa936167d99805624291ad4fa9195ba919081900360200190a39190505600a165627a7a723058200f872613f1a24d461d1235ac36f4fa585a16caaadc695480435fbff34d7679820029";
        
        public TestDeploymentBase():base(BYTECODE) { }
        
        public TestDeploymentBase(string byteCode):base(byteCode) { }
        
        [Parameter("int256", "multipier", 1)]
        public virtual BigInteger Multipier {get; set;}
    }    
    
    public partial class MulitplyFunction:MulitplyFunctionBase{}

    [Function("mulitply", "int256")]
    public class MulitplyFunctionBase:FunctionMessage
    {
        [Parameter("int256", "val", 1)]
        public virtual BigInteger Val {get; set;}
    }    
    
    public partial class OnMultiplyEventDTO:OnMultiplyEventDTOBase{}

    [Event("onMultiply")]
    public class OnMultiplyEventDTOBase: IEventDTO
    {
        [Parameter("int256", "a", 1, true )]
        public virtual BigInteger A {get; set;}
        [Parameter("address", "sender", 2, true )]
        public virtual string Sender {get; set;}
        [Parameter("int256", "result", 3, false )]
        public virtual BigInteger Result {get; set;}
    }    
    

}
