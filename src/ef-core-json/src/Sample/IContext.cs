// See the LICENSE.TXT file in the project root for full license information.

using System;
using System.Threading;
using System.Threading.Tasks;
using Sample.Entities;

namespace Sample
{
    public interface IContext : IDisposable
    {
        IRepository<Car> Cars { get; }

        Task CommitAsync(CancellationToken cancellationToken);
    }
}
