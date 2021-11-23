using System.Net.Sockets;

namespace Csrs.Api.Repositories
{
    /// <summary>
    /// TODO: move this to a common folder
    /// </summary>
    public static class HttpRequestExceptionExtensions
    {
        /// <summary>
        /// Returns a value indicating if the exception represents a timeout
        /// </summary>
        /// <param name="exception"></param>
        /// <returns></returns>
        public static bool IsTimedOut(this HttpRequestException exception)
        {
            return exception?.InnerException is SocketException && IsTimedOut(exception?.InnerException as SocketException);
        }

        public static bool IsNotFound(this HttpRequestException exception)
        {
            return exception?.InnerException is SocketException && IsTimedOut(exception?.InnerException as SocketException);
        }

        private static bool IsTimedOut(SocketException? exception)
        {
            // A connection attempt failed because the connected party did not properly respond after a period of time, or established connection failed because connected host has failed to respond.
            return exception?.ErrorCode == (int)SocketError.TimedOut;
        }

        private static bool IsConnectionRefused(SocketException? exception)
        {
            // A connection attempt failed because the connected party did not properly respond after a period of time, or established connection failed because connected host has failed to respond.
            return exception?.ErrorCode == (int)SocketError.ConnectionRefused;
        }
    }
}
