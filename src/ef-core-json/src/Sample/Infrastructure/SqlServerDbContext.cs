// See the LICENSE.TXT file in the project root for full license information.

using Microsoft.EntityFrameworkCore;
using Sample.Dtos;
using Sample.Entities;
using Sample.Infrastructure.Storage;

namespace Sample
{
    public class SqlServerDbContext : InternalDbContext
    {
        public SqlServerDbContext(DbContextOptions options)
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
                .HasConversion(new JsonValueConverter<Metadata>());

            modelBuilder.Entity<Car>()
                .Property(car => car.NameOfRetailer)
                .HasComputedColumnSql("JSON_VALUE(Metadata, '$.Retailer.Name')");

            base.OnModelCreating(modelBuilder);
        }
    }
}
