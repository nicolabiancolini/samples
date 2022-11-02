using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace SqlServer.DistributedTransactionCoordinator.Tester.Tests
{
    internal class SqlClientTest : ITest
    {
        private readonly SqlConnection connection;
        private readonly PiEfCoreContext piContext;
        private readonly XiEfCoreContext xiContext;
        private readonly string piDatabaseName;
        private readonly string xiDatabaseName;
        private readonly string currentRuntimeFramework;
        private bool isDisposed;

        public SqlClientTest(IConfiguration configuration, ILoggerFactory loggerFactory)
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
            this.piDatabaseName = this.piContext.Database.GetDbConnection().Database;
            this.xiDatabaseName = this.xiContext.Database.GetDbConnection().Database;
            this.connection = new SqlConnection(configuration.GetConnectionString(nameof(PiEf6Context)));
        }

        public string Name => "Raw SQL query";

        public async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            await this.connection.OpenAsync();
            var command = new SqlCommand(
$@"BEGIN DISTRIBUTED TRANSACTION;
	USE [{piDatabaseName}];
	INSERT INTO [Logs] ([Timestamp], [Message])
		VALUES (CURRENT_TIMESTAMP, N'[{nameof(SqlClientTest)}][{this.currentRuntimeFramework}] Hello, PiContext!');
	 
	USE [{xiDatabaseName}];
	INSERT INTO [Logs] ([Timestamp], [Message])
		VALUES (CURRENT_TIMESTAMP, N'[{nameof(SqlClientTest)}][{this.currentRuntimeFramework}] Hello, XiContext!');
COMMIT TRANSACTION;",
connection);

            await command.ExecuteNonQueryAsync();
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
                    this.connection.Close();
                }

                isDisposed = true;
            }
        }
    }
}
