using Microsoft.EntityFrameworkCore;

namespace SqlServer.DistributedTransactionCoordinator.Tester
{
    internal class SqlServerEfCoreContext : DbContext
    {
        public SqlServerEfCoreContext(DbContextOptions options)
            : base(options)
        {
        }

        public virtual DbSet<Log> Logs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Log>(builder =>
            {
                builder.HasKey(l => l.Timestamp);
            });
        }
    }
}
