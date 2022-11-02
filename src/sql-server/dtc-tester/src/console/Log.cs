using System;

namespace SqlServer.DistributedTransactionCoordinator.Tester
{
    internal class Log
    {
        public Log(string message)
        {
            this.Message = message;
            this.Timestamp = DateTimeOffset.Now;
        }

        public DateTimeOffset Timestamp { get; set; }

        public string Message { get; set; }
    }
}
