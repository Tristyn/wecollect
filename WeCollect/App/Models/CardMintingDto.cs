using Contracts.Contracts;
using Contracts.Contracts.Cards.ContractDefinition;
using Microsoft.AspNetCore.Http;
using Nethereum.Hex.HexTypes;
using Nethereum.Util;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Numerics;
using WeCollect.Server.Models;

namespace WeCollect.App.Models
{
    public class CardMintingDto
    {
        public const int PARENTS_MAX_LEN = 7;

        private int[] _parents = new int[PARENTS_MAX_LEN];

        public int MiningLevel { get; set; }
        
        public decimal PriceWeiDecimal { get; set; }
        
        public BigInteger PriceWei { get; set; }

        public long LastMiningCollectedDate { get; set; }
        
        /// <summary>
        /// Only null if root
        /// </summary>
        public int? ParentCardsContractId { get; set; }
        
        // DocumentDb properties
        public string Name { get; set; }

        // Blob
        public IFormFile ImageFormUploadUri { get; set; }

        public Uri ImageUploadUri { get; set; }

        /// <summary>
        /// To cards representation in the contract. For minting cards mainly.
        /// Parent can be null if it's the root card
        /// </summary>
        public MintCardFunction ToMintCardFunction(CardDto parent)
        {
            if (parent != null)
            {
                Debug.Assert(parent.cardsContractId == ParentCardsContractId);
            }
            var parentCards = parent?.ToCardSpecsForChildCard()
                ?.Select(parentSpec => parentSpec.cardContractId)
                ?.ToList() ?? new List<int>(PARENTS_MAX_LEN);
            parentCards = parentCards.Concat(Enumerable.Repeat(-1, PARENTS_MAX_LEN - parentCards.Count)).ToList();
            return new MintCardFunction
            {
                Price = new HexBigInteger(PriceWei),
                MiningLastCollectedDate = new HexBigInteger(LastMiningCollectedDate),
                MiningLevel = MiningLevel,
                ParentCards = parentCards,
                FirstOwner = string.Empty,
                FromAddress = string.Empty,
                Owner = string.Empty
            };
        }
    }
}
