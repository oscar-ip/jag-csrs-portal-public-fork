using System.Net;

namespace Microsoft.Extensions.DependencyInjection;

public static class LoggerExtensions
{
    public static IDisposable AddPartyId(this ILogger logger, string value)
    {
        return logger.AddProperty("PartyId", value);
    }

    public static IDisposable AddFileId(this ILogger logger, string value)
    {
        return logger.AddProperty("FileId", value);
    }

    public static IDisposable AddBCeIdGuid(this ILogger logger, string value)
    {
        return logger.AddProperty("BCeIdGuid", value);
    }

    public static IDisposable Add(this ILogger logger, HttpStatusCode value)
    {
        return logger.AddProperty("HttpStatusCode", value);
    }

    public static IDisposable AddProperty(this ILogger logger, string name, object value)
    {
        return logger.BeginScope(new Dictionary<string, object> { { name, value } });
    }
}