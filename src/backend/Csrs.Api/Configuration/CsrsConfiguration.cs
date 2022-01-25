namespace Csrs.Api.Configuration
{
    /// <remarks>
    /// These fields can be mapped at the root of the appsettings.json file
    /// </remarks>
    public class CsrsConfiguration
    {
        /// <summary>
        /// Contains the Splunk configuration.
        /// </summary>
        public SplunkConfiguration? Splunk { get; set; }
        
        /// <summary>
        /// Contains the OAuth configuration for accessing Dynamics
        /// </summary>
        public OAuthConfiguration? OAuth { get; set; }

        /// <summary>
        /// Contains the Api GateWay options for accessing Dynamics
        /// </summary>
        public ApiGatewayOptions? ApiGateway { get; set; }

        public TracingConfiguration? Tracing { get; set; }

        /// <summary>
        /// Contains the configuration for FileManager service.
        /// </summary>
        public FileManagerConfiguration FileManager { get; set; }
    }
}
