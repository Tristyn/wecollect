using System;
using System.Numerics;

namespace Nethereum.RPC.Eth.DTOs
{
    public static class BlockParameterExtensions
    {
        public static BigInteger ToInt(this BlockParameter block)
        {
            switch (block.ParameterType)
            {
                case BlockParameter.BlockParameterType.latest:
                    if (block.BlockNumber == default)
                        throw new InvalidOperationException();
                    return block.BlockNumber;
                case BlockParameter.BlockParameterType.earliest:
                    return 0;
                case BlockParameter.BlockParameterType.pending:
                    if (block.BlockNumber == default)
                        throw new InvalidOperationException();
                    return block.BlockNumber;
                case BlockParameter.BlockParameterType.blockNumber:
                    if (block.BlockNumber == default)
                        throw new InvalidOperationException();
                    return block.BlockNumber;
                default:
                    throw new IndexOutOfRangeException($"{nameof(block)}.{nameof(block.ParameterType)}");
            }
        }
    }
}