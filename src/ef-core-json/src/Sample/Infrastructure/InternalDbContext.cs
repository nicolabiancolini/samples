// See the LICENSE.TXT file in the project root for full license information.

using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Sample.Dtos;
using Sample.Entities;
using Sample.Infrastructure;
using Sample.Infrastructure.Storage;

namespace Sample
{
    public class InternalDbContext : DbContext, IContext
    {
        public InternalDbContext(DbContextOptions options)
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

            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Car>()
                .HasKey(car => car.Id);

            modelBuilder.Entity<Car>()
                .Property(car => car.Name);
        }
    }
}
