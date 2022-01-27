

using MediatR;

namespace Csrs.Api.Features.Files
{
    public static class AuthorizeRespondent
    {
        public class Request : IRequest<Response>
        {
            public Request(string fileId, string password)
            {                
                FileId = fileId ?? throw new ArgumentNullException(nameof(fileId));
                Password = password ?? throw new ArgumentNullException(nameof(password));
            }

            public string FileId { get; }
            public string Password { get; }
        }

        public class Response
        {
            private Response()
            {
            }

            public Response(Guid partyId, Guid fileId)
            {
                if (partyId == Guid.Empty) throw new ArgumentException("partyId cannot be empty", nameof(partyId));
                if (fileId == Guid.Empty) throw new ArgumentException("fileId cannot be empty", nameof(fileId));

                PartyId = partyId;
                FileId = fileId;
            }

            public Guid PartyId { get;  }
            public Guid FileId { get;  }

            /// <summary>
            /// The FileId or Password was null, empty or whitespace.
            /// </summary>
            public static readonly Response BadRequest = new();

            /// <summary>
            /// No matching records with FileId or Password were found. 
            /// </summary>
            public static readonly Response NotFound = new();
        }

        public class Handler : IRequestHandler<Request, Response>
        {
            private readonly ILogger<Handler> _logger;

            public Handler(ILogger<Handler> logger)
            {
                _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            }

            public async Task<Response> Handle(Request request, CancellationToken cancellationToken)
            {
                if (string.IsNullOrWhiteSpace(request.FileId) || string.IsNullOrWhiteSpace(request.Password))
                {
                    // error - return something to indicate error
                    _logger.LogInformation("Either file id or password is null or empty");
                    return Response.BadRequest;
                }

                // business logic

                return new Response(Guid.NewGuid(), Guid.NewGuid());
            }
        }

    }
}
