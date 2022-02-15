using Csrs.Services.FileManager;
using Microsoft.AspNetCore.Mvc;

namespace Csrs.Api.Services
{
    public interface IDocumentService
    {

        Task<IActionResult> DownloadAttachment(string entityId, string entityName, string serverRelativeUrl, string documentType, CancellationToken cancellationToken);

        Task<IActionResult> UploadAttachment(string entityId, string entityName, IFormFile file, string type, CancellationToken cancellationToken);

        Task<IList<FileSystemItem>> GetAttachmentList(string entityId, string entityName, string documentType, CancellationToken cancellationToken);

    }
}
