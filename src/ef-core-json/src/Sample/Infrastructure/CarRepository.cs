using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.Internal;
using Sample.Entities;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Sample.Infrastructure
{
#pragma warning disable CA1710 // Identifiers should have correct suffix
    public sealed class CarRepository : Repository<Car>
#pragma warning restore CA1710 // Identifiers should have correct suffix
    {
        public CarRepository(DbSet<Car> set)
            : base(set)
        {
        }
    }
}
