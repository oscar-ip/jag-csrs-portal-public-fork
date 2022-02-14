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
                _logger.LogDebug("Checking current user for BCeID Guid");

                string userId = _userService.GetBCeIDUserId();
                //string userId = "44e88bfd-fed0-4a68-97f7-0f701df51315";
                if (string.IsNullOrEmpty(userId))
                {
                    _logger.LogInformation("No BCeID on authenticated user, cannot User Request");
                    throw new HttpOperationException("Unauthenticated");
                }
                var bceidScope = _logger.AddBCeIdGuid(userId);
                // find to see if the person has an account already?
                MicrosoftDynamicsCRMssgCsrspartyCollection parties = await _dynamicsClient.GetPartyByBCeIdAsync(userId, cancellationToken);
                MicrosoftDynamicsCRMssgCsrsparty party;
                if (parties.Value.Count > 0)
                {
                    party = parties.Value.First();
                }
                else
                {
                    _logger.LogInformation("No associated party, cannot create User Request");
                    throw new HttpOperationException("No Party Associated");
                }
                _logger.LogInformation("Creating User Request");
                MicrosoftDynamicsCRMtask task = new MicrosoftDynamicsCRMtask();
                task.Activitytypecode = "task";
                task.Subject = "File " + request.FileNo + " - " + request.RequestType;
                string desc = "Party: " + party.SsgBceidDisplayname + "\n"+
                              "Message: "+request.RequestMessage;
                task.Description =  desc;
                task.Isregularactivity = true;
                //Get the file
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
                    //This OWNERID relationship IS DIFFERENT AND DOES USE select systemusers by ownerid
                    //SystemUser is just a child of Principal
                    //This was verified by dynamics team and does not work correct if you principals
                    //As per Burak/Dynmiacs team we should never touch principals. 
                    task.OwnerIdODataBind = _dynamicsClient.GetEntityURI("systemusers", originFile._owneridValue);
                    task.RegardingobjectidSsgCsrsfileODataBind = _dynamicsClient.GetEntityURI("ssg_csrsfiles", originFile.SsgCsrsfileid);
                }
                task.Prioritycode = 1;
                task.Statuscode = 2;
                //ap.Statecode = 0;  defaults in DB
                MicrosoftDynamicsCRMtask result = await _dynamicsClient.Tasks.CreateAsync(task);
                _logger.LogDebug("User Request created successfully");
                return new Response("User Request Created");
            }
        }

    }
}