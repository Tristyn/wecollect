using Microsoft.Azure.Documents;
using System.Threading.Tasks;
using WeCollect.App.Models;

namespace WeCollect.App.Documents
{
    public class BlockCheckpointEngine
    {
        private readonly CardDb _documentDb;

        public BlockCheckpointDto BlockCheckpoint { get; private set; }

        public BlockCheckpointEngine(CardDb cardDocumentDb, BlockCheckpointDto checkpoint)
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
                await _documentDb.BlockCheckpoints.Replace(checkpoint);
                var gottenCheckpoint = await _documentDb.BlockCheckpoints.Get(checkpoint.id);

                BlockCheckpoint = gottenCheckpoint;
            }
            catch (DocumentClientException ex)
            {
                if (!ex.IsPreconditionFailed())
                    throw;
                
                var gottenCheckpoint = await _documentDb.BlockCheckpoints.Get(checkpoint.id);
                if (gottenCheckpoint.BlockPosition < checkpoint.BlockPosition)
                {
                    throw;
                }

                BlockCheckpoint = checkpoint;
            }
        }

        public static async Task<BlockCheckpointEngine> GetAsync(CardDb documentDb, string id)
        {
            var checkpoint = await documentDb.BlockCheckpoints.Get(id);
            return new BlockCheckpointEngine(documentDb, checkpoint);
        }
    }
}
