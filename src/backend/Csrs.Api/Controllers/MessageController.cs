using Csrs.Api.Features.Messages;
using Csrs.Api.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Net;

namespace Csrs.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class MessageController : CsrsControllerBase<MessageController>
    {
        public MessageController(IMediator mediator, ILogger<MessageController> logger)
            : base(mediator, logger)
        {
        }

        [HttpGet("List")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<IList<Message>> GetAsync([Required]string partyGuid)
        {
            List.Request request = new();
            List.Response response = await _mediator.Send(request);

            return Array.Empty<Message>();
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
