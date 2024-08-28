using System.Data.Entity;

namespace SqlServer.DistributedTransactionCoordinator.Tester
{
    internal class SqlServerEf6Context : DbContext
    {
        public SqlServerEf6Context(string connectionString)
            : base(connectionString)
        {
        }

        public virtual DbSet<Log> Logs { get; set; }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Log>().HasKey(l => l.Timestamp);
        }
    }
}
