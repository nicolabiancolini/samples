// See the LICENSE.TXT file in the project root for full license information.

using System.Collections;
using Microsoft.Extensions.FileProviders;

namespace WebApp.Infrastructure.AzureBlobProvider
{
    public class EmptyContainerContents : IDirectoryContents
    {
        internal EmptyContainerContents()
        {
        }

        public bool Exists
        {
            get { return false; }
        }

        public IEnumerator<IFileInfo> GetEnumerator()
        {
            return Enumerable.Empty<BlobInfo>().GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}
