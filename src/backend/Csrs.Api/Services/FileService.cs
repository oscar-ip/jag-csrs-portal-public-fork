using AutoMapper;
using Csrs.Api.Models;
using Csrs.Api.Repositories;
using Csrs.Interfaces.Dynamics;
using Csrs.Interfaces.Dynamics.Models;
using Microsoft.Extensions.Caching.Memory;
using File = Csrs.Api.Models.File;
using PickLists = Csrs.Api.Models.PickupLists;

namespace Csrs.Api.Services
{
    public class FileService : IFileService
    {
        private readonly IMemoryCache _cache;
        private readonly IDynamicsClient _dynamicsClient;
        private readonly IMapper _mapper;
        private readonly ILogger<FileService> _logger;
        private readonly int   _male = 867670000;
        private readonly int _female = 867670001;

        
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

        public async Task<Tuple<string, string>> CreateFile(MicrosoftDynamicsCRMssgCsrsparty party, 
            MicrosoftDynamicsCRMssgCsrsparty otherParty, File file, CancellationToken cancellationToken)
        {

            MicrosoftDynamicsCRMssgCsrsfile csrsFile = await file.ToDynamicsModel(_dynamicsClient, cancellationToken);

            // map the party and other party to recipient and payor
            if (file.UsersRole == PartyRole.Recipient)
            {
                csrsFile.SsgPartyenrolled = (int)PartyEnrolled.Recipient;
                csrsFile.SsgRecipientODataBind = _dynamicsClient.GetEntityURI("ssg_csrsfiles", party.SsgCsrspartyid);

                if (otherParty is not null)
                {
                    csrsFile.SsgPayorODataBind = _dynamicsClient.GetEntityURI("ssg_csrsfiles", otherParty.SsgCsrspartyid);
                }
            }
            else if (file.UsersRole == PartyRole.Payor)
            {
                csrsFile.SsgPartyenrolled = (int)PartyEnrolled.Payor;
                csrsFile.SsgPayorODataBind = _dynamicsClient.GetEntityURI("ssg_csrsfiles", party.SsgCsrspartyid);

                if (otherParty is not null)
                {
                    csrsFile.SsgRecipientODataBind = _dynamicsClient.GetEntityURI("ssg_csrsfiles", otherParty.SsgCsrspartyid);
                }
            }

            try
            {
                csrsFile = await _dynamicsClient.Ssgcsrsfiles.CreateAsync(csrsFile, cancellationToken: cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception in FileService.CreateFile");
            }

            if (file.Children is not null)
            {
                foreach (var child in file.Children)
                {
                    var csrsChild = child.ToDynamicsModel();
                    if (party.SsgPartygender == _male)
                    {
                        csrsChild.SsgChildsFatherODataBind = _dynamicsClient.GetEntityURI("ssg_csrsparties", party.SsgCsrspartyid);
                    }
                    else if (party.SsgPartygender == _female)
                    {
                        csrsChild.SsgChildsMotherODataBind = _dynamicsClient.GetEntityURI("ssg_csrsparties", party.SsgCsrspartyid);
                    }

                    if (otherParty is not null && otherParty.SsgPartygender == _male)
                    {
                        csrsChild.SsgChildsFatherODataBind = _dynamicsClient.GetEntityURI("ssg_csrsparties", otherParty.SsgCsrspartyid);
                    }
                    else if (otherParty is not null && otherParty.SsgPartygender == _female)
                    {
                        csrsChild.SsgChildsMotherODataBind = _dynamicsClient.GetEntityURI("ssg_csrsparties", otherParty.SsgCsrspartyid);
                    }

                    csrsChild.SsgFileNumberODataBind = _dynamicsClient.GetEntityURI("ssg_csrsfiles", csrsFile.SsgCsrsfileid);

                    try
                    {
                        csrsChild = await _dynamicsClient.Ssgcsrschildren.CreateAsync(csrsChild, cancellationToken: cancellationToken);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Exception in FileService.CreateChildren");
                    }
                }
            }

            return Tuple.Create(csrsFile.SsgCsrsfileid, csrsFile.SsgFilenumber);
        }
    }
}
