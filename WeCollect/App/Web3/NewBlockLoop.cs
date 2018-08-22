using Nito.AsyncEx;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Threading;
using System.Threading.Tasks;

namespace WeCollect.App.Web3
{
    public class NewBlockLoop
    {
        private readonly Nethereum.Web3.Web3 _web3;
        private readonly IBlockCheckpoint _blockCheckpoint;

        private readonly AsyncAutoResetEvent _runEvent = new AsyncAutoResetEvent();

        public NewBlockLoop(Nethereum.Web3.Web3 web3, IBlockCheckpoint blockCheckpoint)
        {
            _web3 = web3;
            _blockCheckpoint = blockCheckpoint;
        }

        public async Task Loop(Func<BigInteger, Task> callback, Func<Exception, Task> error)
        {
            await _runEvent.WaitAsync();

            while (true)
            {
                try
                {
                    while (!await _blockCheckpoint.BlockExistsOnChain())
                    {
                        await Task.Delay(TimeSpan.FromSeconds(1));
                    }

                    await callback(_blockCheckpoint.Checkpoint.BlockNumber.Value);

                    await _blockCheckpoint.MoveNext();
                }
                catch (Exception ex)
                {
                    await error(ex);
                }
            }
        }

        public void Start()
        {
            _runEvent.Set();
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
