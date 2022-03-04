using Csrs.Api.Models;
using Csrs.Api.Repositories;
using Csrs.Api.Services;
using Csrs.Interfaces.Dynamics;
using Csrs.Interfaces.Dynamics.Models;
using MediatR;
using CSRSAccountFile = Csrs.Api.Models.CSRSAccountFile;

namespace Csrs.Api.Features.Accounts
{
    public static class UpdateCSRSAccount
    {
        public class Request : IRequest<Response>
        {
            public Request(Party csrsAccountUser, CSRSAccountFile csrsAccountFile)
            {
                CsrsAccountUser = csrsAccountUser ?? throw new ArgumentNullException(nameof(csrsAccountUser));
                CSRSAccountFile = csrsAccountFile ?? throw new ArgumentNullException(nameof(csrsAccountFile));
            }

            public Party CsrsAccountUser { get; init; }
            public CSRSAccountFile CSRSAccountFile { get; init; }
        }

        public class Response
        {
            public Response(string partyId, string fileId)
            {
                PartyId = partyId;
                FileId = fileId;
            }

            public string PartyId { get; init; }
            public string FileId { get; init; }
        }

        public class Handler : IRequestHandler<Request, Response>
        {
            private readonly IDynamicsClient _dynamicsClient;
            private readonly IUserService _userService;
            private readonly IAccountService _accountService;
            private readonly IFileService _fileService;

            private readonly ILogger<Handler> _logger;

            public Handler(
                IDynamicsClient dynamicsClient,
                IUserService userService,
                IAccountService accountService,
                IFileService fileService,
                ILogger<Handler> logger)
            {
                _dynamicsClient = dynamicsClient ?? throw new ArgumentNullException(nameof(dynamicsClient));
                _userService = userService ?? throw new ArgumentNullException(nameof(userService));
                _accountService = accountService ?? throw new ArgumentNullException(nameof(accountService));
                _fileService = fileService ?? throw new ArgumentNullException(nameof(fileService));
                _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            }

            public async Task<Response> Handle(Request request, CancellationToken cancellationToken)
            {
                _logger.LogDebug("Update CSRS account user for BCeID Guid");

                string userId = _userService.GetBCeIDUserId();
                if (string.IsNullOrEmpty(userId))
                {
                    _logger.LogInformation("No BCeID on authenticated user, cannot update account");
                    return new Response(string.Empty, string.Empty);
                }

                var bceidScope = _logger.AddBCeIdGuid(userId);

                var partyId = request.CsrsAccountUser.PartyId;
                var dynamicsParty = request.CsrsAccountUser.ToDynamicsModel();
                dynamicsParty.SsgBceidGuid = userId;
                dynamicsParty.SsgBceidLastUpdate = DateTimeOffset.Now;

                try
                {
                    await _dynamicsClient.Ssgcsrsparties.UpdateAsync(partyId, dynamicsParty, cancellationToken);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "An exception occurred while updating {PartyId},  CSRS account file update will be aborted", partyId);
                    throw;
                }

                // update the CSRS Account file
                var fileId = await _fileService.UpdateCSRSAccountFile(partyId, request.CSRSAccountFile, cancellationToken);

                _logger.LogDebug("Party and file were updated successfully");

                MicrosoftDynamicsCRMssgCsrsfile file = await _dynamicsClient.GetFileByFileId(fileId, cancellationToken);
                MicrosoftDynamicsCRMtask task = new MicrosoftDynamicsCRMtask();
                task.Activitytypecode = "task";
                var parties = await _dynamicsClient.GetPartyByBCeIdAsync(userId, cancellationToken);
                var fullName = parties?.Value[0].SsgFullname;

                task.Subject = $"File {file.SsgFilenumber} - Account Setup Submitted";
                task.Description = $"Respondent Application submitted, please review.\nParty: {fullName}"; 
                task.Isregularactivity = true;

                var owninguser = file._owninguserValue;
                if (owninguser is not null)
                {
                    task.OwninguserODataBind = _dynamicsClient.GetEntityURI("systemusers", owninguser);
                    task.OwnerIdODataBind = _dynamicsClient.GetEntityURI("systemusers", owninguser);
                }
                task.RegardingobjectidSsgCsrsfileODataBind = _dynamicsClient.GetEntityURI("ssg_csrsfiles", fileId);

                task.Prioritycode = 1;// Normal
                task.Statuscode = 2; // Not Started
                task.Scheduledend = new DateTimeOffset(DateTime.UtcNow);

                try
                {
                    MicrosoftDynamicsCRMtask result = await _dynamicsClient.Tasks.CreateAsync(task);
                } 
                catch (Exception ex)
                {
                    _logger.LogError(ex, "An exception occurred while creating task with partyId = {PartyId} and fileid = {FileId},  CSRS account file update will be aborted", partyId, fileId);
                    throw;
                }

                return new Response(partyId, request.CSRSAccountFile.FileId);
            }
        }
    }
}
