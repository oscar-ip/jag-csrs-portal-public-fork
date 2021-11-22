using MediatR;
using Csrs.Api.Models;
using Csrs.Api.Repositories;

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
            public Response(IEnumerable<PortalAccount> accounts)
            {
                if (accounts is null)
                {
                    throw new ArgumentNullException(nameof(accounts));
                }

                Accounts = new List<PortalAccount>(accounts);
            }

            public IList<PortalAccount> Accounts { get; init; }
        }

        public class Handler : IRequestHandler<Request, Response>
        {
            private readonly ICsrsFileRepository _repository;
            private readonly ILogger<Handler> _logger;

            public Handler(ICsrsFileRepository repository, ILogger<Handler> logger)
            {
                _repository = repository ?? throw new ArgumentNullException(nameof(repository));
                _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            }

            public async Task<Response> Handle(Request request, CancellationToken cancellationToken)
            {
                var items = await _repository.GetAsync(request.BCeIDGuid, cancellationToken);
                return new Response(Array.Empty<PortalAccount>());
            }
        }
    }
}
