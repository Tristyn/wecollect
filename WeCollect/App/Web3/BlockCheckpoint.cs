using Microsoft.Azure.Documents;
using Nethereum.Hex.HexTypes;
using Nethereum.RPC.Eth.DTOs;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Numerics;
using System.Threading.Tasks;
using WeCollect.App.Documents;
using WeCollect.App.Models;

namespace WeCollect.App.Web3
{
    public class BlockCheckpoint : IBlockCheckpoint
    {
        private readonly CardDb _documentDb;
        private BlockCheckpointDto _checkpointDto;
        private BlockCheckpointDto _latestDocumentCheckpointDto;

        IBlockEnumerator _blockEnumerator;

        public BlockParameter Checkpoint => new BlockParameter(new HexBigInteger(_checkpointDto.BlockPosition));
        public Nethereum.Web3.Web3 _web3;
        
        public BlockCheckpoint(Nethereum.Web3.Web3 web3, CardDb documents, IBlockEnumerator blockEnumerator, BlockCheckpointDto checkpointDto)
        {
            _web3 = web3;
            _documentDb = documents;
            _blockEnumerator = blockEnumerator;
            _checkpointDto = checkpointDto;
        }

        public async Task MoveNext()
        {
            var next = JsonConvert.DeserializeObject<BlockCheckpointDto>(JsonConvert.SerializeObject(_checkpointDto));
            next.BlockPosition += 1;



            try
            {

                var engine = await BlockCheckpointEngine.GetAsync(_documentDb, _checkpointDto.id);
                await engine.IncrementBlockId();
                _checkpointDto = engine.BlockCheckpoint;

                var block = await _blockEnumerator.NextBlockAsync();

                await _documentDb.BlockCheckpoints.Replace(_checkpointDto);
            }
            catch (DocumentClientException ex)
            {
                if (ex.IsPreconditionFailed())
                {
                    _latestDocumentCheckpointDto = await _documentDb.BlockCheckpoints.Get(_checkpointDto.id);
                }
                else
                {
                    throw;
                }
            }
        }

        public Task<bool> BlockExistsOnChain()
        {
            return _blockEnumerator.BlockExistsOnChain();
        }
    }

    public interface IBlockCheckpoint
    {
        Task MoveNext();

        BlockParameter Checkpoint { get; }

        Task<bool> BlockExistsOnChain();
    }

    public class BlockCheckpointFactory
    {
        private readonly Nethereum.Web3.Web3 _web3;
        private readonly CardDb _documents;

        public BlockCheckpointFactory(Nethereum.Web3.Web3 web3, CardDb documents)
        {
            _web3 = web3;
            _documents = documents;
        }

        [DebuggerHidden]
        public async Task<BlockCheckpoint> GetOrCreateCheckpoint(string checkpointName, BigInteger startBlock)
        {
            BlockCheckpointDto checkpointDoc;

            if (await _documents.BlockCheckpoints.Exists(BlockCheckpointDto.GetId(checkpointName)))
            {
                checkpointDoc = await _documents.BlockCheckpoints.Get(BlockCheckpointDto.GetId(checkpointName));
            }
            else
            {
                try
                {
                    checkpointDoc = await CreateCheckpoint(checkpointName, startBlock);
                }
                catch (DocumentClientException ex)
                {
                    if (!ex.IsNotFound())
                    {
                        throw;
                    }
                    else
                    {
                        checkpointDoc = await _documents.BlockCheckpoints.Get(BlockCheckpointDto.GetId(checkpointName));
                    }
                }
            }

            var blockId = checkpointDoc.BlockPosition;

            var checkpoint = new BlockCheckpoint(_web3, _documents, new BlockEnumerator(_web3, checkpointDoc.BlockPosition), checkpointDoc);
            return checkpoint;
        }

        private async Task<BlockCheckpointDto> CreateCheckpoint(string checkpointName, BigInteger startBlock)
        {
            var checkpoint = new BlockCheckpointDto
            {
                name = checkpointName,
                BlockPosition = startBlock
            };

            await _documents.BlockCheckpoints.Create(checkpoint);

            return await _documents.BlockCheckpoints.Get(checkpoint.id);


        }
    }
}
