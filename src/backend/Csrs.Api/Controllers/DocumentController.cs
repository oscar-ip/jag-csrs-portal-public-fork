using Csrs.Services.FileManager;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Net;
using Csrs.Api.Features.Documents;

namespace Csrs.Api.Controllers
{
    public class DocumentController : CsrsControllerBase<DocumentController>
    {
        public DocumentController(IMediator mediator,
            ILogger<DocumentController> logger)
            : base(mediator, logger)
        {
        }

        [HttpGet("DownloadAttachment")]
        [ProducesResponseType((int)HttpStatusCode.OK),
         ProducesResponseType((int)HttpStatusCode.Unauthorized),
         ProducesResponseType((int)HttpStatusCode.NotFound),
         ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> DownloadAttachment([Required] string entityId, [Required] string entityName, [Required] string serverRelativeUrl, [Required] string documentType)
        {
            DownloadDocument.Request request = new DownloadDocument.Request(entityId, entityName, serverRelativeUrl, documentType);
            DownloadDocument.Response response = await _mediator.Send(request);

            return response.ActionResult;
        }

        [HttpPost("UploadAttachment")]
        [ProducesResponseType((int)HttpStatusCode.OK),
         ProducesResponseType((int)HttpStatusCode.Unauthorized),
         ProducesResponseType((int)HttpStatusCode.NotFound),
         ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> UploadAttachmentAsync([Required] string entityId, [Required] string entityName, [Required] IFormFile file, [Required] string type)
        {

            UploadDocuments.Request request = new UploadDocuments.Request(entityId, entityName, file, type);
            UploadDocuments.Response response = await _mediator.Send(request);

            return response.ActionResult;

        }

        /// <summary>
        /// Get the file details list in folder associated to the application folder and document type
        /// </summary>
        /// <param name="entityId"></param>
        /// <param name="documentType"></param>
        /// <returns></returns>
        [HttpGet("GetAttachmentList")]
        public async Task<IList<FileSystemItem>> GetAttachmentList([Required] string entityId, [Required] string entityName, [Required] string documentType)
        {

            ListDocuments.Request request = new ListDocuments.Request(entityId, entityName, documentType);
            ListDocuments.Response response = await _mediator.Send(request);

            return response.Attachments;
        }

    }

}
