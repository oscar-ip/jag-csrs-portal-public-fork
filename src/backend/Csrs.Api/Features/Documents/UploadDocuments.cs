using Csrs.Api.Services;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Csrs.Api.Features.Documents
{
    public static class UploadDocuments
    {
        public class Request : IRequest<Response>
        {
            public Request(string entityId, string entityName, IFormFile file, string type)
            {

                EntityId = entityId;

                EntityName = entityName;

                File = file;

                Type = type;

            }

            public string EntityId { get; init; }

            public string EntityName { get; init; }

            public IFormFile File { get; init; }

            public string Type { get; init; }

        }
        public class Response
        { 
            public Response(IActionResult actionResult)
            {
                ActionResult = actionResult;
            }
            public IActionResult ActionResult { get; init; }

        }
        public class Handler : IRequestHandler<Request, Response>
        {
            private readonly ILogger<Handler> _logger;
            private readonly IDocumentService _documentService;
            public Handler(ILogger<Handler> logger, IDocumentService documentService)
            {
                _logger = logger ?? throw new ArgumentNullException(nameof(logger));
                _documentService = documentService ?? throw new ArgumentNullException(nameof(documentService));
            }

            public async Task<Response> Handle(Request request, CancellationToken cancellationToken)
            {

                return new Response(await _documentService.UploadAttachment(request.EntityId, request.EntityName, request.File, request.Type, cancellationToken));

            }
        }

    }
}