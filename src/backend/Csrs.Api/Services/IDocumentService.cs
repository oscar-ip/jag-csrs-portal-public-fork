using Microsoft.AspNetCore.Mvc;

namespace Csrs.Api.Services
{
    public interface IDocumentService
    {

        Task<IActionResult> DownloadAttachmentInternal(string entityId, string entityName, string serverRelativeUrl, string documentType, CancellationToken cancellationToken);

        Task<IActionResult> UploadAttachmentInternal(string entityId, string entityName, IFormFile file, string type, CancellationToken cancellationToken);

        Task<IActionResult> GetAttachmentList(string entityId, string entityName, string documentType, CancellationToken cancellationToken);

    }
}
