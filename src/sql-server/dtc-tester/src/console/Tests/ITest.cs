using System;
using System.Threading;
using System.Threading.Tasks;

namespace SqlServer.DistributedTransactionCoordinator.Tester.Tests
{
    public interface ITest : IDisposable
    {
        string Name { get; }

        Task ExecuteAsync(CancellationToken cancellationToken);
    }
}
