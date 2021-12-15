using Csrs.Api.Features.Documents;
using Csrs.Api.Models;
using MediatR;
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="regardingId">The id of ...</param>
        /// <returns></returns>
        [HttpGet("ListDocuments")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<IList<Document>> ListDocumentsAsync([Required]Guid? regardingId)
        {
            ListDocuments.Request request = new();
            ListDocuments.Response response = await _mediator.Send(request);

            return Array.Empty<Document>();
        }

        [HttpGet("DownloadDocument")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<IList<Document>> DownloadDocumentAsync([Required]Guid? fileId)
        {
            DownloadDocument.Request request = new();
            DownloadDocument.Response response = await _mediator.Send(request);

            return Array.Empty<Document>();
        }

        [HttpPost("UploadDocuments")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<IList<Document>> UploadDocumentsAsync(Document document)
        {
            UploadDocuments.Request request = new();
            UploadDocuments.Response response = await _mediator.Send(request);

            return Array.Empty<Document>();
        }
    }
}
