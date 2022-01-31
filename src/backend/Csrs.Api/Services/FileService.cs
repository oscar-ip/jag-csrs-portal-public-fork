using AutoMapper;
using Csrs.Api.Models;
using Csrs.Api.Repositories;
using Csrs.Interfaces.Dynamics;
using Csrs.Interfaces.Dynamics.Models;
using Microsoft.Extensions.Caching.Memory;
using File = Csrs.Api.Models.File;

namespace Csrs.Api.Services
{
    public class FileService : IFileService
    {
        private readonly IMemoryCache _cache;
        private readonly IDynamicsClient _dynamicsClient;
        private readonly IMapper _mapper;
        private readonly ILogger<FileService> _logger;

        public FileService(
            IMemoryCache cache,
            IDynamicsClient dynamicsClient,
            IMapper mapper,
            ILogger<FileService> logger)
        {
            _cache = cache ?? throw new ArgumentNullException(nameof(cache));
            _dynamicsClient = dynamicsClient ?? throw new ArgumentNullException(nameof(dynamicsClient));
            _mapper = mapper;
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        // Task<PicklistOptionSetMetadata> GetPicklistOptionSetMetadataAsync(string entityName, string attributeName, CancellationToken cancellationToken);

        public async Task<Tuple<string,string>> CreateFile(string partyId, string? otherPartyId, File file, CancellationToken cancellationToken)
        {
            var partyEnrolled = _dynamicsClient.GetPicklistOptionSetMetadataAsync("", "ssg_partyenrolled", _cache, cancellationToken);

            MicrosoftDynamicsCRMssgCsrsfile csrsFile = file.ToDynamicsModel();

            //// map the party and other party to recipient and payor
            if (file.UsersRole == PartyRole.Recipient)
            {
                csrsFile._ssgRecipientValue = partyId;
                csrsFile.SsgPartyenrolled = 0; // recipient

                if (otherPartyId is not null)
                {
                    csrsFile._ssgPayorValue = otherPartyId;
                }
            }
            else if (file.UsersRole == PartyRole.Payor)
            {
                csrsFile._ssgPayorValue = partyId;
                csrsFile.SsgPartyenrolled = 0; // payor

                if (otherPartyId is not null)
                {
                    csrsFile._ssgRecipientValue = otherPartyId;
                }
            }

            csrsFile = await _dynamicsClient.Ssgcsrsfiles.CreateAsync(csrsFile, cancellationToken: cancellationToken);

            if (file.Children is not null)
            {
                foreach (var child in file.Children)
                {
                    var csrsChild = child.ToDynamicsModel();
                    csrsChild._ssgFilenumberValue = csrsFile.SsgCsrsfileid;
                    csrsChild = await _dynamicsClient.Ssgcsrschildren.CreateAsync(csrsChild, cancellationToken: cancellationToken);
                }
            }

            return Tuple.Create(csrsFile.SsgCsrsfileid, csrsFile.SsgFilenumber);
        }
    }
}
