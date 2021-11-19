using Csrs.Api.Models.Dynamics;
using Simple.OData.Client;

namespace Csrs.Api.Repositories
{
    public class CsrsFileRepository : Repository, ICsrsFileRepository
    {
        public CsrsFileRepository(IODataClient client) : base(client)
        {
        }

        public async Task<SSG_CsrsFile?> GetAsync(Guid id, CancellationToken cancellationToken)
        {
            SSG_CsrsFile entity = await Client
                .For<SSG_CsrsFile>()
                .Key(id)
                .Select(SSG_CsrsFile.AllProperties)
                .FindEntryAsync(cancellationToken);

            return entity;
        }

        public async Task<SSG_CsrsFile> InsertAsync(SSG_CsrsFile entity, CancellationToken cancellationToken)
        {
            entity = await Client
                .For<SSG_CsrsFile>()
                .Set(entity)
                .InsertEntryAsync(cancellationToken);

            return entity;
        }

        public async Task<SSG_CsrsFile> UpdateAsync(SSG_CsrsFile entity, CancellationToken cancellationToken)
        {
            entity = await Client
                .For<SSG_CsrsFile>()
                .Key(entity.Id)
                .Set(entity)
                .UpdateEntryAsync(cancellationToken);

            return entity;
        }
    }
}
