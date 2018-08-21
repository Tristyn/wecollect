using Contracts.Contracts;
using Contracts.Contracts.Cards.ContractDefinition;
using Nethereum.Hex.HexTypes;
using System;
using System.Linq;
using System.Numerics;
using WeCollect.Server.Models;

namespace WeCollect.App.Models
{
    public class CardMintingDto
    {
        private int[] _parents = new int[7];

        public int MiningLevel { get; set; }

        public BigInteger PriceWei { get; set; }

        public DateTimeOffset LastMiningCollectedDate { get; set; }

        /// <summary>
        /// Max Length 7
        /// </summary>
        public int[] Parents
        {
            get => _parents;
            set
            {
                if (value.Length > 7) throw new IndexOutOfRangeException();
                if (value.Length < 7)
                {
                    var sized = new int[7];
                    value.CopyTo(sized, 0);
                    value = sized;
                }
                _parents = value;
            }
        }
        // DocumentDb properties
        public string Name { get; set; }


        /// <summary>
        /// To cards representation in the contract. For minting cards mainly
        /// </summary>
        public MintCardFunction ToMintCardFunction()
        {
            return new MintCardFunction
            {
                Price = new HexBigInteger(PriceWei),
                MiningLastCollectedDate = new HexBigInteger(LastMiningCollectedDate.ToUnixTimeSeconds()),
                MiningLevel = MiningLevel,
                ParentCards = Parents.ToList(),
                FirstOwner = string.Empty,
                FromAddress = string.Empty,
                Owner = string.Empty
            };
        }
    }
}
