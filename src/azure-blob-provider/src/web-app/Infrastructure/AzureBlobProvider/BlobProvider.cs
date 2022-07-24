// See the LICENSE.TXT file in the project root for full license information.

using Azure.Identity;
using Azure.Storage.Blobs;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;

namespace WebApp.Infrastructure.AzureBlobProvider
{
    public class BlobProvider : IFileProvider
    {
        private readonly bool containerCreationAllowed;
        private readonly string root;
        private readonly BlobContainerClient client;

        public BlobProvider(IOptions<BlobStorageOptions> options)
        {
            BlobServiceClient serviceClient = null!;
            if (options.Value.ConnectionString is not null)
            {
                serviceClient = new BlobServiceClient(options.Value.ConnectionString);
            }
            else
            {
                serviceClient = new BlobServiceClient(options.Value.ServiceUri, new DefaultAzureCredential());
            }

            this.root = options.Value.ContainerRoot;
            this.client = serviceClient.GetBlobContainerClient(options.Value.ContainerName);
            this.containerCreationAllowed = options.Value.AllowContainerCreation;
        }

        public IDirectoryContents GetDirectoryContents(string subpath)
        {
            if (!this.client.Exists())
            {
                if (this.containerCreationAllowed)
                {
                    this.client.Create();
                }
                else
                {
                    return new EmptyContainerContents();
                }
            }

            return new ContainerContents(this.client, this.client.GetBlobsByHierarchy(prefix: $"{this.root}{subpath}", delimiter: "/"));
        }

        public IFileInfo GetFileInfo(string subpath)
        {
            if (!this.client.Exists())
            {
                if (this.containerCreationAllowed)
                {
                    this.client.Create();
                }
                else
                {
                    throw new InvalidOperationException();
                }
            }

            var blob = this.client.GetBlobs(prefix: this.RootedPath(subpath)).FirstOrDefault();
            return blob is not null
                ? new BlobInfo(this.client, blob)
                : new NotFoundFileInfo(subpath);
        }

        public IChangeToken Watch(string filter)
        {
            throw new NotImplementedException();
        }

        private string RootedPath(string path)
        {
            return $"{this.root.Trim('/')}/{path.Trim('/')}";
        }
    }
}
