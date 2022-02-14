using Csrs.Api.Models;
using Csrs.Api.Services;
using Csrs.Interfaces.Dynamics;
using MediatR;

namespace Csrs.Api.Features.Messages
{
    public static class List
    {
        public class Request : IRequest<Response>
        {
        }
        public class Response
        {
            public static Response Empty = new Response();

            private Response()
            {
                Messages = null;
            }

            public Response(IList<Message> messages)
            {
                ArgumentNullException.ThrowIfNull(messages);
                Messages = messages;
            }

            public IList<Message>? Messages { get; init; }
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

                string userId = _userService.GetBCeIDUserId();


                if (userId == string.Empty)
                {
                    // no bceid value
                    return Response.Empty;
                }

                Party? accountParty = await _accountService.GetPartyByBCeIdAsync(userId, cancellationToken);

                if (accountParty == null)
                {
                    return Response.Empty;
                }

                IList<Message> messages = await _messageService.GetPartyMessages(accountParty.PartyId, cancellationToken);

                return new Response(messages);
            }
        }
    }
}