// See the LICENSE.TXT file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Sample.Infrastructure
{
    public abstract class Repository<TEntity> : IRepository<TEntity>
        where TEntity : class
    {
        private readonly DbSet<TEntity> internalSet;

        public Repository(DbSet<TEntity> set)
        {
            this.internalSet = set;
        }

        public virtual async Task<TEntity> FindAsync(Guid key, CancellationToken cancellationToken = default)
        {
            return await this.internalSet.FindAsync(key, cancellationToken);
        }

        public virtual IQueryable<TEntity> Fetch(bool withTracking = false)
        {
            return withTracking ? this.internalSet.AsNoTracking() : this.internalSet;
        }

        public virtual IQueryable<TEntity> Fetch(Expression<Func<TEntity, bool>> whereExpression, bool withTracking = false)
        {
            return this.Fetch(withTracking).Where(whereExpression);
        }

        public async Task<IEnumerable<TEntity>> MaterializeAsync(Expression<Func<TEntity, bool>> whereExpression, bool withTracking = false, CancellationToken cancellationToken = default)
        {
            return (await this.Fetch(whereExpression, withTracking).ToListAsync(cancellationToken).ConfigureAwait(false)).AsEnumerable();
        }

        public virtual IAsyncEnumerable<TEntity> MaterializeAllAsync(CancellationToken cancellationToken = default)
        {
            return this.internalSet.AsAsyncEnumerable();
        }

        public virtual Task UpdateAsync(TEntity entity, CancellationToken cancellationToken = default)
        {
            this.internalSet.Update(entity);

            return Task.CompletedTask;
        }

        public virtual Task RemoveAsync(TEntity entity, CancellationToken cancellationToken = default)
        {
            this.internalSet.Remove(entity);

            return Task.CompletedTask;
        }

        public async Task AddAsync(TEntity entity, CancellationToken cancellationToken)
        {
            await this.internalSet.AddAsync(entity, cancellationToken);
        }
    }
}
