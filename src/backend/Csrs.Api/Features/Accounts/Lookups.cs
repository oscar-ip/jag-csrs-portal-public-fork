using Csrs.Api.Models;
using Csrs.Api.Repositories;
using Csrs.Api.Services;
using MediatR;

namespace Csrs.Api.Features.Accounts
{
    public static class Lookups
    {
        public enum LookupType
        {
            Unknown = 0,
            Gender,
            Province,
            Identity,
            Referral,
            PreferredContactMethods
        }

        public class Request : IRequest<Response>
        {
            public static readonly Request Gender = new(LookupType.Gender);
            public static readonly Request Province = new(LookupType.Province);
            public static readonly Request Identity = new(LookupType.Identity);
            public static readonly Request Referral = new(LookupType.Referral);
            public static readonly Request PreferredContactMethods = new(LookupType.PreferredContactMethods);

            public LookupType Type { get; init; }

            private Request(LookupType type)
            {
                Type = type;
            }
        }

        public class Response
        {
            public IList<LookupValue> Items { get; init; }

            public Response(IList<LookupValue> items)
            {
                ArgumentNullException.ThrowIfNull(items);
                Items = items;
            }
        }
        public class Handler : IRequestHandler<Request, Response>
        {
            private readonly IAccountService _service;
            private readonly ILogger<Handler> _logger;

            public Handler(IAccountService service, ILogger<Handler> logger)
            {
                _service = service ?? throw new ArgumentNullException(nameof(service));
                _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            }

            public async Task<Response> Handle(Request request, CancellationToken cancellationToken)
            {
                IList<LookupValue> items = Array.Empty<LookupValue>();

                switch (request.Type)
                {
                    case LookupType.Gender:
                        items = await _service.GetGendersAsync(cancellationToken);
                        break;
                    case LookupType.Referral:
                        items = await _service.GetReferralsAsync(cancellationToken);
                        break;
                    case LookupType.Province:
                        items = await _service.GetProvincesAsync(cancellationToken);
                        break;
                    case LookupType.Identity:
                        items = await _service.GetIdentitiesAsync(cancellationToken);
                        break;
                    case LookupType.PreferredContactMethods:
                        items = await _service.GetPreferredContactMethodsAsync(cancellationToken);
                        break;
                    default:
                        _logger.LogInformation("Unexpected lookup type {LookupType}", request.Type);
                        break;
                }

                return new Response(items);
            }
        }
    }
}
