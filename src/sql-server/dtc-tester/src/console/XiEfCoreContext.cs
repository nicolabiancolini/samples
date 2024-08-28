using Microsoft.EntityFrameworkCore;

namespace SqlServer.DistributedTransactionCoordinator.Tester
{
    internal class XiEfCoreContext : SqlServerEfCoreContext
    {
        public XiEfCoreContext(DbContextOptions options)
            : base(options)
        {
        }
    }
}
