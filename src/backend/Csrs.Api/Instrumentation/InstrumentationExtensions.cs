using Csrs.Api.Configuration;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

namespace Microsoft.Extensions.DependencyInjection;

public static class InstrumentationExtensions
{
    /// <summary>
    /// Add OpenTelemetry Metric and Tracing.
    /// </summary>
    /// <param name="builder"></param>
    public static void AddInstrumentation(this WebApplicationBuilder builder)
    {
        // get the configuration type
        CsrsConfiguration configuration = builder.Configuration.Get<CsrsConfiguration>();

        builder.Services.AddOpenTelemetryMetrics(options =>
        {
            //options.AddPrometheusExporter(_ => { });
        });

        builder.Services.AddOpenTelemetryTracing(builder =>
        {
            builder
                .SetResourceBuilder(ResourceBuilder.CreateDefault().AddService("csrs-api"))
                .AddAspNetCoreInstrumentation()
                .AddHttpClientInstrumentation();

            if (configuration?.Tracing?.Zipkin?.Url is not null)
            {
                builder.AddZipkinExporter(options =>
                {
                    options.Endpoint = configuration.Tracing.Zipkin.Url;
                });
            }

            if (configuration?.Tracing?.Jaeger?.Host is not null)
            {
                builder.AddJaegerExporter(options =>
                {
                    options.AgentHost = configuration.Tracing.Jaeger.Host;
                    if (configuration.Tracing.Jaeger.Port is not null)
                    {
                        options.AgentPort = configuration.Tracing.Jaeger.Port.Value;
                    }
                });
            }

            ////builder.AddOtlpExporter(options => { });
        });
    }
}
