using Csrs.Api.Features.Files;
using Csrs.Api.Models;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Net;

namespace Csrs.Api.Controllers
{
    public class FileController : CsrsControllerBase<FileController>
    {
        public FileController(IMediator mediator, ILogger<FileController> logger)
            : base(mediator, logger)
        {
        }

        [HttpPost("ListApplications")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<IList<FileToList>> ListApplicationsAsync([Required] string partyGuid)
        {
            ListApplications.Request request = new();
            ListApplications.Response response = await _mediator.Send(request);

            return Array.Empty<FileToList>();
        }

        [HttpGet("ApplicationDetail")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public Task<IList<FileToCreate>> GetApplicationDetailsAsync([Required] string fileGuid)
        {
            //ApplicationDetail.Request request = new();
            //ApplicationDetail.Response response = await _mediator.Send(request);
            IList<FileToCreate> results = Array.Empty<FileToCreate>();
            return Task.FromResult(results);
        }

        [HttpPost("CreateApplication")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<IList<FileToCreate>> CreateAsync([Required] string partyGuid)
        {
            CreateApplication.Request request = new();
            CreateApplication.Response response = await _mediator.Send(request);

            return Array.Empty<FileToCreate>();
        }

        [HttpPatch("UpdateApplication")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<IList<FileToCreate>> UpdateAsync([Required] string fileGuid)
        {
            UpdateApplication.Request request = new();
            UpdateApplication.Response response = await _mediator.Send(request);

            return Array.Empty<FileToCreate>();
        }

        [AllowAnonymous]
        [HttpPost("UploadAttachment")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<IActionResult> UploadAttachmentAsync([Required] Guid fileId,[Required] IFormFile file,[Required] string type)
        {
            //ListApplications.Request request = new();
            //ListApplications.Response response = await _mediator.Send(request);

            return Ok();
        }

    }
}
