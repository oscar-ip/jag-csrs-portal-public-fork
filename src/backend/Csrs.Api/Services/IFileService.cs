using Csrs.Api.Models;

namespace Csrs.Api.Services
{
    public interface IFileService
    {
        Task<IList<FileSummary>> GetPartyFileSummariesAsync(Guid partyId, CancellationToken cancellationToken);
    }
}
