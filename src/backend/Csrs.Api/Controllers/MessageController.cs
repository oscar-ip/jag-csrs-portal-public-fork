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

        public MessageController(IMediator mediator, ILogger<MessageController> logger)
            : base(mediator, logger)
        {
        }

        [HttpGet("List")]
        [ProducesResponseType(typeof(IList<Message>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetAsync(CancellationToken cancellationToken)
        {

            List.Request request = new List.Request();
            List.Response? response = await _mediator.Send(request, cancellationToken);

            return response != List.Response.Empty ? Ok(response.Messages) : NotFound();

        }

        [HttpGet("ListOutbox")]
        [ProducesResponseType(typeof(IList<Message>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetOutboxAsync(CancellationToken cancellationToken)
        {

            ListOutbox.Request request = new ListOutbox.Request();
            ListOutbox.Response? response = await _mediator.Send(request, cancellationToken);

            return response != ListOutbox.Response.Empty ? Ok(response.Messages) : NotFound();

        }

        [HttpGet("Read")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<IActionResult> ReadAsync([Required] string messageGuid)
        {
            Read.Request request = new(messageGuid);
            Read.Response response = await _mediator.Send(request);

            return Ok("Updated");
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
