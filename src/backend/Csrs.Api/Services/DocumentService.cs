using Csrs.Api.Extensions;
using Csrs.Interfaces.Dynamics;
using Csrs.Interfaces.Dynamics.Extensions;
using Csrs.Interfaces.Dynamics.Models;
using static Csrs.Interfaces.Dynamics.Extensions.EntityDocumentExtensions;
using Csrs.Services.FileManager;
using Google.Protobuf;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Rest;
using static Csrs.Services.FileManager.FileManager;

namespace Csrs.Api.Services
{
    public class DocumentService : IDocumentService
    {

        private readonly IDynamicsClient _dynamicsClient;
        private readonly ILogger<MessageService> _logger;
        private readonly FileManagerClient _fileManagerClient;

        public DocumentService(
            IDynamicsClient dynamicsClient,
            ILogger<MessageService> logger,
            FileManagerClient fileManagerClient)
        {
            _dynamicsClient = dynamicsClient ?? throw new ArgumentNullException(nameof(dynamicsClient));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _fileManagerClient = fileManagerClient ?? throw new ArgumentNullException(nameof(fileManagerClient));
        }
        public async Task<IActionResult> DownloadAttachment(string entityId, string entityName, string serverRelativeUrl, string documentType, CancellationToken cancellationToken)
        {
            // get the file.
            if (string.IsNullOrEmpty(serverRelativeUrl) || string.IsNullOrEmpty(documentType) || string.IsNullOrEmpty(entityId) || string.IsNullOrEmpty(entityName)) return new BadRequestResult();

            var hasAccess = true;
            //if (checkUser)
            //{
            //    ValidateSession();
            //    hasAccess = await CanAccessDocument(entityName, entityId).ConfigureAwait(true);
            //}

            if (!hasAccess) return new UnauthorizedResult();

            MicrosoftDynamicsCRMssgCsrsfile dynamicsFile = await _dynamicsClient.Ssgcsrsfiles.GetByKeyAsync(entityId, null, null, cancellationToken);

            if (dynamicsFile is null) return new NotFoundResult();

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
            return new NotFoundResult();
        }

        public async Task<IList<FileSystemItem>> GetAttachmentList(string entityId, string entityName, string documentType, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(entityId) || string.IsNullOrEmpty(entityName) || string.IsNullOrEmpty(documentType)) return new List<FileSystemItem>();

            var hasAccess = true;

            //if (checkUser)
            //{
            //    ValidateSession();
            //    hasAccess = await CanAccessEntity(entityName, entityId, null);
            //}

            if (!hasAccess) return new List<FileSystemItem>();

            return await GetListFilesInFolder(entityId, entityName, documentType, cancellationToken);
        }

        public async Task<IActionResult> UploadAttachment(string entityId, string entityName, IFormFile file, string type, CancellationToken cancellationToken)
        {
            string result = "";

            if (string.IsNullOrEmpty(entityId) || string.IsNullOrEmpty(entityName) || string.IsNullOrEmpty(type)) return new BadRequestResult();

            var hasAccess = true;
            //if (checkUser)
            //{
            //    hasAccess = await CanAccessDocument(entityName, entityId).ConfigureAwait(true);
            //}

            if (!hasAccess) return new UnauthorizedResult();

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

            string fileName = FileSystemItemExtensions.CombineNameDocumentType(file.FileName, type);

            MicrosoftDynamicsCRMssgCsrsfile dynamicsFile = await _dynamicsClient.Ssgcsrsfiles.GetByKeyAsync(entityId, null, null, cancellationToken);
            //For Testing purposes only   
            if (dynamicsFile is null)
            {
                return new BadRequestResult();
            }

            var folderName = dynamicsFile.GetDocumentFolderName();

            await CreateAccountDocumentLocation(dynamicsFile, folderName, cancellationToken);

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
                result = "Uploaded Successfully";
            }
            else
            {
                _logger.LogError(uploadResult.ResultStatus.ToString());

                result = uploadResult.ErrorDetail;
            }
            //}

            return new JsonResult(result);
        }
        /// <summary>
        /// Return the list of files in a given folder.
        /// </summary>
        /// <param name="entityId"></param>
        /// <param name="entityName"></param>
        /// <param name="documentType"></param>
        /// <returns></returns>
        private async Task<List<FileSystemItem>> GetListFilesInFolder(string entityId, string entityName, string documentType, CancellationToken cancellationToken)
        {
            var fileSystemItemVMList = new List<FileSystemItem>();


            if (string.IsNullOrEmpty(entityId) || string.IsNullOrEmpty(entityName) || string.IsNullOrEmpty(documentType)) return fileSystemItemVMList;
            //Three retries? Why only here?
            for (int i = 0; i < 3; i++)
            {
                try
                {
                    MicrosoftDynamicsCRMssgCsrsfile dynamicsFile = await _dynamicsClient.Ssgcsrsfiles.GetByKeyAsync(entityId, null, null, cancellationToken);
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
        private async Task<bool> CanAccessDocument(string entityId, string entityName, string BCeIDUserId, CancellationToken cancellationToken)
        {

            if (String.IsNullOrEmpty(BCeIDUserId)) return false;

            MicrosoftDynamicsCRMssgCsrspartyCollection partiesCollection = await _dynamicsClient.GetPartyByBCeIdAsync(BCeIDUserId, cancellationToken);

            if (partiesCollection is null || partiesCollection.Value.Count == 0) return false;

            MicrosoftDynamicsCRMssgCsrsparty parties = partiesCollection.Value.FirstOrDefault();

            var actual = await _dynamicsClient.GetFilesByParty(parties.SsgCsrspartyid, cancellationToken);

            if (actual is null) return false;

            if (actual.Value.Count == 0 || !actual.Value.ToList().Exists(file => file.SsgCsrsfileid.Equals(entityId))) return false;

            return true;

        }

        private async Task CreateAccountDocumentLocation(MicrosoftDynamicsCRMssgCsrsfile dynamicsFile, string folderName, CancellationToken cancellationToken)
        {

            // Create the SharePointDocumentLocation entity
            var mdcsdl = new MicrosoftDynamicsCRMsharepointdocumentlocation
            {
                RegardingobjectidSsgCsrsfile = dynamicsFile,
                Relativeurl = folderName,
                Description = "ssg_csrsfile",
                Name = folderName
            };

            var sharepointdocumentlocationid = await DocumentLocationExistsWithCleanup(mdcsdl, cancellationToken);

            if (sharepointdocumentlocationid is null)
            {
                try
                {
                    mdcsdl = await _dynamicsClient.Sharepointdocumentlocations.CreateAsync(mdcsdl, null, cancellationToken);
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

        private async Task<string> DocumentLocationExistsWithCleanup(MicrosoftDynamicsCRMsharepointdocumentlocation mdcsdl, CancellationToken cancellationToken)
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

                        _logger.LogError($"Orphan Sharepointdocumentlocation found. ID is {location.Sharepointdocumentlocationid}");
                        // it is an invalid document location. cleanup.
                        try
                        {
                            //_dynamicsClient.Sharepointdocumentlocations.Delete(location.Sharepointdocumentlocationid);
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