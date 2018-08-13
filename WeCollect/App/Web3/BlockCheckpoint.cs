using Microsoft.Azure.Documents;
using Nethereum.Hex.HexTypes;
using Nethereum.RPC.Eth.DTOs;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
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

        public BlockParameter Checkpoint => new BlockParameter(new HexBigInteger(_checkpointDto.BlockId));
        public Nethereum.Web3.Web3 _web3;


        private BlockCheckpoint(Nethereum.Web3.Web3 web3, CardDocumentDb documents, IBlockEnumerator blockEnumerator, BlockCheckpointDto checkpointDto)
        {
            _web3 = web3;
            _documentDb = documents;
            _blockEnumerator = blockEnumerator;
            _checkpointDto = checkpointDto;
        }

        public static async Task<BlockCheckpoint> Create(Nethereum.Web3.Web3 web3, CardDocumentDb documents, IBlockEnumerator blockEnumerator, string checkpointName)
        {
            var checkpoint = await documents.BlockCheckpoints.Get(BlockCheckpointDto.GetId(checkpointName));
            return new BlockCheckpoint(web3, documents, blockEnumerator, checkpoint);
        }

        public async Task MoveNext()
        {
            var next = JsonConvert.DeserializeObject<BlockCheckpointDto>(JsonConvert.SerializeObject(_checkpointDto));
            next.BlockId += 1;



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

        public async Task<BlockCheckpoint> GetOrCreateCheckpoint(string checkpointName, BlockParameter startBlock)
        {
            BlockCheckpointDto checkpointDto;

            try
            {
                checkpointDto = await _documents.BlockCheckpoints.Get(BlockCheckpointDto.GetId(checkpointName));
            }
            catch (DocumentClientException ex)
            {
                if (ex.IsNotFound())
                {
                    checkpointDto = await CreateCheckpoint(checkpointName, startBlock);
                }
                else
                {
                    throw;
                }
            }

            var checkpoint = await BlockCheckpoint.Create(_web3, _documents, new BlockEnumerator(_web3, checkpointDto.BlockId), checkpointName);

            return checkpoint;
        }

        private async Task<BlockCheckpointDto> CreateCheckpoint(string checkpointName, BlockParameter startBlock)
        {
            var checkpoint = new BlockCheckpointDto { };

            await _documents.BlockCheckpoints.Create(checkpoint);

            return await _documents.BlockCheckpoints.Get(checkpoint.Id);


        }
    }
}
