using Csrs.Api.Models.Dynamics;
using Simple.OData.Client;
using System.Net;

namespace Csrs.Api.Repositories
{
    /// <summary>
    /// Implements the generic repository interface
    /// </summary>
    /// <typeparam name="TEntity">The type of entity.</typeparam>
    public abstract class Repository<TEntity> : IRepository<TEntity> where TEntity : Entity
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
            if (entity is null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            await Client
                .For<TEntity>()
                .Key(entity.Id)
                .DeleteEntryAsync(cancellationToken);
        }

        public async Task<TEntity?> GetAsync(Guid id, System.Linq.Expressions.Expression<Func<TEntity, object>> properties, CancellationToken cancellationToken)
        {
            if (properties is null)
            {
                throw new ArgumentNullException(nameof(properties));
            }

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
            if (entity is null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            entity = await Client
                .For<TEntity>()
                .Set(entity)
                .InsertEntryAsync(cancellationToken);

            return entity;
        }

        public async Task<TEntity> UpdateAsync(TEntity entity, CancellationToken cancellationToken)
        {
            if (entity is null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            entity = await Client
                .For<TEntity>()
                .Key(entity.Id)
                .Set(entity)
                .UpdateEntryAsync(cancellationToken);

            return entity;
        }
    }
}
