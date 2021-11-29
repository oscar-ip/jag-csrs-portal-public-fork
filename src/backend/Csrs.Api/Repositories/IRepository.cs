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
        /// <summary>
        /// Gets an entity by id.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="properties"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"><paramref name="properties"/> is null.</exception>
        Task<TEntity?> GetAsync(Guid id, Expression<Func<TEntity, object>> properties, CancellationToken cancellationToken);

        /// <summary>
        /// Insert an entity.
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"><paramref name="entity"/> is null.</exception>
        Task<TEntity> InsertAsync(TEntity entity, CancellationToken cancellationToken);


        /// <summary>
        /// Update an entity.
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"><paramref name="entity"/> is null.</exception>
        Task<TEntity> UpdateAsync(TEntity entity, CancellationToken cancellationToken);

        /// <summary>
        /// Delete an entity
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"><paramref name="entity"/> is null.</exception>
        Task DeleteAsync(TEntity entity, CancellationToken cancellationToken);
    }
}
