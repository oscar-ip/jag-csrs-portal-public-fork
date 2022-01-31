using Csrs.Api.Models;
using Csrs.Api.Repositories;
using Csrs.Api.Services;
using MediatR;
using File = Csrs.Api.Models.File;

namespace Csrs.Api.Features.Accounts
{
    public static class NewAccountAndFile
    {
        public class Request : IRequest<Response>
        {
            public Request(Party applicant, File file)
            {
                Applicant = applicant ?? throw new ArgumentNullException(nameof(applicant));
                File = file ?? throw new ArgumentNullException(nameof(file));
            }

            public Party Applicant { get; init; }
            public File File { get; init; }
        }

        public class Response
        {
            public Response(string id)
            {
                Id = id;
            }

            public string Id { get; init; }
        }

        public class Handler : IRequestHandler<Request, Response>
        {
            private readonly IUserService _userService;
            private readonly IAccountService _accountService;
            private readonly IFileService _fileService;

            private readonly ILogger<Handler> _logger;

            public Handler(
                IUserService userService, 
                IAccountService accountService,
                IFileService fileService, 
                ILogger<Handler> logger)
            {
                _userService = userService ?? throw new ArgumentNullException(nameof(userService));
                _accountService = accountService ?? throw new ArgumentNullException(nameof(accountService));
                _fileService = fileService ?? throw new ArgumentNullException(nameof(fileService));
                _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            }

            public async Task<Response> Handle(Request request, CancellationToken cancellationToken)
            {
                _logger.LogDebug("Checking current user for BCeID Guid");

                string userId = _userService.GetBCeIDUserId();
                if (string.IsNullOrEmpty(userId))
                {
                    _logger.LogInformation("No BCeID on authenticated user, cannot create account");
                    return new Response(String.Empty);
                }

                var bceidScope = _logger.AddBCeIdGuid(userId);

                // find to see if the person has an account already?
                Party? party = await _accountService.GetPartyByBCeIdAsync(userId, cancellationToken);

                if (party is not null)
                {
                    _logger.LogDebug("Party already exists");
                } 
                else
                {
                    // will case party to be created
                    _logger.LogDebug("Party does not exist, create new party");
                    party = new Party();
                    party.BCeIDGuid = userId;
                }

                // create the party
                _logger.LogInformation("Creating party");

                party = await _accountService.CreateOrUpdateAsync(party, cancellationToken);

                // create the other party
                Party? otherParty = null;
                if (request.File.OtherParty != null)
                {
                    _logger.LogInformation("Creating other party");

                    otherParty = await _accountService.CreateOrUpdateAsync(request.File.OtherParty, cancellationToken);
                }
                else
                {
                    _logger.LogInformation("No other party provided");
                }

                // create the file
                var file = await _fileService.CreateFile(party, otherParty, request.File, cancellationToken);

                _logger.LogDebug("Party and file created successfully");
                return new Response(party.PartyId);
            }
        }
    }
}
