using Csrs.Api.Models;
using Csrs.Api.Models.Dynamics;
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

        Task<List<SSG_CsrsParty>> GetByBCeIdAsync(string id, Expression<Func<SSG_CsrsParty, object>> properties, CancellationToken cancellationToken);
    }
}
