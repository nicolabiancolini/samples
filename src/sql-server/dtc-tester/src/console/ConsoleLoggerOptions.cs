using Microsoft.Extensions.Options;
using System;

namespace SqlServer.DistributedTransactionCoordinator.Tester
{
    internal class ConsoleLoggerOptions : IOptionsMonitor<Microsoft.Extensions.Logging.Console.ConsoleLoggerOptions>, IDisposable
    {
        public ConsoleLoggerOptions()
        {
            this.CurrentValue = new Microsoft.Extensions.Logging.Console.ConsoleLoggerOptions();
        }

        public Microsoft.Extensions.Logging.Console.ConsoleLoggerOptions CurrentValue { get; private set; }

        public void Dispose()
        {
        }

        public Microsoft.Extensions.Logging.Console.ConsoleLoggerOptions Get(string name)
        {
            return this.CurrentValue;
        }

        public IDisposable OnChange(Action<Microsoft.Extensions.Logging.Console.ConsoleLoggerOptions, string> listener)
        {
            return this;
        }
    }
}
