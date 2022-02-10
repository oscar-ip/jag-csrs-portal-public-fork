using Csrs.Api.Models;
using Csrs.Api.Services;
using Csrs.Interfaces.Dynamics;
using MediatR;

namespace Csrs.Api.Features.Messages
{
    public static class Read
    {
        public class Request : IRequest<Response>
        {
            public Request(string messageId)
            {
                MessageId = messageId ?? throw new ArgumentNullException(nameof(messageId));
            }

            public string MessageId { get; init; }
        }
        public class Response
        {
            public Response()
            {
            }

        }
        public class Handler : IRequestHandler<Request, Response>
        {
            private readonly IDynamicsClient _dynamicsClient;
            private readonly IUserService _userService;
            private readonly IMessageService _messageService;
            private readonly IAccountService _accountService;
            private readonly ILogger<Handler> _logger;

            public Handler(
                IDynamicsClient dynamicsClient,
                IUserService userService,
                IAccountService accountService,
                IMessageService messageService,
                ILogger<Handler> logger)
            {
                _dynamicsClient = dynamicsClient ?? throw new ArgumentNullException(nameof(dynamicsClient));
                _userService = userService;
                _accountService = accountService ?? throw new ArgumentNullException(nameof(accountService));
                _messageService = messageService ?? throw new ArgumentNullException(nameof(messageService));
                _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            }

            public async Task<Response> Handle(Request request, CancellationToken cancellationToken)
            {

                await _messageService.SetMessageRead(request.MessageId, cancellationToken);

                return new Response();

            }
        }
    }
}
