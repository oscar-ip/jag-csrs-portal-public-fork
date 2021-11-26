using Csrs.Api.Models.Dynamics;
using Simple.OData.Client;

namespace Csrs.Api.Repositories
{
    public class CsrsFeedbackRepository : Repository<SSG_CsrsFeedback>, ICsrsFeedbackRepository
    {
        public CsrsFeedbackRepository(IODataClient client) : base(client)
        {
        }
    }
}
