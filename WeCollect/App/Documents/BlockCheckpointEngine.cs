using Microsoft.Azure.Documents;
using Nethereum.RPC.Eth.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WeCollect.App.Models;

namespace WeCollect.App.Documents
{
    public class BlockCheckpointEngine
    {
        private readonly CardDocumentDb _documentDb;

        public BlockCheckpointDto BlockCheckpoint { get; private set; }

        public BlockCheckpointEngine(CardDocumentDb cardDocumentDb, BlockCheckpointDto checkpoint)
        {
            _documentDb = cardDocumentDb;
            BlockCheckpoint = checkpoint;
        }

        public async Task IncrementBlockId()
        {
            var checkpoint = BlockCheckpoint.Clone();
            checkpoint.BlockPosition += 1;

            try
            {
                await _documentDb.BlockCheckpoints.Set(checkpoint);
                var gottenCheckpoint = await _documentDb.BlockCheckpoints.Get(checkpoint.Id);

                BlockCheckpoint = gottenCheckpoint;
            }
            catch (DocumentClientException ex)
            {
                if (!ex.IsPreconditionFailed())
                    throw;
                
                var gottenCheckpoint = await _documentDb.BlockCheckpoints.Get(checkpoint.Id);
                if (gottenCheckpoint.BlockPosition < checkpoint.BlockPosition)
                {
                    throw;
                }

                BlockCheckpoint = checkpoint;
            }
        }

        public static async Task<BlockCheckpointEngine> GetAsync(CardDocumentDb documentDb, string id)
        {
            var checkpoint = await documentDb.BlockCheckpoints.Get(id);
            return new BlockCheckpointEngine(documentDb, checkpoint);
        }
    }
}
