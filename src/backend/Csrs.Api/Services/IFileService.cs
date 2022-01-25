using Csrs.Api.Models;
using File = Csrs.Api.Models.File;

namespace Csrs.Api.Services
{
    public interface IFileService
    {
        Task<IList<FileSummary>> GetPartyFileSummariesAsync(Guid partyId, CancellationToken cancellationToken);

        Task<File> CreateFile(Party party, Party? otherOarty, File file, CancellationToken cancellationToken);
    }

    public interface IChildService
    {
        Task CreateChild();
    }

    public class ChildService : IChildService
    {
        public Task CreateChild()
        {
            throw new NotImplementedException();
        }
    }
}
