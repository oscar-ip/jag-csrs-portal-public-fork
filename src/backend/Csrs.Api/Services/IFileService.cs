using Csrs.Api.Models;
using File = Csrs.Api.Models.File;

namespace Csrs.Api.Services
{
    public interface IFileService
    {
        Task<File> CreateFile(Party party, Party? otherOarty, File file, CancellationToken cancellationToken);
    }
}
