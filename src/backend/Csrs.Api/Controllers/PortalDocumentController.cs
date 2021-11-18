using Csrs.Api.Models;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Net;

namespace Csrs.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class PortalDocumentController : ControllerBase
    {
        [HttpGet("ListDocuments")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public Task<IList<PortalDocument>> ListDocumentsAsync([Required] string regardingGuid)
        {
            IList<PortalDocument> documents = Array.Empty<PortalDocument>();
            return Task.FromResult(documents);
        }

        [HttpGet("DownloadDocument")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public Task<IList<PortalDocument>> DownloadDocumentAsync([Required]string fileGuid)
        {
            IList<PortalDocument> documents = Array.Empty<PortalDocument>();
            return Task.FromResult(documents);
        }

        [HttpPost("UploadDocuments")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public Task<IList<PortalDocument>> UploadDocumentsAsync(PortalDocument document)
        {
            IList<PortalDocument> documents = Array.Empty<PortalDocument>();
            return Task.FromResult(documents);
        }
    }
}
