using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Transactions;
using Microsoft.EntityFrameworkCore;

namespace SqlServer.DistributedTransactionCoordinator.Tester.Tests
{
    internal class EntityFrameworkCoreTest : ITest
    {
        private readonly PiEfCoreContext piContext;
        private readonly XiEfCoreContext xiContext;
        private readonly TransactionScope transaction;
        private readonly string currentRuntimeFramework;
        private bool isDisposed;

        public EntityFrameworkCoreTest(IConfiguration configuration, ILoggerFactory loggerFactory)
        {
#if NET
            this.currentRuntimeFramework = "NET";
#endif

#if NETFRAMEWORK
            this.currentRuntimeFramework = "NETFRAMEWORK";
#endif
            this.piContext = new PiEfCoreContext(new DbContextOptionsBuilder()
                .UseLoggerFactory(loggerFactory)
                .UseSqlServer(configuration.GetConnectionString("PiContext"))
                .UseQueryTrackingBehavior(QueryTrackingBehavior.TrackAll)
                .Options);

            this.xiContext = new XiEfCoreContext(new DbContextOptionsBuilder()
                .UseLoggerFactory(loggerFactory)
                .UseSqlServer(configuration.GetConnectionString("XiContext"))
                .UseQueryTrackingBehavior(QueryTrackingBehavior.TrackAll)
                .Options);

            this.piContext.Database.EnsureCreated();
            this.xiContext.Database.EnsureCreated();
            this.transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
        }

        public string Name => "Multiple EF Core context";

        public async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            var messageTemplate = "[{1}][{2}] Hello, {0}!";

            using (var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                piContext.Logs.Add(new Log(string.Format(messageTemplate, nameof(PiEf6Context), nameof(EntityFrameworkCoreTest), this.currentRuntimeFramework)));
                await piContext.SaveChangesAsync();

                xiContext.Logs.Add(new Log(string.Format(messageTemplate, nameof(XiEf6Context), nameof(EntityFrameworkCoreTest), this.currentRuntimeFramework)));
                await xiContext.SaveChangesAsync();

                transaction.Complete();
            }
        }

        public void Dispose()
        {
            this.Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!isDisposed)
            {
                if (disposing)
                {
                    this.piContext.Dispose();
                    this.xiContext.Dispose();
                    this.transaction.Dispose();
                }

                isDisposed = true;
            }
        }
    }
}
