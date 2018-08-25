using Microsoft.AspNetCore.Http;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace WeCollect.App.Blob
{
    public class BlobService
    {
        private readonly CloudBlobContainer _container;

        public const string CardImage = "cardimage";

        private BlobService(CloudBlobContainer container)
        {
            _container = container;
        }

        public static async Task<BlobService> Create(CloudBlobContainer container)
        {
            await ConfigureContainer(container);
            return new BlobService(container);
        }

        public async Task Save(Stream stream, string name)
        {
            var blob = _container.GetBlockBlobReference(name);
            await blob.UploadFromStreamAsync(stream);
        }

        public Uri GetUrl(string name)
        {
            return new Uri(_container.Uri, name);
        }

        private static async Task ConfigureContainer(CloudBlobContainer container)
        {
            await container.CreateIfNotExistsAsync();

            // If in release, use the azure CDN instead
#if DEBUG
            var permissions = await container.GetPermissionsAsync();
            permissions.PublicAccess = BlobContainerPublicAccessType.Container;
            await container.SetPermissionsAsync(permissions);
#endif
        }
    }


}
