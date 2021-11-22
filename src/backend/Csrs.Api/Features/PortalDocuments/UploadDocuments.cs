using MediatR;
using Csrs.Api.Models;
using Csrs.Api.Repositories;

namespace Csrs.Api.Features.PortalDocuments
{
    public static class UploadDocuments
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