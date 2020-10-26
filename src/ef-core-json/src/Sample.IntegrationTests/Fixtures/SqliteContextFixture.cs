// See the LICENSE.TXT file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Sample.IntegrationTests.Fixtures
{
    public class SqliteContextFixture : ContextFixture
    {
        private readonly IDictionary<SqliteDbContext, string> contexts;
        private readonly string tempDir;

        public SqliteContextFixture()
        {
            this.contexts = new Dictionary<SqliteDbContext, string>();

            this.tempDir = Path.Combine(Path.GetTempPath(), "ef-core-json");

            if (!Directory.Exists(this.tempDir))
            {
                Directory.CreateDirectory(this.tempDir);
            }
        }

        public override IContext CreateContext()
        {
            string filePath = Path.Combine(this.tempDir, Guid.NewGuid().ToString());

            File.Create(filePath).Dispose();

            var optionsBuilder = new DbContextOptionsBuilder()
                .UseSqlite($"Filename={filePath}");

            var context = new SqliteDbContext(optionsBuilder.Options);
            context.Database.EnsureCreated();

            this.AddDisposableObject(context);
            this.contexts.Add(context, filePath);

            return context;
        }

        protected override void BeforeDispose(IDisposable disposable)
        {
        }

        protected override void AfterDispose(IDisposable disposable)
        {
            File.Delete(this.contexts.Single(context => ReferenceEquals(context.Key, disposable)).Value);
        }
    }
}
