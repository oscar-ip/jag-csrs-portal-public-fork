using Csrs.Api.Features.Documents;
using Csrs.Api.Models;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Net;

namespace Csrs.Api.Controllers
{
    public class DocumentController : CsrsControllerBase<DocumentController>
    {
        public DocumentController(IMediator mediator, ILogger<DocumentController> logger)
            : base(mediator, logger)
        {
        }

        [AllowAnonymous]
        [HttpPost("UploadAttachment")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<IActionResult> UploadAttachmentAsync([Required] Guid fileId, string entityName, [Required] IFormFile file, [Required] string type)
        {
            //ListApplications.Request request = new();
            //ListApplications.Response response = await _mediator.Send(request);

            return Ok();
        }

    }
}
