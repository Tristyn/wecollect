﻿using Nethereum.Hex.HexTypes;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;

namespace WeCollect.App.Models
{
    [JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy))]
    public class BoughtMiningLevelDto
    {
        public Int32 Id { get; set; }

        //public CardDto Card { get; set; }

        public HexBigInteger WccPrice { get; set; }

        public HexBigInteger MiningCollected { get; set; }

        public HexBigInteger SpentOnUpgrade { get; set; }
    }
}
