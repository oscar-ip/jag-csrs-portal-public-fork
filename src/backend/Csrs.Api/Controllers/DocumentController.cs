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
using Csrs.Api.Services;
using Csrs.Api.Repositories;
using Microsoft.Rest;

namespace Csrs.Api.Controllers
{
    public class DocumentController : CsrsControllerBase<DocumentController>
    {

        private readonly IDynamicsClient _dynamicsClient;
        private readonly IUserService _userService;
        private readonly FileManagerClient _fileManagerClient;

        public DocumentController(IMediator mediator,
            ILogger<DocumentController> logger,
            IDynamicsClient dynamicsClient,
            IUserService userService,
            FileManagerClient fileManagerClient)
            : base(mediator, logger)
        {
            _dynamicsClient = dynamicsClient;
            _userService = userService;
            _fileManagerClient = fileManagerClient;
        }

        [HttpGet("DownloadAttachment")]
        [ProducesResponseType((int)HttpStatusCode.OK),
         ProducesResponseType((int)HttpStatusCode.Unauthorized),
         ProducesResponseType((int)HttpStatusCode.NotFound),
         ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> DownloadAttachment([Required] string fileId, [Required] string entityName, [Required] string serverRelativeUrl, [Required] string documentType)
        {
            return await DownloadAttachmentInternal(fileId, entityName, serverRelativeUrl, documentType, true).ConfigureAwait(true);
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
        private async Task<IActionResult> DownloadAttachmentInternal(string entityId, string entityName, string serverRelativeUrl, string documentType, bool checkUser)
        {
            // get the file.
            if (string.IsNullOrEmpty(serverRelativeUrl) || string.IsNullOrEmpty(documentType) || string.IsNullOrEmpty(entityId) || string.IsNullOrEmpty(entityName)) return BadRequest();

            var hasAccess = true;
            //if (checkUser)
            //{
            //    ValidateSession();
            //    hasAccess = await CanAccessDocument(entityName, entityId).ConfigureAwait(true);
            //}

            if (!hasAccess) return Unauthorized();

            //var logUrl = WordSanitizer.Sanitize(serverRelativeUrl);


            MicrosoftDynamicsCRMssgCsrsfile dynamicsFile = await _dynamicsClient.Ssgcsrsfiles.GetByKeyAsync(entityId, null, null);

            // call the web service
            var downloadRequest = new DownloadFileRequest
            {
                ServerRelativeUrl = serverRelativeUrl
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
        [ProducesResponseType((int)HttpStatusCode.OK),
         ProducesResponseType((int)HttpStatusCode.Unauthorized),
         ProducesResponseType((int)HttpStatusCode.NotFound),
         ProducesResponseType((int)HttpStatusCode.BadRequest)]
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
            //    hasAccess = await CanAccessDocument(entityName, entityId).ConfigureAwait(true);
            //}

            if (!hasAccess) return Unauthorized();

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
            if (dynamicsFile is null)
            {
                //return BadRequest("File can't be uploaded to no existent file");
                //Create the file if it doesn't exist. This will be deleted.
                dynamicsFile = new MicrosoftDynamicsCRMssgCsrsfile();
                dynamicsFile.SsgCsrsfileid = CleanGuidForSharePoint(entityId);

                dynamicsFile = await _dynamicsClient.Ssgcsrsfiles.CreateAsync(dynamicsFile);
            }

            var folderName = dynamicsFile.GetDocumentFolderName();

            await CreateAccountDocumentLocation(dynamicsFile, folderName);

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


        /// <summary>
        /// Get the file details list in folder associated to the application folder and document type
        /// </summary>
        /// <param name="entityId"></param>
        /// <param name="documentType"></param>
        /// <returns></returns>
        [HttpGet("GetAttachmentList")]
        public async Task<IActionResult> GetAttachmentList(string entityId, string entityName, string documentType)
        {
            if (string.IsNullOrEmpty(entityId) || string.IsNullOrEmpty(entityName) || string.IsNullOrEmpty(documentType)) return BadRequest();

            var hasAccess = true;

            //if (checkUser)
            //{
            //    ValidateSession();
            //    hasAccess = await CanAccessEntity(entityName, entityId, null);
            //}

            if (!hasAccess) return new NotFoundResult();

            var fileSystemItemVMList = await GetListFilesInFolder(entityId, entityName, documentType);
            return new JsonResult(fileSystemItemVMList);
        }

        /// <summary>
        /// Return the list of files in a given folder.
        /// </summary>
        /// <param name="entityId"></param>
        /// <param name="entityName"></param>
        /// <param name="documentType"></param>
        /// <returns></returns>
        private async Task<List<FileSystemItem>> GetListFilesInFolder(string entityId, string entityName, string documentType)
        {
            var fileSystemItemVMList = new List<FileSystemItem>();


            if (string.IsNullOrEmpty(entityId) || string.IsNullOrEmpty(entityName) || string.IsNullOrEmpty(documentType)) return fileSystemItemVMList;
            //Three retries? Why only here?
            for (int i = 0; i < 3; i++)
            {
                try
                {
                    MicrosoftDynamicsCRMssgCsrsfile dynamicsFile = await _dynamicsClient.Ssgcsrsfiles.GetByKeyAsync(entityId, null, null);
                    // call the web service
                    var request = new FolderFilesRequest
                    {
                        DocumentType = documentType,
                        EntityId = entityId,
                        EntityName = entityName,
                        FolderName = dynamicsFile.GetDocumentFolderName()
                };

                    var result = _fileManagerClient.FolderFiles(request);

                    if (result.ResultStatus == ResultStatus.Success)
                    {

                        // convert the results to the view model.
                        foreach (var fileDetails in result.Files)
                        {
                            var fileSystemItemVM = new FileSystemItem
                            {
                                // remove the document type text from file name
                                Name = fileDetails.Name.Substring(fileDetails.Name.IndexOf("__") + 2),
                                // convert size from bytes (original) to KB
                                Size = fileDetails.Size,
                                ServerRelativeUrl = fileDetails.ServerRelativeUrl,
                                TimeCreated = fileDetails.TimeCreated,
                                TimeLastModified = fileDetails.TimeLastModified,
                                DocumentType = fileDetails.DocumentType
                            };

                            fileSystemItemVMList.Add(fileSystemItemVM);

                        }

                        return fileSystemItemVMList;

                    }

                    _logger.LogError($"ERROR in getting folder files for entity {entityName}, entityId {entityId}, docuemnt type {documentType} ");

                }
                catch (Exception e)
                {
                    _logger.LogError(e, "Error getting SharePoint File List");
                }
            }
            return fileSystemItemVMList;
        }


        /// <summary>
        /// Initial Access Check
        /// </summary>
        /// <param name="entityId">File Id</param>
        /// <param name="entityName">Unused currently</param>
        /// <returns></returns>
        private async Task<bool> CanAccessDocument(string entityId, string entityName)
        {

            if (String.IsNullOrEmpty(_userService.GetBCeIDUserId())) return false;

            MicrosoftDynamicsCRMssgCsrspartyCollection partiesCollection = await _dynamicsClient.GetPartyByBCeIdAsync(_userService.GetBCeIDUserId(), cancellationToken: CancellationToken.None);

            if (partiesCollection is null || partiesCollection.Value.Count == 0) return false;

            List<MicrosoftDynamicsCRMssgCsrsparty> parties = partiesCollection.Value.ToList();

            string filter = String.Format($"_ssg_payor_value eq {0} or _ssg_recipient_value eq {0}", parties[0].SsgCsrspartyid);

            var actual = await _dynamicsClient.Ssgcsrsfiles.GetAsync(top: 10, filter: filter, expand: null, cancellationToken: CancellationToken.None);

            if (actual is null) return false;

            if (actual.Value.Count == 0 || !actual.Value.ToList().Exists(file => file.SsgCsrsfileid.Equals(entityId))) return false;

            return true;

        }

        private async Task CreateAccountDocumentLocation(MicrosoftDynamicsCRMssgCsrsfile dynamicsFile, string folderName)
        {

            // Create the SharePointDocumentLocation entity
            var mdcsdl = new MicrosoftDynamicsCRMsharepointdocumentlocation
            {
                RegardingobjectidSsgCsrsfile = dynamicsFile,
                Relativeurl = folderName,
                Description = "ssg_csrsfile",
                Name = folderName
            };

            var sharepointdocumentlocationid = await DocumentLocationExistsWithCleanup(mdcsdl);

            if (sharepointdocumentlocationid is null)
            {
                try
                {
                    mdcsdl = _dynamicsClient.Sharepointdocumentlocations.Create(mdcsdl);
                }
                catch (HttpOperationException odee)
                {
                    _logger.LogError(odee, "Error creating SharepointDocumentLocation");
                    mdcsdl = null;
                }
                if (mdcsdl != null)
                {
                    try
                    {
                        dynamicsFile.SsgCsrsfileSharePointDocumentLocations.Add(mdcsdl);
                        await _dynamicsClient.Ssgcsrsfiles.UpdateAsync(dynamicsFile.SsgCsrsfileid, dynamicsFile);
                    }
                    catch (HttpOperationException odee)
                    {
                        _logger.LogError(odee, "Error adding reference to SharepointDocumentLocation");
                    }
                }
            }
        }

        private async Task<string> DocumentLocationExistsWithCleanup(MicrosoftDynamicsCRMsharepointdocumentlocation mdcsdl)
        {
            var relativeUrl = mdcsdl.Relativeurl.Replace("'", "''");
            var filter = $"relativeurl eq '{relativeUrl}'";
            // start by getting the existing document locations.
            string result = null;
            try
            {
                var locations =
                   await _dynamicsClient.Sharepointdocumentlocations.GetAsync(filter: filter);

                foreach (var location in locations.Value.ToList())
                    if (string.IsNullOrEmpty(location._regardingobjectidValue))
                    {

                        _logger.LogError($"Orphan Sharepointdocumentlocation found.  ID is {location.Sharepointdocumentlocationid}");
                        // it is an invalid document location. cleanup.
                        try
                        {
                            _dynamicsClient.Sharepointdocumentlocations.Delete(location.Sharepointdocumentlocationid);
                        }
                        catch (HttpOperationException odee)
                        {
                            _logger.LogError("Error caught?");
                        }
                    }
                    else
                    {
                        if (result != null)
                            _logger.LogError($"Duplicate Sharepointdocumentlocation found.  ID is {location.Sharepointdocumentlocationid}");
                        else
                            result = location.Sharepointdocumentlocationid;
                    }
            }
            catch (HttpOperationException odee)
            {
                _logger.LogError(odee, "Error getting SharepointDocumentLocations");
            }

            return result;


        }

    }


}
