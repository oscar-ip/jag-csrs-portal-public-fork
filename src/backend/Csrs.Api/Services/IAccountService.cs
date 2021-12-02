using Csrs.Api.Models;

namespace Csrs.Api.Services
{
    public interface IAccountService
    {
        Task<Party?> GetPartyByBCeIdAsync(string bceidGuid, CancellationToken cancellationToken);

        Task<Party?> GetPartyAsync(Guid id, CancellationToken cancellationToken);
    }
}
