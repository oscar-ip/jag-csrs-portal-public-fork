using Csrs.Api.Features.PortalDocuments;
using Csrs.Api.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Net;

namespace Csrs.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class PortalDocumentController : CsrsControllerBase<PortalDocumentController>
    {
        public PortalDocumentController(IMediator mediator, ILogger<PortalDocumentController> logger)
            : base(mediator, logger)
        {
        }

        [HttpGet("ListDocuments")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<IList<PortalDocument>> ListDocumentsAsync([Required] string regardingGuid)
        {
            ListDocuments.Request request = new();
            ListDocuments.Response response = await _mediator.Send(request);

            return Array.Empty<PortalDocument>();
        }

        [HttpGet("DownloadDocument")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<IList<PortalDocument>> DownloadDocumentAsync([Required]string fileGuid)
        {
            DownloadDocument.Request request = new();
            DownloadDocument.Response response = await _mediator.Send(request);

            return Array.Empty<PortalDocument>();
        }

        [HttpPost("UploadDocuments")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<IList<PortalDocument>> UploadDocumentsAsync(PortalDocument document)
        {
            UploadDocuments.Request request = new();
            UploadDocuments.Response response = await _mediator.Send(request);

            return Array.Empty<PortalDocument>();
        }
    }
}
