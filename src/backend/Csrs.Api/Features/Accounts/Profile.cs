using MediatR;
using Csrs.Api.Models;
using System.Security.Claims;
using Csrs.Api.Services;

namespace Csrs.Api.Features.Accounts
{
    public static class Profile
    {
        public class Request : IRequest<Response>
        {
            public Request(ClaimsPrincipal user)
            {
                User = user ?? throw new ArgumentNullException(nameof(user));
            }

            public ClaimsPrincipal User { get; init; }
        }

        public class Response
        {
            public static Response Empty = new Response();

            private Response()
            {
                AccountFileFileSummary = null;
            }

            public Response(AccountFileSummary accountFileFileSummary)
            {
                ArgumentNullException.ThrowIfNull(accountFileFileSummary);
                AccountFileFileSummary = accountFileFileSummary;
            }

            public AccountFileSummary? AccountFileFileSummary { get; init; }
        }

        public class Handler : IRequestHandler<Request, Response>
        {
            private readonly IAccountService _accountService;
            private readonly IFileService _fileService;
            private readonly ILogger<Handler> _logger;

            public Handler(IAccountService accountService,
                IFileService fileService,
                ILogger<Handler> logger)
            {
                _accountService = accountService ?? throw new ArgumentNullException(nameof(accountService));
                _fileService = fileService ?? throw new ArgumentNullException(nameof(fileService));
                _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            }

            public async Task<Response> Handle(Request request, CancellationToken cancellationToken)
            {
                Guid? userId = request.User.GetBCeIDUserId();
                if (userId is null)
                {
                    // no bceid value
                    return Response.Empty;
                }

                Party? accountParty = await _accountService.GetPartyByBCeIdAsync(userId.Value, cancellationToken);

                if (accountParty == null)
                {
                    return Response.Empty;
                }

                AccountFileSummary summary = new AccountFileSummary();
                summary.User = accountParty;
                summary.Files = await _fileService.GetPartyFileSummariesAsync(accountParty.PartyId, cancellationToken);

                return new Response(summary);
            }
        }
    }
}
