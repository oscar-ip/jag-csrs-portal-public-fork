using Csrs.Api.Models.Dynamics;

namespace Csrs.Api.Repositories
{
    public interface ICsrsPartyRepository : IInsertRepository<SSG_CsrsParty>, IUpdateRepository<SSG_CsrsParty>
    {
    }
}
