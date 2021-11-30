using Csrs.Api.Models;
using Csrs.Api.Models.Dynamics;

namespace Csrs.Api.Repositories
{
    public interface ICsrsPartyRepository : IRepository<SSG_CsrsParty>
    {
        /// <summary>
        /// Gets all the <see cref="SSG_CsrsParty"/> Gener pick list values.
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<IList<LookupValue>> GetGenderPicklistAsync(CancellationToken cancellationToken);

        Task<IList<LookupValue>> GetIdentityPicklistAsync(CancellationToken cancellationToken);

        Task<IList<LookupValue>> GetProvincePicklistAsync(CancellationToken cancellationToken);

        Task<IList<LookupValue>> GetReferralPicklistAsync(CancellationToken cancellationToken);        
    }
}
