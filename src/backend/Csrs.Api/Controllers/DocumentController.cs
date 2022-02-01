using Csrs.Api.Features.Documents;
using Csrs.Api.Models;
using Csrs.Interfaces.Dynamics;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Net;
using static Csrs.Services.FileManager.FileManager;

namespace Csrs.Api.Controllers
{
    public class DocumentController : CsrsControllerBase<DocumentController>
    {

        private readonly IDynamicsClient _dynamicsClient;
        private readonly FileManagerClient _fileManagerClient;

        public DocumentController(IMediator mediator, 
            ILogger<DocumentController> logger,
            IDynamicsClient dynamicsClient,
            FileManagerClient fileManagerClient)
            : base(mediator, logger)
        {
            _dynamicsClient = dynamicsClient;
            _fileManagerClient = fileManagerClient;
        }


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
