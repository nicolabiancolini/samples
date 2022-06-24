// See the LICENSE.TXT file in the project root for full license information.

using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using Microsoft.EntityFrameworkCore;

namespace Sample.SqlServer.Design
{
    internal class DesignContext : DbContext
    {
        public DesignContext([NotNull] DbContextOptions options)
            : base(options)
        {
        }

        protected override void OnModelCreating([NotNull] ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}
