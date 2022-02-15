using Csrs.Api.Services;
using Csrs.Services.FileManager;
using MediatR;

namespace Csrs.Api.Features.Documents
{
    public static class ListDocuments
    {
        public class Request : IRequest<Response>
        {
            public Request(string entityId, string entityName, string type)
            {

                EntityId = entityId;

                EntityName = entityName;

                Type = type;

            }

            public string EntityId { get; init; }

            public string EntityName { get; init; }

            public string Type { get; init; }

        }
        public class Response
        {
            public Response(IList<FileSystemItem> attachments)
            {
                Attachments = attachments;
            }
            public IList<FileSystemItem> Attachments { get; init; }

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

                return new Response(await _documentService.GetAttachmentList(request.EntityId, request.EntityName, request.Type, cancellationToken));

            }
        }
    }
}