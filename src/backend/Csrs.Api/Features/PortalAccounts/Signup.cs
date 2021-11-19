using AutoMapper;
using Csrs.Api.Models;
using Csrs.Api.Models.Dynamics;
using Csrs.Api.Repositories;
using MediatR;

namespace Csrs.Api.Features.PortalAccounts
{
    public static class Signup
    {
        public class Request : IRequest<Response>
        {
            public Request(PortalAccount account)
            {
                Account = account ?? throw new ArgumentNullException(nameof(account));
            }

            public PortalAccount Account { get; init; }
        }

        public class Response
        {
            public Response(Guid id)
            {
                if (id == Guid.Empty)
                {
                    throw new ArgumentException("id cannot be empty", nameof(id));
                }

                Id = id;
            }

            public Guid Id { get; init; }
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
                SSG_CsrsParty entity = _mapper.Map<SSG_CsrsParty>(request.Account);

                entity = await _repository.InsertAsync(entity, cancellationToken);

                return new Response(entity.Id);
            }
        }
    }
}
