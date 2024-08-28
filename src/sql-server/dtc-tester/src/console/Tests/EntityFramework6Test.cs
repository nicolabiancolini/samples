using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Transactions;

namespace SqlServer.DistributedTransactionCoordinator.Tester.Tests
{
    internal class EntityFramework6Test : ITest
    {
        private readonly PiEf6Context piContext;
        private readonly XiEf6Context xiContext;
        private readonly string currentRuntimeFramework;
        private bool isDisposed;

        public EntityFramework6Test(IConfiguration configuration, ILoggerFactory loggerFactory)
        {
#if NET
            this.currentRuntimeFramework = "NET";
#endif

#if NETFRAMEWORK
            this.currentRuntimeFramework = "NETFRAMEWORK";
#endif

            this.piContext = new PiEf6Context(configuration.GetConnectionString("PiContext"));

            this.xiContext = new XiEf6Context(configuration.GetConnectionString("XiContext"));

            this.piContext.Database.CreateIfNotExists();
            this.xiContext.Database.CreateIfNotExists();
        }

        public string Name => "Multiple EF 6 context";

        public async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            var messageTemplate = "[{1}][{2}] Hello, {0}!";

            using (var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                piContext.Logs.Add(new Log(string.Format(messageTemplate, nameof(PiEf6Context), nameof(EntityFramework6Test), this.currentRuntimeFramework)));
                await piContext.SaveChangesAsync();

                xiContext.Logs.Add(new Log(string.Format(messageTemplate, nameof(XiEf6Context), nameof(EntityFramework6Test), this.currentRuntimeFramework)));
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
                }

                isDisposed = true;
            }
        }
    }
}
