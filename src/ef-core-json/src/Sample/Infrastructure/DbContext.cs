// See the LICENSE.TXT file in the project root for full license information.

using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Sample.Dtos;
using Sample.Entities;
using Sample.Infrastructure;
using Sample.Infrastructure.Storage;

namespace Sample
{
    public class DbContext : Microsoft.EntityFrameworkCore.DbContext, IContext
    {
        private readonly ILoggerFactory loggerFactory;

        public DbContext(DbContextOptions options)
            : base(options)
        {
            this.Cars = new CarRepository(this.Set<Car>());
        }

        public IRepository<Car> Cars { get; private set; }

        public Task CommitAsync(CancellationToken cancellationToken)
        {
            return this.SaveChangesAsync(cancellationToken);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            if (modelBuilder is null)
            {
                throw new System.ArgumentNullException(nameof(modelBuilder));
            }

            modelBuilder.Entity<Car>()
                .HasKey(car => car.Id);

            modelBuilder.Entity<Car>()
                .Property(car => car.Name);

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
