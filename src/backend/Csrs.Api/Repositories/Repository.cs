using Csrs.Api.Models.Dynamics;
using Simple.OData.Client;
using System.Linq.Expressions;
using System.Net;

namespace Csrs.Api.Repositories
{
    /// <summary>
    /// Implements the generic repository interface.
    /// </summary>
    /// <typeparam name="TEntity">The type of entity.</typeparam>
    public abstract class Repository<TEntity> : IRepository<TEntity>, ILookupRepository<TEntity> where TEntity : Entity
    {
        /// <summary>
        /// The <see cref="IODataClient"/>.
        /// </summary>
        protected readonly IODataClient Client;
        
        protected Repository(IODataClient client)
        {
            Client = client ?? throw new ArgumentNullException(nameof(client));
        }

        public async Task DeleteAsync(TEntity entity, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(entity);

            await Client
                .For<TEntity>()
                .Key(entity.Id)
                .DeleteEntryAsync(cancellationToken);
        }

        public async Task<IList<TEntity>> GetAllAsync(Expression<Func<TEntity, object>> properties, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(properties);

            IEnumerable<TEntity>? entities = await Client
                .For<TEntity>()
                .Select(properties)
                .FindEntriesAsync(cancellationToken);

            return new List<TEntity>(entities);
        }

        public async Task<TEntity?> GetAsync(Guid id, Expression<Func<TEntity, object>> properties, CancellationToken cancellationToken)
        {
            if (id == Guid.Empty)
            {
                return null;
            }

            ArgumentNullException.ThrowIfNull(properties);

            try
            {
                TEntity entity = await Client
                    .For<TEntity>()
                    .Key(id)
                    .Select(properties)
                    .FindEntryAsync(cancellationToken);

                return entity;
            }
            catch (HttpRequestException exception) when (exception.IsTimedOut())
            {
                throw;
            }
            catch (WebRequestException exception) when (exception.Code == HttpStatusCode.NotFound)
            {
                return null;
            }
        }

        public async Task<TEntity> InsertAsync(TEntity entity, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(entity);

            entity = await Client
                .For<TEntity>()
                .Set(entity)
                .InsertEntryAsync(cancellationToken);

            return entity;
        }

        public async Task<TEntity> UpdateAsync(TEntity entity, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(entity);

            entity = await Client
                .For<TEntity>()
                .Key(entity.Id)
                .Set(entity)
                .UpdateEntryAsync(cancellationToken);

            return entity;
        }
    }
}
