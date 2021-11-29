using Csrs.Api.Models.Dynamics;
using Simple.OData.Client;

namespace Csrs.Api.Repositories
{
    public class CourtLevelRepository : Repository<SSG_CsrsBCCourtLevel>, ICourtLevelRepository
    {
        public CourtLevelRepository(IODataClient client) : base(client)
        {
        }
    }
}
