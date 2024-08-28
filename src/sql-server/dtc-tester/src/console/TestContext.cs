using Microsoft.Extensions.Logging;
using SqlServer.DistributedTransactionCoordinator.Tester.Tests;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;

namespace SqlServer.DistributedTransactionCoordinator.Tester
{
    internal class TestContext
    {
        private readonly ILogger<TestContext> logger;
        private readonly IEnumerable<ITest> tests;
        private readonly IDictionary<ITest, Exception> testExceptions;

        public TestContext(ILogger<TestContext> logger, IEnumerable<ITest> tests)
        {
            this.logger = logger;
            this.tests = tests;
            this.testExceptions = new Dictionary<ITest, Exception>();
        }

        public IReadOnlyDictionary<ITest, Exception> TestExceptions => new ReadOnlyDictionary<ITest, Exception>(this.testExceptions);

        public async Task ExecuteAsync(CancellationToken cancellationToken = default)
        {
            foreach (var test in this.tests)
            {
                this.logger.LogInformation("Run test: {Name}", test.Name);
                try
                {
                    await test.ExecuteAsync(cancellationToken);
                }
                catch (Exception ex)
                {
                    this.testExceptions.Add(test, ex);
                }

                test.Dispose();
            }
        }
    }
}
