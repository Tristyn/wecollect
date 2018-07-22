using Microsoft.Azure.Documents;
using Newtonsoft.Json;
using System;

namespace WeCollect.App.Models
{
    public class CardDto : Document
    {
        public override string Name { get; set; }

        protected override string Type => nameof(CardDto);

        [JsonProperty(PropertyName = "ownerAddr")]
        public string OwnerAddr { get; set; }

        [JsonProperty(PropertyName = "ownerName")]
        public string OwnerName { get; set; }

        [JsonProperty(PropertyName = "firstOwnerAddr")]

        public string FirstOwnerAddr { get; set; }

        [JsonProperty(PropertyName = "firstOwnerName")]
        public string FirstownerName { get; set; }

        [JsonProperty(PropertyName = "currentPrice")]
        public int CurrentPrice { get; set; }

        [JsonProperty(PropertyName = "nextPrice")]
        public int NextPrice { get; set; }

        [JsonProperty(PropertyName = "miningLevel")]
        public int MiningLevel { get; set; } = 1;

        [JsonProperty(PropertyName = "LastCollectedDate")]
        public DateTimeOffset LastCollectedDate { get; set; }

        [JsonProperty(PropertyName = "parents")]
        public CardSpecDto[] Parents { get; set; } = Array.Empty<CardSpecDto>();

        public CardSpecDto ToCardSpec()
        {
            return new CardSpecDto
            {
                Id = Id,
                Name = Id
            };
        }
    }
}
