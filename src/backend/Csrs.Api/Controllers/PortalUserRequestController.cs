using Csrs.Api.Models;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Net;

namespace Csrs.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class PortalUserRequestController : ControllerBase
    {
        [HttpPost("Create")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public Task<string> CreateAsync([Required] PortalUserRequest request)
        {
            return Task.FromResult(string.Empty);
        }
    }
}
