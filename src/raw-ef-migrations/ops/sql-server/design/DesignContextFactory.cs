// See the LICENSE.TXT file in the project root for full license information.

using System;
using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Sample.SqlServer.Design
{
    internal class DesignContextFactory : IDesignTimeDbContextFactory<DesignContext>, IDisposable
    {
        private readonly DbContextOptions options;
        private readonly ILoggerFactory loggerFactory;
        private bool isDisposed;

        public DesignContextFactory()
        {
            var configuration = new ConfigurationBuilder()
                .AddJsonFile(Path.Combine(Environment.CurrentDirectory, "settings.json"))
                .AddEnvironmentVariables()
                .Build();

            this.loggerFactory = LoggerFactory.Create(options => options.AddConsole());

            this.options = new DbContextOptionsBuilder<DesignContext>()
                .UseSqlServer(configuration.GetConnectionString("SqlServerDesignTime"))
                .EnableSensitiveDataLogging()
                .UseLoggerFactory(this.loggerFactory)
                .Options;
        }

        public DesignContextFactory(DbContextOptions options)
        {
            this.options = options;
            this.loggerFactory = LoggerFactory.Create(options => options.AddConsole());
        }

        public DesignContext CreateDbContext(string[] args)
        {
            return new DesignContext(this.options);
        }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            this.Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!this.isDisposed)
            {
                if (disposing)
                {
                    this.loggerFactory.Dispose();
                }

                this.isDisposed = true;
            }
        }
    }
}
