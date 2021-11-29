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
        Task<IList<OptionValue>> GetGenderPicklistAsync(CancellationToken cancellationToken);

        Task<IList<OptionValue>> GetIdentityPicklistAsync(CancellationToken cancellationToken);

        Task<IList<OptionValue>> GetProvincePicklistAsync(CancellationToken cancellationToken);

        Task<IList<OptionValue>> GetReferralPicklistAsync(CancellationToken cancellationToken);        
    }
}
