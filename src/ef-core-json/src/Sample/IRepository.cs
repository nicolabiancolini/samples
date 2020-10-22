// See the LICENSE.TXT file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace Sample
{
    public interface IRepository<TEntity>
        where TEntity : class
    {
        Task AddAsync(TEntity entity, CancellationToken cancellationToken = default);

        Task UpdateAsync(TEntity entity, CancellationToken cancellationToken = default);

        Task RemoveAsync(TEntity entity, CancellationToken cancellationToken = default);

        Task<TEntity> FindAsync(Guid key, CancellationToken cancellationToken = default);

        IQueryable<TEntity> Fetch(bool withTracking = false);

        IQueryable<TEntity> Fetch(Expression<Func<TEntity, bool>> whereExpression, bool withTracking = false);

        Task<IEnumerable<TEntity>> MaterializeAsync(Expression<Func<TEntity, bool>> whereExpression, bool withTracking = false, CancellationToken cancellationToken = default);

        IAsyncEnumerable<TEntity> MaterializeAllAsync(CancellationToken cancellationToken = default);
    }
}
