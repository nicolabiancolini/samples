// See the LICENSE.TXT file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace Sample.IntegrationTests.Fixtures
{
    public class SqlServerContextFixture : ContextFixture
    {
        private readonly ICollection<SqlServerDbContext> contexts;

        private readonly string userId;
        private readonly string server;
        private readonly string password;

        public SqlServerContextFixture()
        {
            this.contexts = new HashSet<SqlServerDbContext>();

            var vault = Path.Combine(Environment.CurrentDirectory, ".env.sqlserver");
            if (File.Exists(vault))
            {
                var secrets = File.ReadAllLines(vault)
                    .Select(line => line.Split(':'))
                    .ToDictionary(pair => pair[0], pair => pair[1]);

                this.userId = secrets["user id"];
                this.server = secrets["server"];
                this.password = secrets["password"];
            }
        }

        public override IContext CreateContext()
        {
            return this.CreateContext(this.server, this.userId, this.password);
        }

        public IContext CreateContext(string server, string user, string password)
        {
            StringBuilder connectionsStringBuilder = new StringBuilder();
            connectionsStringBuilder.Append($"server={server};database={Guid.NewGuid()};user id={user}");

            if (!string.IsNullOrEmpty(password))
            {
                connectionsStringBuilder.Append($";password={password}");
            }

            var optionsBuilder = new DbContextOptionsBuilder()
                .UseSqlServer(connectionsStringBuilder.ToString());

            var context = new SqlServerDbContext(optionsBuilder.Options);
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
