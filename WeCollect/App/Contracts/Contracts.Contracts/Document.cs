using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace WeCollect.App.Models
{
    [JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy))]
    public abstract class Document
    {
        [JsonProperty(PropertyName = "id")]
        public virtual string id
        {
            get
            {
                return type + name;
            }
            set
            {
            }
        }

        [JsonProperty(PropertyName = "name")]
        public abstract string name { get; set; }

        public abstract string type { get; }
        
        [JsonProperty(PropertyName = "ttl", NullValueHandling = NullValueHandling.Ignore)]
        public int? TimeToLive { get; set; }
        
        [JsonProperty(PropertyName = "_rid")]
        public virtual string ResourceId { get; set; }

        [JsonProperty(PropertyName = "_self")]
        public string SelfLink { get; }

        [JsonIgnore]
        public string AltLink { get; set; }
        
        [JsonProperty(PropertyName = "_ts")]
        private long _timestamp { get; } = DateTimeOffset.UtcNow.ToUnixTimeSeconds();

        public DateTimeOffset Timestamp => DateTimeOffset.FromUnixTimeSeconds(_timestamp);

        [JsonProperty(PropertyName = "_etag")]
        public string ETag { get; }
    }
}
