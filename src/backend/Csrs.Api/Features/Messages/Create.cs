using MediatR;
//UNUSED CODE/CLASS
//Messages are only created by staff in Dynamics
namespace Csrs.Api.Features.Messages
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

            public Task<Response> Handle(Request request, CancellationToken cancellationToken)
            {
                return Task.FromResult(new Response());
            }
        }

    }
}