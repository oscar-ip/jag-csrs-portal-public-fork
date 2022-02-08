using Csrs.Api.Features.Messages;
using Csrs.Api.Models;
using Csrs.Api.Services;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Net;

namespace Csrs.Api.Controllers
{
    public class MessageController : CsrsControllerBase<MessageController>
    {

        private readonly IMessageService _messageService;

        public MessageController(IMessageService messageService, IMediator mediator, ILogger<MessageController> logger)
            : base(mediator, logger)
        {
            _messageService = messageService ?? throw new ArgumentNullException(nameof(messageService));
        }

        [HttpGet("List")]
        [ProducesResponseType(typeof(IList<Message>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetAsync(CancellationToken cancellationToken)
        {

            IList<Message> messages = await _messageService.GetPartyMessages(cancellationToken);

            if (messages is not null && messages.Count > 0) 
            {
                return Ok(messages);
            }
            else
            {
                return NotFound();
            }
            
        }

        [HttpGet("Read")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<IList<Message>> ReadAsync([Required] string messageGuid)
        {
            Read.Request request = new();
            Read.Response response = await _mediator.Send(request);

            return Array.Empty<Message>();
        }

        [HttpPost("Create")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<string> CreateAsync([Required] Message message)
        {
            Create.Request request = new();
            Create.Response response = await _mediator.Send(request);

            return string.Empty;
        }

        [HttpDelete("Delete")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<string> DeleteAsync([Required] string messageGuid)
        {
            Delete.Request request = new();
            Delete.Response response = await _mediator.Send(request);

            return string.Empty;
        }
    }
}
