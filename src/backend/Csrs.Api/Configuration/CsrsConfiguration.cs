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

        public JwtConfiguration? Jwt { get; set; }
    }
}
