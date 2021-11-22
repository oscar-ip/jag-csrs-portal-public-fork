using Csrs.Api.Models.Dynamics;
using Simple.OData.Client;

namespace Csrs.Api.Repositories
{
    public class CsrsPartyRepository : Repository, ICsrsPartyRepository
    {
        public CsrsPartyRepository(IODataClient client) : base(client)
        {
        }

        public Task<SSG_CsrsParty> InsertAsync(SSG_CsrsParty entity, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<SSG_CsrsParty> UpdateAsync(SSG_CsrsParty entity, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
