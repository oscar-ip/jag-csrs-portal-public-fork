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
           
            // for now, just use Zipkin
            builder.AddZipkinExporter(options => {
                // not needed, it's the default
                //options.Endpoint = new Uri("http://localhost:9411/api/v2/spans");
            });

            ////builder.AddJaegerExporter(options => {
            ////    // not needed, it's the default
            ////    //options.AgentHost = "localhost";
            ////    //options.AgentPort = 6831;
            ////});

            ////builder.AddOtlpExporter(options => { });
        });
    }
}
