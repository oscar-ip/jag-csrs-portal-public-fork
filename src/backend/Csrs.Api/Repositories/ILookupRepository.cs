using Csrs.Api.Models.Dynamics;
using System.Linq.Expressions;

namespace Csrs.Api.Repositories
{
    /// <summary>
    /// Generic lookup repository
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public interface ILookupRepository<TEntity> where TEntity : Entity
    {
        /// <summary>
        /// Gets all the entities of the given type.
        /// </summary>
        /// <param name="properties"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"><paramref name="cancellationToken"/> is null.</exception>
        Task<IList<TEntity>> GetAllAsync(Expression<Func<TEntity, object>> properties, CancellationToken cancellationToken);
    }
}
