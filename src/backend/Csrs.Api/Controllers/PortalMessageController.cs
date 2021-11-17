using Csrs.Api.Models;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Net;

namespace Csrs.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class PortalMessageController : ControllerBase
    {
        [HttpGet("List")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public Task<IList<PortalMessage>> GetAsync([Required]string partyGuid)
        {
            IList<PortalMessage> messages = Array.Empty<PortalMessage>();
            return Task.FromResult(messages);
        }

        [HttpGet("Read")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public Task<IList<PortalMessage>> ReadAsync([Required] string messageGuid)
        {
            IList<PortalMessage> messages = Array.Empty<PortalMessage>();
            return Task.FromResult(messages);
        }

        [HttpPost("Create")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public Task<string> CreateAsync([Required] PortalMessage message)
        {
            return Task.FromResult(string.Empty);
        }

        [HttpDelete("Delete")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public Task<string> DeleteAsync([Required] string messageGuid)
        {
            return Task.FromResult(string.Empty);
        }
    }
}
