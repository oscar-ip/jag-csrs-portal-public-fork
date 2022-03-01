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
using Csrs.Api.Models;

namespace Csrs.Api.Services
{
    public class DocumentService : IDocumentService
    {

        private readonly IDynamicsClient _dynamicsClient;
        private readonly ILogger<DocumentService> _logger;
        private readonly FileManagerClient _fileManagerClient;
        private readonly IUserService _userService;
        private readonly ITaskService _taskService;

        public DocumentService(
            IDynamicsClient dynamicsClient,
            ILogger<DocumentService> logger,
            FileManagerClient fileManagerClient,
            IUserService userService, 
            ITaskService taskService)
        {
            _dynamicsClient = dynamicsClient ?? throw new ArgumentNullException(nameof(dynamicsClient));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _fileManagerClient = fileManagerClient ?? throw new ArgumentNullException(nameof(fileManagerClient));
            _userService = userService ?? throw new ArgumentNullException(nameof(userService));
            _taskService = taskService ?? throw new ArgumentNullException(nameof(taskService));
        }
        public async Task<IActionResult> DownloadAttachment(string entityId, string entityName, string serverRelativeUrl, string documentType, CancellationToken cancellationToken)
        {
            // get the file.
            if (string.IsNullOrEmpty(serverRelativeUrl) || string.IsNullOrEmpty(documentType) || string.IsNullOrEmpty(entityId) || string.IsNullOrEmpty(entityName)) return new BadRequestResult();
            
            var dynamicsFile = await CanAccessDocument(entityId, _userService.GetBCeIDUserId(), cancellationToken);

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

            string folderName;

            switch (entityName.ToLower())
            {
                case "ssg_csrsfile":

                    var dynamicsFile = await CanAccessDocument(entityId, _userService.GetBCeIDUserId(), cancellationToken);

                    if (dynamicsFile is null) return new List<FileSystemItem>();

                    folderName = dynamicsFile.GetDocumentFolderName();

                    break;

                case "ssg_csrscommunicationmessage":

                    var message = await CanAccessMessageDocument(entityId, _userService.GetBCeIDUserId(), cancellationToken);
                    
                    if (message is null) return new List<FileSystemItem>();

                    folderName = message.GetDocumentFolderName();
                    
                    break;

                default: return new List<FileSystemItem>();
            }

            return await GetListFilesInFolder(entityId, entityName, documentType, folderName, cancellationToken);

        }

        public async Task<IActionResult> UploadAttachment(string entityId, string entityName, IFormFile file, string type, CancellationToken cancellationToken)
        {
            UploadResult result = new UploadResult();

            if (string.IsNullOrEmpty(entityId) || string.IsNullOrEmpty(entityName) || string.IsNullOrEmpty(type)) return new BadRequestResult();

            var dynamicsFile = await CanAccessDocument(entityId, _userService.GetBCeIDUserId(), cancellationToken);

            if (dynamicsFile is null) return new NotFoundResult();

            var ms = new MemoryStream();
            file.OpenReadStream().CopyTo(ms);
            var data = ms.ToArray();

            string fileName = FileSystemItemExtensions.CombineNameDocumentType(file.FileName, type);

            var folderName = dynamicsFile.GetDocumentFolderName();

           // await CreateAccountDocumentLocation(dynamicsFile, folderName, cancellationToken);

            // call the web service
            var uploadRequest = new UploadFileRequest
            {
                ContentType = file.ContentType,
                Data = ByteString.CopyFrom(data),
                EntityName = entityName,
                FileName = fileName,
                FolderName = folderName
            };
            //This try/catch is for debugging purposes in DEV ONLY
            var uploadResult = new UploadFileReply();
            try
            {
                uploadResult = _fileManagerClient.UploadFile(uploadRequest);
            }catch (Exception ex)
            {
                _logger.LogError(ex, "Issue with file upload.");
            }

            if (uploadResult != null && uploadResult.ResultStatus == ResultStatus.Success)
            {
                // Update modifiedon to current time
                //UpdateEntityModifiedOnDate(entityName, entityId, true);
                _logger.LogInformation("Success");
                result.Message = "Uploaded Successfully";
                result.Uploaded = true;
                result.TaskCreated = await createTask(entityId, fileName, folderName, entityName, cancellationToken);
            }
            else
            {
                _logger.LogError(uploadResult.ResultStatus.ToString());
                result.Uploaded = false;
                result.Message = uploadResult.ErrorDetail;
                result.TaskCreated = false;
            }

            return new JsonResult(result);
        }
        /// <summary>
        /// Return the list of files in a given folder.
        /// </summary>
        /// <param name="entityId"></param>
        /// <param name="entityName"></param>
        /// <param name="documentType"></param>
        /// <param name="folderName"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        private async Task<List<FileSystemItem>> GetListFilesInFolder(string entityId, string entityName, string documentType, string folderName, CancellationToken cancellationToken)
        {
            var fileSystemItemVMList = new List<FileSystemItem>();

            if (string.IsNullOrEmpty(entityId) || string.IsNullOrEmpty(entityName) || string.IsNullOrEmpty(documentType)) return fileSystemItemVMList;

            //Three retries? Why only here?
            for (int i = 0; i < 3; i++)
            {
                try
                {
                    
                    // call the web service
                    var request = new FolderFilesRequest
                    {
                        DocumentType = documentType,
                        EntityId = entityId,
                        EntityName = entityName,
                        FolderName = folderName
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
        /// <param name="BCeIDUserId">BCeID</param>
        /// <param name="cancellationToken">cancellation token</param>
        /// <returns></returns>
        private async Task<MicrosoftDynamicsCRMssgCsrsfile> CanAccessDocument(string entityId, string BCeIDUserId, CancellationToken cancellationToken)
        {

            if (string.IsNullOrEmpty(BCeIDUserId)) return null;

            string partyId = await _dynamicsClient.GetPartyIdByBCeIdAsync(BCeIDUserId, cancellationToken);

            if (partyId is null) return null;

            return await _dynamicsClient.GetFileByPartyAndId(partyId, entityId, cancellationToken);

        }

        private async Task<MicrosoftDynamicsCRMssgCsrscommunicationmessage> CanAccessMessageDocument(string entityId, string BCeIDUserId, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(BCeIDUserId)) return null;

            string partyId = await _dynamicsClient.GetPartyIdByBCeIdAsync(BCeIDUserId, cancellationToken);

            if (partyId is null) return null;

            return await _dynamicsClient.GetCommunicationMessagesByPartyAndIdAsync(partyId, entityId, cancellationToken);
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

            var sharepointdocumentlocationid = await _dynamicsClient.GetSharepointDocumentLocationIdByRelatveUrlAsync(mdcsdl.Relativeurl, cancellationToken);

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

        private async Task<bool> createTask(string fileId, string fileName, string folderName, string entityName, CancellationToken cancellationToken)
        {
            
            string subject = $"File: {fileName} Uploaded";
            string description = $"For {fileId} \n Uploaded to: {entityName}\\{folderName}\\{fileName} ";

            MicrosoftDynamicsCRMtask task = new MicrosoftDynamicsCRMtask();
            task.Subject = subject;
            task.Description = description;

            return await _taskService.CreateTask(fileId, subject, description, cancellationToken);

        }

    }

}
