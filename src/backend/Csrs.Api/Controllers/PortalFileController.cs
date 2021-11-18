using Csrs.Api.Models;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Net;

namespace Csrs.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class PortalFileController : ControllerBase
    {
        [HttpPost("ListApplications")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public Task<IList<PortalFileToList>> ListApplicationsAsync([Required] string partyGuid)
        {
            IList<PortalFileToList> files = Array.Empty<PortalFileToList>();
            return Task.FromResult(files);
        }

        [HttpGet("ApplicationDetail")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public Task<IList<PortalFileToCreate>> GetApplicationDetailsAsync([Required] string fileGuid)
        {
            IList<PortalFileToCreate> files = Array.Empty<PortalFileToCreate>();
            return Task.FromResult(files);
        }

        [HttpPost("CreateApplication")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public Task<IList<PortalFileToCreate>> CreateAsync([Required] string partyGuid)
        {
            IList<PortalFileToCreate> files = Array.Empty<PortalFileToCreate>();
            return Task.FromResult(files);
        }

        [HttpPatch("UpdateApplication")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public Task<IList<PortalFileToCreate>> UpdateAsync([Required] string fileGuid)
        {
            IList<PortalFileToCreate> files = Array.Empty<PortalFileToCreate>();
            return Task.FromResult(files);
        }
    }
}
