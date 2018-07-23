using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Azure.Documents;
using Newtonsoft.Json;

namespace WeCollect.App.Models
{
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

        [JsonIgnore()]
        private string Type => GetType().Name;
        
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
