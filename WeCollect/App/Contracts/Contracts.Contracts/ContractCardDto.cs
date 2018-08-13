using Nethereum.Hex.HexTypes;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Text;

namespace Contracts.Contracts
{
    /// <summary>
    /// The card struct in the contract
    /// </summary>
    [JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy))]
    public class ContractCardDto
    {
        public string Owner { get; set; }

        public string FirstOwner { get; set; }

        public HexBigInteger Price { get; set; }

        public HexBigInteger MiningLastCollectedDate { get; set; }

        public int MiningLevel { get; set; }

        /// <summary>
        /// Max Length 7
        /// </summary>
        public int[] ParentCards { get; set; }
    }
}
