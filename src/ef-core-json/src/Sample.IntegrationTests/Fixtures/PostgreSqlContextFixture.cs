// See the LICENSE.TXT file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace Sample.IntegrationTests.Fixtures
{
    public class PostgreSqlContextFixture : ContextFixture
    {
        private readonly ICollection<PostgreSqlDbContext> contexts;

        private readonly string userId;
        private readonly string host;
        private readonly string password;

        public PostgreSqlContextFixture()
        {
            this.contexts = new HashSet<PostgreSqlDbContext>();

            var vault = Path.Combine(Environment.CurrentDirectory, ".env.postgresql");
            if (File.Exists(vault))
            {
                var secrets = File.ReadAllLines(vault)
                    .Select(line => line.Split(':'))
                    .ToDictionary(pair => pair[0], pair => pair[1]);

                this.userId = secrets["user id"];
                this.host = secrets["host"];
                this.password = secrets["password"];
            }
        }

        public override IContext CreateContext()
        {
            return this.CreateContext(this.host, this.userId, this.password);
        }

        public IContext CreateContext(string host, string user, string password)
        {
            StringBuilder connectionsStringBuilder = new StringBuilder();
            connectionsStringBuilder.Append($"host={host};database={Guid.NewGuid()};username={user}");

            if (!string.IsNullOrEmpty(password))
            {
                connectionsStringBuilder.Append($";password={password}");
            }

            var optionsBuilder = new DbContextOptionsBuilder()
                .UseNpgsql(connectionsStringBuilder.ToString());

            var context = new PostgreSqlDbContext(optionsBuilder.Options);
            context.Database.EnsureCreated();

            this.AddDisposableObject(context);
            this.contexts.Add(context);

            return context;
        }

        protected override void BeforeDispose(IDisposable disposable)
        {
            this.contexts.Single(context => ReferenceEquals(context, disposable)).Database.EnsureDeleted();
        }

        protected override void AfterDispose(IDisposable disposable)
        {
        }
    }
}
