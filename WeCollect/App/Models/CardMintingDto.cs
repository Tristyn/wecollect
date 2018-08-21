using Contracts.Contracts;
using Nethereum.Hex.HexTypes;
using System;
using System.Numerics;
using WeCollect.Server.Models;

namespace WeCollect.App.Models
{
    public class CardMintingDto
    {
        public int MiningLevel { get; set; }

        public BigInteger PriceWei { get; set; }

        public DateTimeOffset LastMiningCollectedDate { get; set; }

        /// <summary>
        /// Max Length 7
        /// </summary>
        public int[] Parents { get; set; }

        // DocumentDb properties
        public string Name { get; set; }
        

        /// <summary>
        /// To cards representation in the contract. For minting cards mainly
        /// </summary>
        public ContractCardDto ToContractCardDto()
        {
            return new ContractCardDto
            {
                Price = new HexBigInteger(PriceWei),
                MiningLastCollectedDate = new HexBigInteger(LastMiningCollectedDate.ToUnixTimeSeconds()),
                MiningLevel = MiningLevel,
                ParentCards = Parents
            };
        }
    }
}
