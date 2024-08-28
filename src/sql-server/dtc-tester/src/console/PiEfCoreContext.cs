using Microsoft.EntityFrameworkCore;

namespace SqlServer.DistributedTransactionCoordinator.Tester
{
    internal class PiEfCoreContext : SqlServerEfCoreContext
    {
        public PiEfCoreContext(DbContextOptions options)
            : base(options)
        {
        }
    }
}
