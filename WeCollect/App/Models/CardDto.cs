using Newtonsoft.Json;
using System;

namespace WeCollect.App.Models
{
    public class CardDto : Document
    {
        public override string Name { get; set; }

        [JsonProperty(PropertyName = "set")]
        public string Set { get; set; }

        [JsonProperty(PropertyName = "ownerAddr")]
        public string OwnerAddr { get; set; }

        [JsonProperty(PropertyName = "ownerName")]
        public string OwnerName { get; set; }

        [JsonProperty(PropertyName = "firstOwnerAddr")]

        public string FirstOwnerAddr { get; set; }

        [JsonProperty(PropertyName = "firstOwnerName")]
        public string FirstOwnerName { get; set; }

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

        public static string GetId(string name)
        {
            return nameof(CardDto) + name;
        }
    }
}
