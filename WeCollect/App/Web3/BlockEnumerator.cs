using Microsoft.Extensions.Logging;
using Nethereum.Hex.HexTypes;
using Nethereum.RPC.Eth.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;

namespace WeCollect.App.Web3
{
    public class BlockEnumerator : IBlockEnumerator
    {
        private static readonly ILogger _loopLogger = Logger.GetLogger($"{nameof(BlockEnumerator)}.{nameof(NextBlockAsync)}.Loop");
        private readonly Nethereum.Web3.Web3 _web3;
        private BigInteger _last;

        public BlockEnumerator(Nethereum.Web3.Web3 web3, BigInteger last)
        {
            _web3 = web3;
            _last = last;
        }

        public async Task<BlockParameter> NextBlockAsync()
        {
            while (true)
            {
                try
                {
                    var current = await _web3.Eth.Blocks.GetBlockNumber.SendRequestAsync();
                    var blockParam = new BlockParameter(current);

                    if (_last + 1 > current.Value)
                    {
                        await Task.Delay(TimeSpan.FromSeconds(15));
                        continue;
                    }
                    else
                    {
                        _last++;
                        return new BlockParameter(new HexBigInteger(_last));
                    }
                }
                catch (Exception ex)
                {
                    _loopLogger.LogError(ex);
                }
            }

        }

        public async Task<bool> BlockExistsOnChain()
        {
            var current = await _web3.Eth.Blocks.GetBlockNumber.SendRequestAsync();

            return current >= _last;
        }
    }

    public interface IBlockEnumerator
    {
        Task<BlockParameter> NextBlockAsync();

        Task<bool> BlockExistsOnChain();
    }
}
