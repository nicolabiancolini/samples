using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;
using SqlServer.DistributedTransactionCoordinator.Tester.Tests;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SqlServer.DistributedTransactionCoordinator.Tester
{
    internal class Program
    {
        private static async Task Main(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .AddEnvironmentVariables()
                .Build();

            var loggerFactory = new LoggerFactory();
            loggerFactory.AddProvider(new ConsoleLoggerProvider(new ConsoleLoggerOptions()));

            var logger = loggerFactory.CreateLogger<TestContext>();

            var context = new TestContext(
                logger,
                new ITest[]
                {
                    new EntityFramework6Test(configuration, loggerFactory),
                    new EntityFrameworkCoreTest(configuration, loggerFactory),
                    new SqlClientTest(configuration, loggerFactory)
                });
            await context.ExecuteAsync();

            if (context.TestExceptions.Any())
            {
                foreach (var ex in context.TestExceptions)
                {
                    logger.LogError(ex.Value, ex.Key.Name);
                }

                Environment.Exit(1);
            }
            else
            {
                Environment.Exit(0);
            }
        }
    }
}

