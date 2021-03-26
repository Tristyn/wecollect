using System.Numerics;

namespace WeCollect.App.Models
{

    public class BlockCheckpointDto : Document
    {
        private BigInteger _blockPosition;

        public override string name { get; set; }

        public override string type => nameof(BlockCheckpointDto);

        public BigInteger BlockPosition { get => _blockPosition;
            set => _blockPosition = value; }

        public static string GetId(string name)
        {
            return nameof(BlockCheckpointDto) + name;
        }

        public BlockCheckpointDto Clone()
        {
            return (BlockCheckpointDto)MemberwiseClone();
        }
    }
}
