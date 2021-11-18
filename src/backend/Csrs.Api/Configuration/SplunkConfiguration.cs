namespace Csrs.Api.Configuration
{
    public class SplunkConfiguration
    {
        /// <summary>
        /// HTTP Event Collector (HEC) Url
        /// </summary>
        public string? Url { get; set; }

        /// <summary>
        /// HTTP Event Collector (HEC) Token
        /// </summary>
        public string? Token { get; set; }

        /// <summary>
        /// Should the TLS certificate be validated
        /// </summary>
        public bool ValidatServerCertificate { get; set; } = true;
    }
}
