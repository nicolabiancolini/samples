// See the LICENSE.TXT file in the project root for full license information.

using Azure.Storage.Blobs;

namespace WebApp.Infrastructure.AzureBlobProvider
{
    public class BlobStorageOptions
    {
        public Uri ServiceUri { get; set; } = default!;

        public string ContainerName { get; set; } = default!;

        public string ContainerRoot { get; set; } = string.Empty;

        public string ConnectionString { get; set; } = default!;

        public bool AllowContainerCreation { get; set; } = true;

        public BlobClientOptions? BlobClient { get; set; }
    }
}
