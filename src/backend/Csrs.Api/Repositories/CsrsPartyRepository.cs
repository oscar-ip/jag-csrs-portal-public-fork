using Csrs.Api.Models.Dynamics;
using Simple.OData.Client;

namespace Csrs.Api.Repositories
{
    public class CsrsPartyRepository : Repository<SSG_CsrsParty>, ICsrsPartyRepository
    {
        public CsrsPartyRepository(IODataClient client) : base(client)
        {
        }
    }
}
