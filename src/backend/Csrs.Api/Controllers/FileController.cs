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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fileId">The file id for the file trying to access</param>
        /// <param name="password">The party password</param>
        /// <returns></returns>
        /// <response code="200">The file id and password were correct.</response>
        /// <response code="400">Either the file id or password were not supplied, empty or whitespace.</response>
        /// <response code="404">The respondent file with matching file id and password was not found.</response>
        /// <response code="500">There was a server error that prevented the search from completing successfully.</response>
        [HttpGet("AuthorizeRespondent")]
        [ProducesResponseType(typeof(RespondentResult), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> AuthorizeRespondentAsync([Required] string fileId, [Required] string password)
        {
            AuthorizeRespondent.Request request = new AuthorizeRespondent.Request(fileId, password);
            AuthorizeRespondent.Response response = await _mediator.Send(request);

            if (response == AuthorizeRespondent.Response.BadRequest)
            {
                return BadRequest();
            }

            if (response == AuthorizeRespondent.Response.NotFound)
            {
                return NotFound();
            }

            return Ok(new RespondentResult { FileId = response.FileId, PartyId = response.PartyId });
        }

        // TODO: move this to the models folder
        public class RespondentResult
        {
            public Guid PartyId { get; set; }
            public Guid FileId { get; set; }
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
