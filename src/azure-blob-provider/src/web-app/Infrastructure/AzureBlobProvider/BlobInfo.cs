// See the LICENSE.TXT file in the project root for full license information.

using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.Extensions.FileProviders;

namespace WebApp.Infrastructure.AzureBlobProvider
{
    public class BlobInfo : IFileInfo
    {
        private readonly BlobClient client;

        internal BlobInfo(BlobContainerClient client, BlobHierarchyItem item)
        {
            this.Exists = true;
            this.IsDirectory = !item.IsBlob;
            this.Name = this.IsDirectory
                ? item.Prefix.TrimEnd('/').Split("/").LastOrDefault(string.Empty)
                : item.Blob.Name.Split("/").Last();
            this.LastModified = item.Blob?.Properties.LastModified.GetValueOrDefault() ?? default;
            this.Length = item.Blob?.Properties.ContentLength.GetValueOrDefault() ?? default;
            this.client = client.GetBlobClient(this.IsDirectory ? null! : item.Blob!.Name);
        }

        internal BlobInfo(BlobContainerClient client, BlobItem item)
        {
            this.IsDirectory = false;
            this.Exists = true;
            this.Name = item.Name.Split("/").Last();
            this.LastModified = item.Properties.LastModified.GetValueOrDefault();
            this.Length = item.Properties.ContentLength.GetValueOrDefault();
            this.client = client.GetBlobClient(item.Name);
        }

        public bool Exists { get; }

        public bool IsDirectory { get; }

        public DateTimeOffset LastModified { get; }

        public long Length { get; }

        public string Name { get; }

        public string PhysicalPath { get; } = null!;

        public Stream CreateReadStream()
        {
            if (!this.IsDirectory && this.Exists)
            {
                return this.client.OpenRead();
            }

            return Stream.Null;
        }
    }
}
