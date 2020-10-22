using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Xunit.Abstractions;

namespace Sample.IntegrationTests.Fixtures
{
    public class SqlServerContextFixture : ContextFixture
    {
        private readonly ICollection<DbContext> contexts;

        private readonly string userId;
        private readonly string server;
        private readonly string password;

        public SqlServerContextFixture()
        {
            this.contexts = new HashSet<DbContext>();

            var vault = Path.Combine(Environment.CurrentDirectory, ".env");
            if (File.Exists(vault))
            {
                IDictionary<string,string> secrets = File.ReadAllLines(vault)
                    .Select(line => line.Split(':'))
                    .ToDictionary(pair => pair[0], pair => pair[1]);

                this.userId = secrets["user id"];
                this.server = secrets["server"];
                this.password = secrets["password"];
            }
        }

        public IContext CreateContext()
        {
            return this.CreateContext(this.server, this.userId, this.password);
        }

        public IContext CreateContext(string server = ".", string user = "sa", string password = null)
        {
            StringBuilder connectionsStringBuilder = new StringBuilder();
            connectionsStringBuilder.Append($"server={server};database={Guid.NewGuid()};user id={user}");

            if (!string.IsNullOrEmpty(password))
            {
                connectionsStringBuilder.Append($";password={password}");
            }

            var optionsBuilder = new DbContextOptionsBuilder()
                .UseSqlServer(connectionsStringBuilder.ToString());

            var context = new DbContext(optionsBuilder.Options);
            context.Database.EnsureCreated();

            this.AddDisposableObject(context);
            this.contexts.Add(context);

            return context;
        }

        protected override void Dispose(IDisposable disposable)
        {
            this.contexts.Single(context => ReferenceEquals(context, disposable)).Database.EnsureDeleted();
        }
    }
}
