using MediatR;
using Csrs.Api.Models;
using Csrs.Api.Repositories;
using Csrs.Api.Models.Dynamics;
using AutoMapper;

namespace Csrs.Api.Features.PortalAccounts
{
    public static class Profile
    {
        public class Request : IRequest<Response>
        {
            //[FromQuery(Name = "bceIDGuid")]
            //[Required]
            public Guid BCeIDGuid { get; set; }
        }

        public class Response
        {
            public Response()
            {
                Accounts = Array.Empty<PortalAccount>();
            }

            public Response(PortalAccount account)
            {
                if (account is null)
                {
                    throw new ArgumentNullException(nameof(account));
                }

                Accounts = new List<PortalAccount>() { account };
            }

            public IList<PortalAccount> Accounts { get; init; }
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
                var item = await _repository.GetAsync(request.BCeIDGuid, SSG_CsrsParty.AllProperties, cancellationToken);
                if (item is null)
                {
                    return new Response();
                }

                PortalAccount account = _mapper.Map<PortalAccount>(item);
                return new Response(account);
            }
        }
    }
}
