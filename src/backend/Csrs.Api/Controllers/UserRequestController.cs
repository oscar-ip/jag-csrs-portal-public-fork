using Csrs.Api.Features.UserRequests;
using Csrs.Api.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Net;

namespace Csrs.Api.Controllers
{
    public class UserRequestController : CsrsControllerBase<UserRequestController>
    {
        public UserRequestController(IMediator mediator, ILogger<UserRequestController> logger)
            : base(mediator, logger)
        {
        }

        [HttpPost("Create")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<IActionResult> CreateAsync([Required] UserRequest userRequest, CancellationToken cancellationToken)
        {
            if (userRequest == null || userRequest.FileNo == null || userRequest.RequestType == null || userRequest.RequestMessage == null)
            {
                return BadRequest();
            }
            Create.Request request = new(userRequest.FileNo, userRequest.RequestType, userRequest.RequestMessage);
            Create.Response? response = await _mediator.Send(request, cancellationToken);
            return Ok(response);
        }
    }
}
