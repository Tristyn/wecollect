using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Azure.Documents;
using Newtonsoft.Json;

namespace WeCollect.App.Models
{
    public abstract class Document : Microsoft.Azure.Documents.Document
    {
        [JsonProperty(PropertyName = "id")]
        public override string Id
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
        protected abstract string Type { get; }
    }
}
