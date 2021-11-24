using Csrs.Api.Configuration;
using OpenTelemetry.Instrumentation.AspNetCore;
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
            // Prometheus Scraping Endpoint is not currently working, comment out for now

            ////options.AddPrometheusExporter(_ => {
            ////    _.StartHttpListener = true;
            ////    _.HttpListenerPrefixes = new string[] { "http://*:15692/" };
            ////});
        });

        builder.Services.AddOpenTelemetryTracing(builder =>
        {
            builder
                .SetResourceBuilder(ResourceBuilder.CreateDefault().AddService("csrs-api"))
                .AddAspNetCoreInstrumentation(Configure)
                .AddHttpClientInstrumentation();

            if (configuration?.Tracing?.Zipkin is not null)
            {
                AddZipkinExporter(builder, configuration.Tracing.Zipkin);
            }

            if (configuration?.Tracing?.Jaeger is not null)
            {
                AddJaegerExporter(builder, configuration.Tracing.Jaeger);
            }

            ////builder.AddOtlpExporter(options => { });
        });

        void Configure(AspNetCoreInstrumentationOptions options)
        {
            if (builder.Environment.IsDevelopment())
            {
                // ignore hot reload 
                options.Filter = httpContext => !httpContext.Request.Path.Value?.StartsWith("/_framework/aspnetcore-browser-refresh.js") ?? false;
            }

            // should we ignore stuff going to splunk or seq?
        }
    }



    private static void AddZipkinExporter(TracerProviderBuilder builder, ZipkinConfiguration configuration)
    {
        if (configuration?.Url is null)
        {
            return;
        }

        builder.AddZipkinExporter(options =>
        {
            options.Endpoint = configuration.Url;

        });
    }

    private static void AddJaegerExporter(TracerProviderBuilder builder, JaegerConfiguration configuration)
    {
        if (configuration?.Host is null)
        {
            return;
        }

        builder.AddJaegerExporter(options =>
        {
            options.AgentHost = configuration?.Host;
            if (configuration?.Port is not null)
            {
                options.AgentPort = configuration.Port.Value;
            }
        });

    }
}
