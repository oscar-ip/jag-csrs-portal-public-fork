using Csrs.Api.Features.Feedbacks;
using Csrs.Api.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Csrs.Api.Controllers
{
    public class FeedbackController : CsrsControllerBase<FeedbackController>
    {
        public FeedbackController(IMediator mediator, ILogger<FeedbackController> logger)
            : base(mediator, logger)
        {
        }

        [HttpPost("Create")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<string> CreateAsync(Feedback feedback)
        {
            Create.Request request = new();
            Create.Response? response = await _mediator.Send(request);

            return string.Empty;
        }
    }
}
