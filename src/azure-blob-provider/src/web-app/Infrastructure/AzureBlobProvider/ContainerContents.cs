// See the LICENSE.TXT file in the project root for full license information.

using System.Collections;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.Extensions.FileProviders;

namespace WebApp.Infrastructure.AzureBlobProvider
{
    public class ContainerContents : IDirectoryContents
    {
        private readonly BlobContainerClient client;
        private readonly IEnumerable<BlobHierarchyItem> items;

        internal ContainerContents(BlobContainerClient client, IEnumerable<BlobHierarchyItem> items)
        {
            this.client = client;
            this.items = items;
        }

        public bool Exists
        {
            get { return this.items.Any(); }
        }

        public IEnumerator<IFileInfo> GetEnumerator()
        {
            return this.items.Select(i => new BlobInfo(this.client, i)).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}
