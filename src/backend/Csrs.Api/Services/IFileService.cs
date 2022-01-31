using Csrs.Api.Models;
using Csrs.Interfaces.Dynamics.Models;
using File = Csrs.Api.Models.File;

namespace Csrs.Api.Services
{
    public interface IFileService
    {
        Task<Tuple<string, string>> CreateFile(string partyId, string? otherPartyId, File file, CancellationToken cancellationToken);
    }
}
