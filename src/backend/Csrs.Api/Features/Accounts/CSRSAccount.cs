using MediatR;
using Csrs.Api.Models;
using System.Security.Claims;
using Csrs.Api.Services;
using Csrs.Interfaces.Dynamics;
using Csrs.Api.Repositories;
using CSRSAccount = Csrs.Api.Models.CSRSAccount;

namespace Csrs.Api.Features.Accounts
{
    public static class CheckCSRSAccount
    {
        public class Request : IRequest<Response>
        {
            public Request(ClaimsPrincipal user, CSRSAccount csrsaccount)
            {
                User = user ?? throw new ArgumentNullException(nameof(user));
                CsrsAccount = csrsaccount ?? throw new ArgumentNullException(nameof(csrsaccount));
            }

            public ClaimsPrincipal User { get; init; }
            public CSRSAccount CsrsAccount { get; init; }

        }

        public class Response
        {
            public static Response Empty = new Response();

            private Response()
            {
                CSRSPartyFileIds = null;
            }

            public Response(CSRSPartyFileIds csrsIds)
            {
                ArgumentNullException.ThrowIfNull(csrsIds);
                CSRSPartyFileIds = csrsIds;
            }

            public CSRSPartyFileIds CSRSPartyFileIds { get; init; }
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

                CSRSPartyFileIds result = 
                    await _accountService.GetPartyFileIdsByBCeIdAndCSRSAccountAsync(userId, request.CsrsAccount, cancellationToken);

                return new Response(result);
            }
        }
    }
}
