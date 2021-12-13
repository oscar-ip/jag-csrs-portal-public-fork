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
        protected const int Active = 1;

        /// <summary>
        /// The <see cref="IODataClient"/>.
        /// </summary>
        protected readonly IODataClient Client;
        protected ILogger<Repository<TEntity>> Logger { get; }

        protected Repository(IODataClient client, ILogger<Repository<TEntity>> logger)
        {
            Client = client ?? throw new ArgumentNullException(nameof(client));
            Logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task DeleteAsync(Guid key, CancellationToken cancellationToken)
        {
            if (key == Guid.Empty)
            {
                return;
            }

            await Client
                .For<TEntity>()
                .Key(key)
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
                    .Filter(_ => _.StatusCode == Active)
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

        public async Task<TEntity> InsertAsync(Dictionary<string, object?> entry, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(entry);

            using var scope = Logger.BeginScope(entry);
            Logger.LogTrace("Inserting new entity");

            TEntity entity = await Client
                .For<TEntity>()
                .Set(entry)
                .InsertEntryAsync(cancellationToken);

            return entity;
        }

        public async Task<TEntity> UpdateAsync(Guid key, Dictionary<string, object?> entry, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(entry);

            using var scope = Logger.BeginScope(entry);
            Logger.LogTrace("Updating existing entity");

            TEntity entity = await Client
                .For<TEntity>()
                .Key(key)
                .Set(entry)
                .UpdateEntryAsync(cancellationToken);

            return entity;
        }
    }
}
