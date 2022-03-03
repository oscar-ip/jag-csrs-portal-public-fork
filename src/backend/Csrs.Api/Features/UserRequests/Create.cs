using Csrs.Api.Services;
using Csrs.Interfaces.Dynamics;
using Csrs.Interfaces.Dynamics.Models;
using MediatR;
using Microsoft.Rest;
using System.Net;

namespace Csrs.Api.Features.UserRequests
{
    public static class Create
    {
        public class Request : IRequest<Response>
        {
            public Request(string fileId, string fileNo, string requestType, string requestMessage)
            {
                FileId = fileId;
                FileNo = fileNo;
                RequestType = requestType;
                RequestMessage = requestMessage;
            }
            public string FileId { get; init; }
            public string FileNo { get; init; }
            public string RequestType { get; init; }
            public string RequestMessage { get; init; }
        }
        public class Response
        {
            public Response(string responseMessage)
            {
                ResponseMessage = responseMessage;
            }

            public string ResponseMessage { get; init; }
        }
        public class Handler : IRequestHandler<Request, Response>
        {
            private readonly IDynamicsClient _dynamicsClient;
            private readonly IUserService _userService;
            private readonly IAccountService _accountService;
            private readonly IFileService _fileService;
            private readonly ILogger<Handler> _logger;
            private readonly ITaskService _taskService;

            public Handler(
                IDynamicsClient dynamicsClient,
                IUserService userService,
                IAccountService accountService,
                IFileService fileService,
                ILogger<Handler> logger,
                ITaskService taskService )
            {
                _dynamicsClient = dynamicsClient ?? throw new ArgumentNullException(nameof(dynamicsClient));
                _userService = userService ?? throw new ArgumentNullException(nameof(userService));
                _accountService = accountService ?? throw new ArgumentNullException(nameof(accountService));
                _fileService = fileService ?? throw new ArgumentNullException(nameof(fileService));
                _logger = logger ?? throw new ArgumentNullException(nameof(logger));
                _taskService = taskService ?? throw new ArgumentNullException(nameof(taskService));
            }

            public async Task<Response> Handle(Request request, CancellationToken cancellationToken)
            {
                
                _logger.LogInformation("Creating User Request");
                string userId = _userService.GetBCeIDUserId();
                if (string.IsNullOrEmpty(userId))
                {
                    _logger.LogInformation("No BCeID on authenticated user, cannot create User Request");
                    return new Response("Unauthenticated");
                }
                _logger.AddBCeIdGuid(userId);
                MicrosoftDynamicsCRMssgCsrspartyCollection parties = await _dynamicsClient.GetPartyByBCeIdAsync(userId, cancellationToken);
                MicrosoftDynamicsCRMssgCsrsparty party;
                if (parties.Value.Count > 0)
                {
                    party = parties.Value.First();
                }
                else
                {
                    _logger.LogInformation("No associated party, cannot create User Request");
                    return new Response("No Party Associated");
                }
                _logger.AddPartyId(party.SsgCsrspartyid);
                MicrosoftDynamicsCRMtask task = new MicrosoftDynamicsCRMtask();
                task.Activitytypecode = "task";
                task.Subject = "File " + request.FileNo + " - " + request.RequestType;
                string desc = "Party: " + party.SsgFullname + "\n"+
                              "Message: "+request.RequestMessage;
                task.Description =  desc;
                task.Isregularactivity = true;
                //Get the file
                _logger.AddFileId(request.FileId);
                MicrosoftDynamicsCRMssgCsrsfile originFile;
                try
                {
                    originFile = await _dynamicsClient.GetFileByFileId(request.FileId, cancellationToken);
                }
                catch (HttpOperationException exception) when (exception.Response?.StatusCode == HttpStatusCode.NotFound)
                {
                    _logger.LogInformation("No file associated, cannot create User Request");
                    return new Response("Incorrect file number supplied");
                }
                if (originFile != null)
                {
                    if (originFile._owninguserValue != null)
                    {
                        task.OwninguserTaskODataBind = _dynamicsClient.GetEntityURI("systemusers", originFile._owninguserValue);
                    }
                    else if (originFile._owningteamValue != null)
                    {
                        task.OwningteamTaskODataBind = _dynamicsClient.GetEntityURI("teams", originFile._owningteamValue);
                    }
                    else
                    {
                        _logger.LogInformation("File has no owner, cannot create User Request");
                        return new Response("File has no owner");
                    }
                    task.RegardingobjectidSsgCsrsfileODataBind = _dynamicsClient.GetEntityURI("ssg_csrsfiles", originFile.SsgCsrsfileid);
                }
                task.Prioritycode = 1;// Normal
                task.Statuscode = 2; // Not Started
                task.Scheduledend = new DateTimeOffset(DateTime.UtcNow);
                //ap.Statecode = 0;  defaults in DB
                MicrosoftDynamicsCRMtask result = await _dynamicsClient.Tasks.CreateAsync(task);
                string subject = "Contact Us Record Created";
                string description = "User created a contact request for file: " + request.FileNo;
                await _taskService.CreateTask(request.FileId, subject, description, cancellationToken);
                _logger.AddProperty("ActivityId", result.Activityid);
                _logger.LogDebug("User Request created successfully");
                return new Response("User Request Created");
            }
        }

    }
}