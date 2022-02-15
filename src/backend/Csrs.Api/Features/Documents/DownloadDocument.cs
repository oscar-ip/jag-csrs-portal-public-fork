using Csrs.Api.Services;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Csrs.Api.Features.Documents
{
    public static class DownloadDocument
    {
        public class Request : IRequest<Response>
        {
            public Request(string entityId, string entityName, string serverRelativeUrl, string type)
            {

                EntityId = entityId;

                EntityName = entityName;

                ServerRelativeUrl = serverRelativeUrl;

                Type = type;

            }

            public string EntityId { get; init; }

            public string EntityName { get; init; }

            public string ServerRelativeUrl { get; init; }

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

                return new Response(await _documentService.DownloadAttachment(request.EntityId, request.EntityName, request.ServerRelativeUrl, request.Type, cancellationToken));
            
            }
        }
    }
}