namespace Csrs.Api.Configuration
{
    public class TracingConfiguration
    {
        public ZipkinConfiguration? Zipkin { get; set; }
        public JaegerConfiguration? Jaeger { get; set; }
    }
}
