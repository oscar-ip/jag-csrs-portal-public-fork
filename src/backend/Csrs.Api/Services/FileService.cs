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

        //public async Task<Tuple<string,string>> CreateFile(string partyId, string? otherPartyId, File file, CancellationToken cancellationToken)
        public async Task<Tuple<string, string>> CreateFile(MicrosoftDynamicsCRMssgCsrsparty party, MicrosoftDynamicsCRMssgCsrsparty otherParty, File file, CancellationToken cancellationToken)
        {
            //var partyEnrolled = _dynamicsClient.GetPicklistOptionSetMetadataAsync("", "ssg_partyenrolled", _cache, cancellationToken);

            //MicrosoftDynamicsCRMssgCsrsfile csrsFile = file.ToDynamicsModel();
            MicrosoftDynamicsCRMssgCsrsfile csrsFile = new MicrosoftDynamicsCRMssgCsrsfile();
            csrsFile.SsgCourtfiletype = PickLists.GetCourtFileTypes("Court Order");
            csrsFile.SsgAct = PickLists.GetCourtFileTypes("Court Order");
            csrsFile.SsgFmepfileactive = true;
            csrsFile.SsgSafetyalert = true;
            csrsFile.SsgSafetyalertpayor = true;
            csrsFile.SsgSafetyconcerndescription = "Safety Concern description";
            csrsFile.SsgSharedparenting = true;
            csrsFile.SsgSplitparentingarrangement = true;


            //csrsFile.SsgSection7expenses = 867670000;
            //csrsFile.SsgSection7payorsamount = 1000.00m;
            //csrsFile.SsgSection7recipientsamount = 400.00m;

            //csrsFile.SsgSection7payorsproportion = 20;

            
            csrsFile.SsgBCCourtLevel = new MicrosoftDynamicsCRMssgCsrsbccourtlevel {SsgCourtlevellabel = "Provincial" };
            //csrsFile.SsgBCCourtLocation = new MicrosoftDynamicsCRMssgIjssbccourtlocation { SsgIjssbccourtlocationid =  "Victoria Court" };



            //// map the party and other party to recipient and payor
            if (file.UsersRole == PartyRole.Recipient)
            {
                //csrsFile._ssgRecipientValue = partyId;
                //csrsFile.SsgPartyenrolled = 0; // recipient
                csrsFile.SsgPartyenrolled = PickLists.GetPartyEnrolled("Recipient");
                //csrsFile.SsgRecipient = party;
                    //new MicrosoftDynamicsCRMssgCsrsparty { SsgCsrspartyid = party.SsgCsrspartyid };

                //csrsFile.SsgRecipient.SsgCsrspartyid = partyId;
                //csrsFile.SsgRecipient = party;

                //if (otherPartyId is not null)
                if (otherParty is not null)
                {
                    //csrsFile._ssgPayorValue = otherPartyId;
                    //csrsFile.SsgPayor = otherParty;
                   // csrsFile.SsgPayor = otherParty;
                        //new MicrosoftDynamicsCRMssgCsrsparty { SsgCsrspartyid = otherParty.SsgCsrspartyid };
                }
            }
            else if (file.UsersRole == PartyRole.Payor)
            {
                //csrsFile._ssgPayorValue = partyId;
                //csrsFile.SsgPartyenrolled = 0; // payor
                csrsFile.SsgPartyenrolled = PickLists.GetPartyEnrolled("Payor");
                //csrsFile.SsgPayor = party;
                //new MicrosoftDynamicsCRMssgCsrsparty { SsgCsrspartyid = party.SsgCsrspartyid };
                // csrsFile.SsgPayor = party;


                //if (otherPartyId is not null)
                if (otherParty is not null)
                {
                    //csrsFile._ssgRecipientValue = otherPartyId;
                    //csrsFile.SsgRecipient = otherParty;
                  //  csrsFile.SsgRecipient = otherParty;
                        //new MicrosoftDynamicsCRMssgCsrsparty { SsgCsrspartyid = otherParty.SsgCsrspartyid };
                }
            }

            csrsFile = await _dynamicsClient.Ssgcsrsfiles.CreateAsync(csrsFile, cancellationToken: cancellationToken);

            if (file.Children is not null)
            {
                foreach (var child in file.Children)
                {
                    var csrsChild = child.ToDynamicsModel();
                    //csrsChild._ssgFilenumberValue = csrsFile.SsgCsrsfileid;
                    csrsChild = await _dynamicsClient.Ssgcsrschildren.CreateAsync(csrsChild, cancellationToken: cancellationToken);
                }
            }

            return Tuple.Create(csrsFile.SsgCsrsfileid, csrsFile.SsgFilenumber);
        }
    }
}
