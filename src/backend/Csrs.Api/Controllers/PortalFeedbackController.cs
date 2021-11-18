using Csrs.Api.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Csrs.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class PortalFeedbackController : ControllerBase
    {
        [HttpPost("Create")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public Task<string> CreateAsync(PortalFeedback feedback)
        {
            return Task.FromResult(string.Empty);
        }
    }
}
