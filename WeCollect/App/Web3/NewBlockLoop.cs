using System;
using System.Numerics;
using System.Threading;
using System.Threading.Tasks;

namespace WeCollect.App.Web3
{
    public class NewBlockLoop
    {
        private readonly Nethereum.Web3.Web3 _web3;
        private readonly IBlockCheckpoint _blockCheckpoint;
        
        public NewBlockLoop(Nethereum.Web3.Web3 web3, IBlockCheckpoint blockCheckpoint)
        {
            _web3 = web3;
            _blockCheckpoint = blockCheckpoint;
        }

        public async Task Loop(Func<BigInteger, Task> callback)
        {            
            while (true)
            {
                try
                {
                    while (!await _blockCheckpoint.BlockExistsOnChain())
                    {
                        await Task.Delay(TimeSpan.FromSeconds(5));
                    }

                    await callback(_blockCheckpoint.Checkpoint.BlockNumber.Value);

                    await _blockCheckpoint.MoveNext();
                }
                catch (Exception ex)
                {
                    Log.LogError(ex);
                }
            }
        }

        public CancellationToken BlockCreatedToken => _blockCreatedCts.Token;
        private CancellationTokenSource _blockCreatedCts = new CancellationTokenSource();

        private void ResetBlockCreatedToken()
        {
            _blockCreatedCts = new CancellationTokenSource();
        }

        private void SetBlockCreatedToken()
        {
            _blockCreatedCts.Cancel();
        }
    }
}
