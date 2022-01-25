using System.Net;

namespace Csrs.Api.Authentication
{
    [Serializable]
    public class OAuthAuthorizationException : Exception
    {
        public OAuthAuthorizationException(string message, HttpStatusCode statusCode)
            : base(message)
        {
            StatusCode = statusCode;
        }

        public HttpStatusCode StatusCode { get; }
    }
}
