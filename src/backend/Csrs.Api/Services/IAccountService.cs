using Csrs.Api.Models;

namespace Csrs.Api.Services
{
    public interface IAccountService
    {
        Task<Party?> GetPartyByBCeIdAsync(string bceidGuid, CancellationToken cancellationToken);

        Task<IList<LookupValue>> GetGendersAsync(CancellationToken cancellationToken);

        Task<IList<LookupValue>> GetIdentitiesAsync(CancellationToken cancellationToken);

        Task<IList<LookupValue>> GetPreferredContactMethodsAsync(CancellationToken cancellationToken);

        Task<IList<LookupValue>> GetProvincesAsync(CancellationToken cancellationToken);

        Task<IList<LookupValue>> GetReferralsAsync(CancellationToken cancellationToken);

        Task<Party> CreateOrUpdateAsync(Party party, CancellationToken cancellationToken);
        
    }
}
