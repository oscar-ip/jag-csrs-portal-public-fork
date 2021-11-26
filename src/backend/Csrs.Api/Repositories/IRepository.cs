using Csrs.Api.Models.Dynamics;
using System.Linq.Expressions;

namespace Csrs.Api.Repositories
{
    /// <summary>
    /// Generic repository interface
    /// </summary>
    /// <typeparam name="TEntity">The type of entity</typeparam>
    public interface IRepository<TEntity> where TEntity : Entity
    {
        Task<TEntity?> GetAsync(Guid id, Expression<Func<TEntity, object>> properties, CancellationToken cancellationToken);
        Task<TEntity> InsertAsync(TEntity entity, CancellationToken cancellationToken);
        Task<TEntity> UpdateAsync(TEntity entity, CancellationToken cancellationToken);
        Task DeleteAsync(TEntity entity, CancellationToken cancellationToken);
    }
}
