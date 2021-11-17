using MediatR;
using Csrs.Api.Models;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

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
            private readonly ILogger<Handler> _logger;

            public Handler(ILogger<Handler> logger)
            {
                _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            }

            public Task<Response> Handle(Request request, CancellationToken cancellationToken)
            {
                return Task.FromResult(new Response(Array.Empty<PortalAccount>()));
            }
        }
    }
}
