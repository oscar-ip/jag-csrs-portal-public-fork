using Csrs.Api.Configuration;
using Serilog;

namespace Microsoft.Extensions.DependencyInjection;

public static class SerilogExtensions
{
    public static void UseSerilog(this WebApplicationBuilder builder)
    {
        if (builder is null) throw new ArgumentNullException(nameof(builder));

        builder.Host.UseSerilog((hostingContext, loggerConfiguration) =>
        {
            loggerConfiguration.ReadFrom.Configuration(builder.Configuration);

            // get the configuration type
            CsrsConfiguration configuration = builder.Configuration.Get<CsrsConfiguration>();

            var splunk = configuration.Splunk;
            if (splunk is null || string.IsNullOrEmpty(splunk.Url) || string.IsNullOrEmpty(splunk.Token))
            {
                return;
            }

            HttpClientHandler? handler = null;

            if (!splunk.ValidatServerCertificate)
            {
                handler = new HttpClientHandler
                {
                    ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
                };
            }

            // When writing to splunk, limit data to information level
            loggerConfiguration.WriteTo.EventCollector(
                splunkHost: splunk.Url,
                eventCollectorToken: splunk.Token,
                sourceType: typeof(SerilogExtensions).Assembly?.GetName()?.Name,
                restrictedToMinimumLevel: Serilog.Events.LogEventLevel.Information,
                messageHandler: handler);
        });
    }
}
