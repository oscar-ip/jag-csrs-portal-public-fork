using AutoMapper;
using Csrs.Api.Models;
using Csrs.Api.Repositories;
using File = Csrs.Api.Models.File;

namespace Csrs.Api.Services
{
    public class FileService : IFileService
    {
        private readonly IMapper _mapper;
        private readonly ILogger<FileService> _logger;

        public FileService(
            IMapper mapper,
            ILogger<FileService> logger)
        {
            _mapper = mapper;
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<File> CreateFile(Party party, Party? otherParty, File file, CancellationToken cancellationToken)
        {
            //var fields = new Dictionary<string, object>();

            //// map the party and other party to recipient and payor
            //if (file.UsersRole == PartyRole.Recipient)
            //{
            //    _logger.LogDebug("Setting party as recipient");
            //    fields.Add(SSG_CsrsFile.Attributes.ssg_recipient, party.PartyId);
            //    fields.Add(SSG_CsrsFile.Attributes.ssg_partyenrolled, SSG_CsrsFile.PartyEnrolledIsRecipient);

            //    if (otherParty is not null)
            //    {
            //        _logger.LogDebug("Setting other party as payor");
            //        fields.Add(SSG_CsrsFile.Attributes.ssg_payor, otherParty.PartyId);
            //    }
            //}
            //else if (file.UsersRole == PartyRole.Payor)
            //{
            //    _logger.LogDebug("Setting party as payor");
            //    fields.Add(SSG_CsrsFile.Attributes.ssg_payor, party.PartyId);
            //    fields.Add(SSG_CsrsFile.Attributes.ssg_partyenrolled, SSG_CsrsFile.PartyEnrolledIsPayor);

            //    if (otherParty is not null)
            //    {
            //        _logger.LogDebug("Setting other party as recipient");
            //        fields.Add(SSG_CsrsFile.Attributes.ssg_recipient, otherParty.PartyId);
            //    }
            //}
            //else
            //{
            //    using var scope = _logger.AddProperty("PartyRole", file.UsersRole);
            //    _logger.LogInformation("Party role is not in allowable values, expected Recipient or Payor");
            //}


            //var csrsFile = await _fileRepository.InsertAsync(fields, cancellationToken);
            //using var fileScope = _logger.AddFileId(csrsFile.Key);

            //if (file.Children is not null)
            //{
            //    foreach (var child in file.Children)
            //    {
            //        var childFields = _childInsertOrUpdateFieldMapper.GetFieldsForInsert(child);
            //        // add link to child
            //        childFields.Add(SSG_CsrsChild.Attributes.ssg_filenumber, csrsFile.CsrsFileId);

            //        await _childRepository.InsertAsync(childFields, cancellationToken);
            //    }
            //}
            //else
            //{
            //    _logger.LogInformation("New file contains no children");
            //}

            //file = _mapper.Map<File>(csrsFile);

            //return file;
            return null;
        }
    }
}
