using Csrs.Api.Models;
using Csrs.Api.Models.Dynamics;
using Csrs.Api.Repositories;

namespace Csrs.Api.Services
{
    public class FileService : IFileService
    {
        private readonly ICsrsFileRepository _fileRepository;
        private readonly ILogger<FileService> _logger;

        public FileService(
            ICsrsFileRepository fileRepository,
            ILogger<FileService> logger)
        {
            _fileRepository = fileRepository ?? throw new ArgumentNullException(nameof(fileRepository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<IList<FileSummary>> GetPartyFileSummariesAsync(Guid partyId, CancellationToken cancellationToken)
        {
            using var scope = _logger.BeginScope(new Dictionary<string, object> { { "PartyId", partyId } });

            List<SSG_CsrsFile>? csrsFiles = await _fileRepository.GetFileSummaryByPartyAsync(partyId, cancellationToken);
            if (csrsFiles == null || csrsFiles.Count == 0)
            {
                _logger.LogDebug("No active or draft files found");
                return Array.Empty<FileSummary>();
            }

            var files = new List<FileSummary>(csrsFiles.Count);
            foreach (var csrsFile in csrsFiles)
            {
                var file = new FileSummary
                {
                    FileId = csrsFile.CsrsFileId,
                    UsersRole = GetPartyRole(partyId, csrsFile),
                    Status = GetFileStatus(csrsFile.StatusCode)
                };

                files.Add(file); 
            }
            
            return files;
        }

        private PartyRole GetPartyRole(Guid partyId, SSG_CsrsFile file)
        {            
            if (file!.Recipient?.PartyId == partyId) return PartyRole.Recipient;
            if (file!.Payor?.PartyId == partyId) return PartyRole.Payor;

            using var scope = _logger.BeginScope(new Dictionary<string, object> { { "FileId", file!.CsrsFileId! } });
            _logger.LogInformation("Could not determine party role on file, party is not designated as Recipient or Payor, party role will be Unknown");
            return PartyRole.Unknown;
        }

        private FileStatus GetFileStatus(int statusCode)
        {
            SSG_CsrsFile.StatusCodes? value = SSG_CsrsFile.StatusCodes.FromValue(statusCode);
            if (value is not null)
            {
                return (FileStatus)Enum.Parse(typeof(FileStatus), value.Name);
            }

            _logger.LogInformation("Could not decode {EntityLogicalName} status code value {StatusCode}, file status will be Unknown", SSG_CsrsFile.EntityLogicalName, statusCode);
            return FileStatus.Unknown;
        }
    }
}
