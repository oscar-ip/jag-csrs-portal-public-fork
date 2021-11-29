using MediatR;
using Csrs.Api.Models;
using Csrs.Api.Repositories;
using Csrs.Api.Models.Dynamics;
using AutoMapper;
using System.Security.Claims;

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
                Account = null;
            }

            public Response(PortalAccount account)
            {
                ArgumentNullException.ThrowIfNull(account);
                Account = account;
            }

            public PortalAccount? Account { get; init; }
        }

        public class Handler : IRequestHandler<Request, Response>
        {
            private readonly ICsrsPartyRepository _repository;
            private readonly IMapper _mapper;
            private readonly ILogger<Handler> _logger;

            public Handler(ICsrsPartyRepository repository, IMapper mapper, ILogger<Handler> logger)
            {
                _repository = repository ?? throw new ArgumentNullException(nameof(repository));
                _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
                _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            }

            public async Task<Response> Handle(Request request, CancellationToken cancellationToken)
            {
                Guid? userId = request.User.GetBCeIDUserId();
                if (userId == null)
                {
                    return Response.Empty;
                }

                var item = await _repository.GetAsync(userId.Value, SSG_CsrsParty.AllProperties, cancellationToken);
                if (item is null)
                {
                    return Response.Empty;
                }

                PortalAccount account = _mapper.Map<PortalAccount>(item);
                return new Response(account);
            }
        }
    }
}
