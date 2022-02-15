using MediatR;
using Csrs.Api.Models;
using System.Security.Claims;
using Csrs.Api.Services;
using Csrs.Interfaces.Dynamics;
using Csrs.Api.Repositories;

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
            private readonly IDynamicsClient _dynamicsClient;
            private readonly IUserService _userService;
            private readonly IAccountService _accountService;
            private readonly ILogger<Handler> _logger;

            public Handler(
                IDynamicsClient dynamicsClient,
                IUserService userService,
                IAccountService accountService,
                ILogger<Handler> logger)
            {
                _dynamicsClient = dynamicsClient ?? throw new ArgumentNullException(nameof(dynamicsClient));
                _userService = userService;
                _accountService = accountService ?? throw new ArgumentNullException(nameof(accountService));
                _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            }

            public async Task<Response> Handle(Request request, CancellationToken cancellationToken)
            {
                string userId = _userService.GetBCeIDUserId();
                if (userId == string.Empty)
                {
                    // no bceid value
                    return Response.Empty;
                }

                Party? accountParty = await _accountService.GetPartyByBCeIdAsync(userId, cancellationToken);

                if (accountParty == null)
                {
                    return Response.Empty;
                }

                AccountFileSummary summary = new AccountFileSummary();
                summary.User = accountParty;
                summary.Files = await _dynamicsClient.GetFileSummaryByPartyAsync(accountParty.PartyId, cancellationToken);

                return new Response(summary);
            }
        }
    }
}
