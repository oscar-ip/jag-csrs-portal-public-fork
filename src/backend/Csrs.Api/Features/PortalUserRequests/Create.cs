using MediatR;
using Csrs.Api.Models;
using Csrs.Api.Repositories;

namespace Csrs.Api.Features.PortalUserRequests
{
    public static class Create
    {
        public class Request : IRequest<Response>
        {
        }
        public class Response
        {
        }
        public class Handler : IRequestHandler<Request, Response>
        {
            private readonly ILogger<Handler> _logger;

            public Handler(ILogger<Handler> logger)
            {
                _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            }

            public async Task<Response> Handle(Request request, CancellationToken cancellationToken)
            {
                return new Response();
            }
        }

    }
}