using Csrs.Api.Models;
using Csrs.Api.Models.Dynamics;
using Simple.OData.Client;
using System.Linq.Expressions;

namespace Csrs.Api.Repositories
{
    public interface ICsrsPartyRepository : IRepository<SSG_CsrsParty>
    {
        /// <summary>
        /// Gets all the <see cref="SSG_CsrsParty"/> Gener pick list values.
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<IList<LookupValue>> GetGendersAsync(CancellationToken cancellationToken);

        Task<IList<LookupValue>> GetIdentitiesAsync(CancellationToken cancellationToken);

        Task<IList<LookupValue>> GetProvincesAsync(CancellationToken cancellationToken);

        Task<IList<LookupValue>> GetReferralsAsync(CancellationToken cancellationToken);

        Task<IList<LookupValue>> GetPreferredContactMethodsAsync(CancellationToken cancellationToken);

        Task<List<SSG_CsrsParty>> GetByBCeIdAsync(Guid id, Expression<Func<SSG_CsrsParty, object>> properties, CancellationToken cancellationToken);
    }


    public interface ICsrsChildRepository : IRepository<SSG_CsrsChild>
    {
        Task<List<SSG_CsrsChild>> GetChildrenOnFileAsync(Guid fileId, Expression<Func<SSG_CsrsChild, object>> properties, CancellationToken cancellationToken);
    }

    public class CsrsChildRepository : Repository<SSG_CsrsChild>, ICsrsChildRepository
    {
        public CsrsChildRepository(IODataClient client, ILogger<CsrsChildRepository> logger) : base(client, logger)
        {
        }

        public async Task<List<SSG_CsrsChild>> GetChildrenOnFileAsync(Guid fileId, Expression<Func<SSG_CsrsChild, object>> properties, CancellationToken cancellationToken)
        {
            var client = Client.For<SSG_CsrsChild>();

            using var scope = Logger.AddFileId(fileId);
            Logger.LogDebug("Looking up children on file");

            IEnumerable<SSG_CsrsChild> entries = await client
                .Filter(_ => _.FileId != null && _.FileId.CsrsFileId == fileId && _.StatusCode == Active)
                .Select(properties)
                .FindEntriesAsync(cancellationToken);

            var entities = entries.ToList();
            return entities;
        }
    }
}
