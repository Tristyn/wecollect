using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WeCollect
{
    public class ServerConfiguration
    {
        public Uri Web3Url { get; set; }

        public string Web3ServerAddress { get; set; }

        public string Web3ServerPrivateKey { get; set; }

        public Uri DocumentDbEndpoint { get; set; }

        public string DocumentDbKey { get; set; }

        public Uri StorageAccountBlobsUri { get; set; }

        public string StorageAccountName { get; set; }

        public string StorageAccountKey { get; set; }
    }
}
