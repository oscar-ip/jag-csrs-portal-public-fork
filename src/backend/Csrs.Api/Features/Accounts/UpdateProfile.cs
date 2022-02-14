using AutoMapper;
using MediatR;
using Csrs.Api.Models;
using Csrs.Api.Services;

namespace Csrs.Api.Features.Accounts
{
    public static class UpdateProfile
    {
        public class Request : IRequest<Response>
        {
            public Request(Party account)
            {
                Account = account ?? throw new ArgumentNullException(nameof(account));
            }

            public Party Account { get; init; }
        }

        public class Response
        {
            public Response(Party account)
            {
                Account = account ?? throw new ArgumentNullException(nameof(account));
            }

            public Party Account { get; init; }
        }

        public class Handler : IRequestHandler<Request, Response>
        {
            private readonly IAccountService _accountService;
            private readonly IMapper _mapper;
            private readonly ILogger<Handler> _logger;

            public Handler(IAccountService accountService, IMapper mapper, ILogger<Handler> logger)
            {
                _accountService = accountService ?? throw new ArgumentNullException(nameof(accountService));
                _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
                _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            }

            public async Task<Response> Handle(Request request, CancellationToken cancellationToken)
            {
                var account = await _accountService.CreateOrUpdateAsync(request.Account, cancellationToken);
                return new Response(account);
            }
        }
    }
}
