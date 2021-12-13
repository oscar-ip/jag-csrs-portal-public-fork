using AutoMapper;
using Csrs.Api.Models;
using Csrs.Api.Models.Dynamics;
using Csrs.Api.Repositories;
using System.Diagnostics;
using File = Csrs.Api.Models.File;

namespace Csrs.Api.Services
{
    public class FileService : IFileService
    {
        private readonly ICsrsFileRepository _fileRepository;
        private readonly ICsrsChildRepository _childRepository;
        private readonly IInsertOrUpdateFieldMapper<File, SSG_CsrsFile> _fileInsertOrUpdateFieldMapper;
        private readonly IInsertOrUpdateFieldMapper<Child, SSG_CsrsChild> _childInsertOrUpdateFieldMapper;
        private readonly IMapper _mapper;
        private readonly ILogger<FileService> _logger;

        public FileService(
            ICsrsFileRepository fileRepository,
            ICsrsChildRepository childRepository,
            IInsertOrUpdateFieldMapper<File, SSG_CsrsFile> fileInsertOrUpdateFieldMapper,
            IMapper mapper,
            ILogger<FileService> logger)
        {
            _fileRepository = fileRepository ?? throw new ArgumentNullException(nameof(fileRepository));
            _childRepository = childRepository;
            _fileInsertOrUpdateFieldMapper = fileInsertOrUpdateFieldMapper;
            _mapper = mapper;
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<IList<FileSummary>> GetPartyFileSummariesAsync(Guid partyId, CancellationToken cancellationToken)
        {
            using var scope = _logger.AddPartyId(partyId);

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

            Debug.Assert(file.CsrsFileId is not null);

            using var scope = _logger.AddFileId(file.CsrsFileId.Value);

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

        public async Task<File> CreateFile(Party party, Party? otherParty, File file, CancellationToken cancellationToken)
        {
            var fields = _fileInsertOrUpdateFieldMapper.GetFieldsForInsert(file);

            // map the party and other party to recipient and payor
            if (file.UsersRole == PartyRole.Recipient)
            {
                _logger.LogDebug("Setting party as recipient");
                fields.Add(SSG_CsrsFile.Attributes.ssg_recipient, party.PartyId);

                if (otherParty is not null)
                {
                    _logger.LogDebug("Setting other party as payor");
                    fields.Add(SSG_CsrsFile.Attributes.ssg_payor, otherParty.PartyId);
                }
            }
            else if (file.UsersRole == PartyRole.Payor)
            {
                _logger.LogDebug("Setting party as payor");
                fields.Add(SSG_CsrsFile.Attributes.ssg_payor, party.PartyId);

                if (otherParty is not null)
                {
                    _logger.LogDebug("Setting other party as recipient");
                    fields.Add(SSG_CsrsFile.Attributes.ssg_recipient, otherParty.PartyId);
                }
            }
            else
            {
                using var scope = _logger.AddProperty("PartyRole", file.UsersRole);
                _logger.LogInformation("Party role is not in allowable values, expected Recipient or Payor");
            }


            var csrsFile = await _fileRepository.InsertAsync(fields, cancellationToken);
            using var fileScope = _logger.AddFileId(csrsFile.Key);

            if (file.Children is not null)
            {
                foreach (var child in file.Children)
                {
                    var childFields = _childInsertOrUpdateFieldMapper.GetFieldsForInsert(child);
                    // add link to child
                    childFields.Add(SSG_CsrsChild.Attributes.ssg_filenumber, csrsFile.CsrsFileId);

                    _childRepository.InsertAsync(childFields, cancellationToken);
                }
            }
            else
            {
                _logger.LogInformation("New file contains no children");
            }


            file = _mapper.Map<File>(csrsFile);

            return file;

        }
    }
}
