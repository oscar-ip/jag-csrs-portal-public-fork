using Csrs.Api.Repositories;
using MediatR;

namespace Csrs.Api.Features.Lookups
{
    public static class CourtLevels
    {
        public class Request : IRequest<Response>
        {
            public static readonly Request Default = new Request();

            private Request()
            {
            }
        }

        public class Response
        {
            public List<Models.Dynamics.SSG_CsrsBCCourtLevel> Items { get; init; }

            public Response(IEnumerable<Models.Dynamics.SSG_CsrsBCCourtLevel> items)
            {
                ArgumentNullException.ThrowIfNull(items);
                Items = new List<Models.Dynamics.SSG_CsrsBCCourtLevel>(items);
            }
        }

        public class Handler : IRequestHandler<Request, Response>
        {
            private readonly ICourtLevelRepository _repository;

            public Handler(ICourtLevelRepository repository)
            {
                _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            }

            public async Task<Response> Handle(Request request, CancellationToken cancellationToken)
            {
                var items = await _repository.GetAllAsync(Models.Dynamics.SSG_CsrsBCCourtLevel.AllProperties, cancellationToken);
                var response = new Response(items);

                return response;
            }
        }
    }
}
