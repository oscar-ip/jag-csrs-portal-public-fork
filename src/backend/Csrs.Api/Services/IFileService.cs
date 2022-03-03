using Csrs.Api.Models;
using Csrs.Interfaces.Dynamics.Models;
using File = Csrs.Api.Models.File;

namespace Csrs.Api.Services
{
    public interface IFileService
    {
        //Task<Tuple<string, string>> CreateFile(string partyId, string? otherPartyId, File file, CancellationToken cancellationToken);
        Task<Tuple<string, string>> CreateFile(MicrosoftDynamicsCRMssgCsrsparty party, MicrosoftDynamicsCRMssgCsrsparty otherParty, File file, CancellationToken cancellationToken);

        Task<string> UpdateCSRSAccountFile(string partyId, CSRSAccountFile file, CancellationToken cancellationToken);

    }
}
