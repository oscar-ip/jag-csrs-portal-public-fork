using Csrs.Api.Models;
using File = Csrs.Api.Models.File;

namespace Csrs.Api.Services
{
    public interface IFileService
    {
        Task<IList<FileSummary>> GetPartyFileSummariesAsync(Guid partyId, CancellationToken cancellationToken);

        Task<File> CreateFile(Party party, File file, CancellationToken cancellationToken);
    }
}
