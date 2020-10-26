// See the LICENSE.TXT file in the project root for full license information.

using Microsoft.EntityFrameworkCore;
using Sample.Entities;

namespace Sample
{
    public class PostgreSqlDbContext : InternalDbContext
    {
        public PostgreSqlDbContext(DbContextOptions options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            if (modelBuilder is null)
            {
                throw new System.ArgumentNullException(nameof(modelBuilder));
            }

            modelBuilder.Entity<Car>()
                .Property(car => car.Metadata)
                .HasColumnType("jsonb");

            modelBuilder.Entity<Car>()
                .Ignore(car => car.NameOfRetailer);

            base.OnModelCreating(modelBuilder);
        }
    }
}
