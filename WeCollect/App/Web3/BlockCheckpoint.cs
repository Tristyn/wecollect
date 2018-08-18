using Microsoft.Azure.Documents;
using Nethereum.Hex.HexTypes;
using Nethereum.RPC.Eth.DTOs;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;
using WeCollect.App.Documents;
using WeCollect.App.Models;

namespace WeCollect.App.Web3
{
    public class BlockCheckpoint : IBlockCheckpoint
    {
        private readonly CardDocumentDb _documentDb;
        private BlockCheckpointDto _checkpointDto;
        private BlockCheckpointDto _latestDocumentCheckpointDto;

        IBlockEnumerator _blockEnumerator;

        public BlockParameter Checkpoint => new BlockParameter(new HexBigInteger(_checkpointDto.BlockPosition));
        public Nethereum.Web3.Web3 _web3;


        private BlockCheckpoint(Nethereum.Web3.Web3 web3, CardDocumentDb documents, IBlockEnumerator blockEnumerator, BlockCheckpointDto checkpointDto)
        {
            _web3 = web3;
            _documentDb = documents;
            _blockEnumerator = blockEnumerator;
            _checkpointDto = checkpointDto;
        }

        public static async Task<BlockCheckpoint> Create(Nethereum.Web3.Web3 web3, CardDocumentDb documents, BlockCheckpointDto checkpoint, IBlockEnumerator blockEnumerator, string checkpointName)
        {
            return new BlockCheckpoint(web3, documents, blockEnumerator, checkpoint);
        }

        public async Task MoveNext()
        {
            var next = JsonConvert.DeserializeObject<BlockCheckpointDto>(JsonConvert.SerializeObject(_checkpointDto));
            next.BlockPosition += 1;



            try
            {

                var engine = await BlockCheckpointEngine.GetAsync(_documentDb, _checkpointDto.Id);
                await engine.IncrementBlockId();
                _checkpointDto = engine.BlockCheckpoint;

                var block = await _blockEnumerator.NextBlockAsync();

                await _documentDb.BlockCheckpoints.Set(_checkpointDto);
            }
            catch (DocumentClientException ex)
            {
                if (ex.IsPreconditionFailed())
                {
                    _latestDocumentCheckpointDto = await _documentDb.BlockCheckpoints.Get(_checkpointDto.Id);
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
        private readonly CardDocumentDb _documents;

        public BlockCheckpointFactory(Nethereum.Web3.Web3 web3, CardDocumentDb documents)
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

            var checkpoint = await BlockCheckpoint.Create(_web3, _documents, checkpointDoc, new BlockEnumerator(_web3, checkpointDoc.BlockPosition), checkpointName);
            return checkpoint;
        }

        private async Task<BlockCheckpointDto> CreateCheckpoint(string checkpointName, BigInteger startBlock)
        {
            var checkpoint = new BlockCheckpointDto
            {
                Name = checkpointName,
                BlockPosition = startBlock
            };

            await _documents.BlockCheckpoints.Create(checkpoint);

            return await _documents.BlockCheckpoints.Get(checkpoint.Id);


        }
    }
}
