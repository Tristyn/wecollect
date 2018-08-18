﻿using Nethereum.Hex.HexTypes;
using Nethereum.RPC.Eth.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;

namespace WeCollect.App.Models
{

    public class BlockCheckpointDto : Document
    {
        private BigInteger _blockPosition;

        public override string Name { get; set; }

        public override string Type => nameof(BlockCheckpointDto);

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