using Csrs.Api.Features.PortalFiles;
using Csrs.Api.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Net;

namespace Csrs.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class PortalFileController : CsrsControllerBase<PortalFileController>
    {
        public PortalFileController(IMediator mediator, ILogger<PortalFileController> logger)
            : base(mediator, logger)
        {
        }

        [HttpPost("ListApplications")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<IList<PortalFileToList>> ListApplicationsAsync([Required] string partyGuid)
        {
            ListApplications.Request request = new();
            ListApplications.Response response = await _mediator.Send(request);

            return Array.Empty<PortalFileToList>();
        }

        [HttpGet("ApplicationDetail")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<IList<PortalFileToCreate>> GetApplicationDetailsAsync([Required] string fileGuid)
        {
            //ApplicationDetail.Request request = new();
            //ApplicationDetail.Response response = await _mediator.Send(request);

            return Array.Empty<PortalFileToCreate>();
        }

        [HttpPost("CreateApplication")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<IList<PortalFileToCreate>> CreateAsync([Required] string partyGuid)
        {
            CreateApplication.Request request = new();
            CreateApplication.Response response = await _mediator.Send(request);

            return Array.Empty<PortalFileToCreate>();
        }

        [HttpPatch("UpdateApplication")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<IList<PortalFileToCreate>> UpdateAsync([Required] string fileGuid)
        {
            UpdateApplication.Request request = new();
            UpdateApplication.Response response = await _mediator.Send(request);

            return Array.Empty<PortalFileToCreate>();
        }
    }
}
