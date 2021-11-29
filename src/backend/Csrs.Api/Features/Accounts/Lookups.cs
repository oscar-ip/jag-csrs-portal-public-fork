using Csrs.Api.Models;
using Csrs.Api.Repositories;
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
            Referral
        }

        public class Request : IRequest<Response>
        {
            public static readonly Request Gender = new(LookupType.Gender);
            public static readonly Request Province = new(LookupType.Province);
            public static readonly Request Identity = new(LookupType.Identity);
            public static readonly Request Referral = new(LookupType.Referral);

            public LookupType Type { get; init; }

            private Request(LookupType type)
            {
                Type = type;
            }
        }

        public class Response
        {
            public IList<OptionValue> Items { get; init; }

            public Response(IList<OptionValue> items)
            {
                ArgumentNullException.ThrowIfNull(items);
                Items = items;
            }
        }
        public class Handler : IRequestHandler<Request, Response>
        {
            private readonly ICsrsPartyRepository _repository;
            private readonly ILogger<Handler> _logger;

            public Handler(ICsrsPartyRepository repository, ILogger<Handler> logger)
            {
                _repository = repository ?? throw new ArgumentNullException(nameof(repository));
                _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            }

            public async Task<Response> Handle(Request request, CancellationToken cancellationToken)
            {
                IList<OptionValue> items = Array.Empty<OptionValue>();

                switch (request.Type)
                {
                    case LookupType.Gender:
                        items = await _repository.GetGenderPicklistAsync(cancellationToken);
                        break;
                    case LookupType.Referral:
                        items = await _repository.GetReferralPicklistAsync(cancellationToken);
                        break;
                    case LookupType.Province:
                        items = await _repository.GetProvincePicklistAsync(cancellationToken);
                        break;
                    case LookupType.Identity:
                        items = await _repository.GetIdentityPicklistAsync(cancellationToken);
                        break;
                    default:
                        break;
                }

                return new Response(items);
            }
        }
    }
}
