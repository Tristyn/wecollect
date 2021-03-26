using System;

namespace WeCollect
{
    public class ServerConfiguration
    {
        public Uri Web3Url { get; set; }

        public string Web3HotAddress { get; set; }

        public string Web3HotPrivateKey { get; set; }

        public Uri DocumentDbEndpoint { get; set; }

        public string DocumentDbKey { get; set; }

        public Uri StorageAccountBlobsUri { get; set; }

        public string StorageAccountName { get; set; }

        public string StorageAccountKey { get; set; }
    }
}
