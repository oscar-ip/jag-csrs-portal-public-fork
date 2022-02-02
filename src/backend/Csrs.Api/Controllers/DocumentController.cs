using Csrs.Interfaces.Dynamics;
using Csrs.Services.FileManager;
using Google.Protobuf;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Net;
using static Csrs.Services.FileManager.FileManager;
using static Csrs.Interfaces.Dynamics.Extensions.EntityDocumentExtensions;
using Csrs.Api.Extensions;
using Csrs.Interfaces.Dynamics.Models;

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

        [HttpGet("DownloadAttachment")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<IActionResult> DownloadAttachment([Required] string fileId, [Required] string entityName, [Required] string fileName, [Required] string documentType)
        {
            return await DownloadAttachmentInternal(fileId, entityName, fileName, documentType, true).ConfigureAwait(true);
        }

        //TODO: This logic belong in a service
        /// <summary>
        /// Internal implementation of download attachment T
        /// </summary>
        /// <param name="entityId"></param>
        /// <param name="entityName"></param>
        /// <param name="fileName"></param>
        /// <param name="documentType"></param>
        /// <param name="checkUser"></param>
        /// <returns></returns>
        private async Task<IActionResult> DownloadAttachmentInternal(string entityId, string entityName, string fileName, string documentType, bool checkUser)
        {
            // get the file.
            if (string.IsNullOrEmpty(fileName) || string.IsNullOrEmpty(documentType) || string.IsNullOrEmpty(entityId) || string.IsNullOrEmpty(entityName)) return BadRequest();

            var hasAccess = true;
            //if (checkUser)
            //{
            //    ValidateSession();
            //    hasAccess = await CanAccessEntityFile(entityName, entityId, documentType, serverRelativeUrl).ConfigureAwait(true);
            //}

            if (!hasAccess) return BadRequest();

            //var logUrl = WordSanitizer.Sanitize(serverRelativeUrl);


            MicrosoftDynamicsCRMssgCsrsfile dynamicsFile = await _dynamicsClient.Ssgcsrsfiles.GetByKeyAsync(entityId, null, null);

            // call the web service
            var downloadRequest = new DownloadFileRequest
            {
                ServerRelativeUrl = dynamicsFile.GetDocumentFolderName() + "\\" + fileName
            };

            var downloadResult = _fileManagerClient.DownloadFile(downloadRequest);

            if (downloadResult.ResultStatus == ResultStatus.Success)
            {
                var fileContents = downloadResult.Data.ToByteArray();
                return new FileContentResult(fileContents, "application/octet-stream");
            }

            //Download result failed
            return NotFound();

        }

        [HttpPost("UploadAttachment")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<IActionResult> UploadAttachmentAsync([Required] Guid fileId, [Required] string entityName, [Required] IFormFile file, [Required] string type)
        {
            //ListApplications.Request request = new();
            //ListApplications.Response response = await _mediator.Send(request);

            
            return await UploadAttachmentInternal(fileId.ToString(), entityName, file, type, true);

        }
        //TODO: This logic belong in a service
        private async Task<IActionResult> UploadAttachmentInternal(string entityId, string entityName,
            IFormFile file, string documentType, bool checkUser)
        {
            FileSystemItem result = null;

            if (string.IsNullOrEmpty(entityId) || string.IsNullOrEmpty(entityName) || string.IsNullOrEmpty(documentType)) return BadRequest();

            var hasAccess = true;
            //if (checkUser)
            //{
            //    ValidateSession();
            //    hasAccess = await CanAccessEntity(entityName, entityId, null).ConfigureAwait(true);
            //}

            if (!hasAccess) return new NotFoundResult();

            var ms = new MemoryStream();
            file.OpenReadStream().CopyTo(ms);
            var data = ms.ToArray();

            // Check for a bad file type.

            //var mimeTypes = new MimeTypes();

            //var mimeType = mimeTypes.GetMimeType(data);

            // Add additional allowed mime types here
            //if (mimeType == null || !(mimeType.Name.Equals("image/png") || mimeType.Name.Equals("image/jpeg") ||
            //                         mimeType.Name.Equals("application/pdf")))
            //{
            //    _logger.LogError($"ERROR in uploading file due to invalid mime type {mimeType?.Name}");
            //    return new NotFoundResult();
            //}
            //else
            //{
                // Sanitize file name
                //var illegalInFileName = new Regex(@"[#%*<>?{}~¿""]");
                //var fileName = illegalInFileName.Replace(file.FileName, "");
                //illegalInFileName = new Regex(@"[&:/\\|]");
                //fileName = illegalInFileName.Replace(fileName, "-");

            string fileName = FileSystemItemExtensions.CombineNameDocumentType(file.FileName, documentType);

            MicrosoftDynamicsCRMssgCsrsfile dynamicsFile = await _dynamicsClient.Ssgcsrsfiles.GetByKeyAsync(entityId, null, null);
            //For Testing purposes only   
            if (dynamicsFile is null) {
                //return BadRequest("File can't be uploaded to no existent file");
                //Create the file if it doesn't exist. This will be deleted.
                dynamicsFile = new MicrosoftDynamicsCRMssgCsrsfile();
                dynamicsFile.SsgCsrsfileid = CleanGuidForSharePoint(entityId);

                dynamicsFile = await _dynamicsClient.Ssgcsrsfiles.CreateAsync(dynamicsFile);
            }

            var folderName = dynamicsFile.GetDocumentFolderName();

            //_dynamicsClient.CreateEntitySharePointDocumentLocation(entityName, entityId, folderName, folderName);

            // call the web service
            var uploadRequest = new UploadFileRequest
            {
                ContentType = file.ContentType,
                Data = ByteString.CopyFrom(data),
                EntityName = entityName,
                FileName = fileName,
                FolderName = folderName
            };

            var uploadResult = _fileManagerClient.UploadFile(uploadRequest);

            if (uploadResult.ResultStatus == ResultStatus.Success)
            {
                // Update modifiedon to current time
                //UpdateEntityModifiedOnDate(entityName, entityId, true);
                _logger.LogInformation("Success");
            }
            else
            {
                _logger.LogError(uploadResult.ResultStatus.ToString());
            }
        //}

            return new JsonResult(result);
        }
    }
}
