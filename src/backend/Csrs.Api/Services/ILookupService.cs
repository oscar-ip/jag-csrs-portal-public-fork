using Csrs.Api.Models;

namespace Csrs.Api.Services
{
    public interface ILookupService
    {
        Task<IList<CourtLookupValue>> GetCourtLevelsAsync(CancellationToken cancellationToken);

        Task<IList<CourtLookupValue>> GetCourtLocationsAsync(CancellationToken cancellationToken);

    }
}
