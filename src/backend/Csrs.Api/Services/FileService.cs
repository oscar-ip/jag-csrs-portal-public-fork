using AutoMapper;
using Csrs.Api.Models;
using Csrs.Api.Models.Dynamics;
using Csrs.Api.Repositories;
using File = Csrs.Api.Models.File;

namespace Csrs.Api.Services
{
    public class FileService : IFileService
    {
        private readonly ICsrsFileRepository _fileRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<FileService> _logger;

        public FileService(
            ICsrsFileRepository fileRepository,
            IMapper mapper,
            ILogger<FileService> logger)
        {
            _fileRepository = fileRepository ?? throw new ArgumentNullException(nameof(fileRepository));
            _mapper = mapper;
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
            if (statusCode == SSG_CsrsFile.Active.Id) return FileStatus.Active;
            if (statusCode == SSG_CsrsFile.Draft.Id) return FileStatus.Draft;

            _logger.LogInformation("Could not decode {EntityLogicalName} status code value {StatusCode}, file status will be Unknown", SSG_CsrsFile.EntityLogicalName, statusCode);
            return FileStatus.Unknown;
        }

        public async Task<File> CreateFile(Party party, File file, CancellationToken cancellationToken)
        {
            SSG_CsrsFile csrsFile = _mapper.Map<SSG_CsrsFile>(file);

            // TODO: handle other party

            if (file.UsersRole == PartyRole.Recipient)
            {
                _logger.LogDebug("Setting party as recipient");
                csrsFile.Recipient = new SSG_CsrsParty { PartyId = party.PartyId };
            }
            else if (file.UsersRole == PartyRole.Payor)
            {
                _logger.LogDebug("Setting party as payor");
                csrsFile.Payor = new SSG_CsrsParty { PartyId = party.PartyId };
            }

            csrsFile.StatusCode = SSG_CsrsFile.Draft.Id;

            csrsFile = await _fileRepository.InsertAsync(csrsFile, cancellationToken);

            file = _mapper.Map<File>(csrsFile);

            return file;

        }
    }
}
