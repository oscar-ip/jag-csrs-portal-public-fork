using Csrs.Api.Repositories;
using MediatR;

namespace Csrs.Api.Features.Lookups
{
    public static class CourtLocations
    {
        public class Request : IRequest<Response>
        {
            public static readonly Request Default = new();

            private Request()
            {
            }
        }

        public class Response
        {
            public List<Models.Dynamics.SSG_IJSSBCCourtlocation> Items { get; init; }

            public Response(IEnumerable<Models.Dynamics.SSG_IJSSBCCourtlocation> items)
            {
                ArgumentNullException.ThrowIfNull(items);
                Items = new List<Models.Dynamics.SSG_IJSSBCCourtlocation>(items);
            }
        }

        public class Handler : IRequestHandler<Request, Response>
        {
            private readonly ICourtLocationRepository _repository;

            public Handler(ICourtLocationRepository repository)
            {
                _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            }

            public async Task<Response> Handle(Request request, CancellationToken cancellationToken)
            {
                var items = await _repository.GetAllAsync(Models.Dynamics.SSG_IJSSBCCourtlocation.AllProperties, cancellationToken);
                var response = new Response(items);

                return response;
            }
        }
    }
}
