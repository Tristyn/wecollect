using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;

namespace WeCollect.App.Models
{
    [JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy))]
    public abstract class Document
    {
        [JsonProperty(PropertyName = "id")]
        public virtual string Id
        {
            get
            {
                return Type + Name;
            }
            set
            {
            }
        }

        [JsonProperty(PropertyName = "name")]
        public abstract string Name { get; set; }

        public abstract string Type { get; }
        
        [JsonProperty(PropertyName = "ttl", NullValueHandling = NullValueHandling.Ignore)]
        public int? TimeToLive { get; set; }
        
        [JsonProperty(PropertyName = "_rid")]
        public virtual string ResourceId { get; set; }

        [JsonProperty(PropertyName = "_self")]
        public string SelfLink { get; }

        [JsonIgnore]
        public string AltLink { get; set; }

        [JsonConverter(typeof(UnixDateTimeConverter))]
        [JsonProperty(PropertyName = "_ts")]
        public virtual DateTime Timestamp { get; internal set; }

        [JsonProperty(PropertyName = "_etag")]
        public string ETag { get; }
    }
}
