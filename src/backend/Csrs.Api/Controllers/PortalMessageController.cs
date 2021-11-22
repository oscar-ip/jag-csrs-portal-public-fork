using Csrs.Api.Features.PortalMessages;
using Csrs.Api.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Net;

namespace Csrs.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class PortalMessageController : CsrsControllerBase<PortalMessageController>
    {
        public PortalMessageController(IMediator mediator, ILogger<PortalMessageController> logger)
            : base(mediator, logger)
        {
        }

        [HttpGet("List")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<IList<PortalMessage>> GetAsync([Required]string partyGuid)
        {
            List.Request request = new();
            List.Response response = await _mediator.Send(request);

            return Array.Empty<PortalMessage>();
        }

        [HttpGet("Read")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<IList<PortalMessage>> ReadAsync([Required] string messageGuid)
        {
            Read.Request request = new();
            Read.Response response = await _mediator.Send(request);

            return Array.Empty<PortalMessage>();
        }

        [HttpPost("Create")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<string> CreateAsync([Required] PortalMessage message)
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
