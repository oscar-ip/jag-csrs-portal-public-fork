using Csrs.Api.Models.Dynamics;
using Simple.OData.Client;

namespace Csrs.Api.Repositories
{
    public class CourtLocationRepository : Repository<SSG_IJSSBCCourtlocation>, ICourtLocationRepository
    {
        public CourtLocationRepository(IODataClient client) : base(client)
        {
        }
    }
}
