using Csrs.Api.Features.PortalFeedbacks;
using Csrs.Api.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Csrs.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class PortalFeedbackController : CsrsControllerBase<PortalFeedbackController>
    {
        public PortalFeedbackController(IMediator mediator, ILogger<PortalFeedbackController> logger)
            : base(mediator, logger)
        {
        }

        [HttpPost("Create")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<string> CreateAsync(PortalFeedback feedback)
        {
            Create.Request request = new();
            Create.Response? response = await _mediator.Send(request);

            return string.Empty;
        }
    }
}
