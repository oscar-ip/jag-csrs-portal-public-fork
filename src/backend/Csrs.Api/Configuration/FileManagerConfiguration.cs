namespace Csrs.Api.Configuration
{
    public class FileManagerConfiguration
    {
        /// <summary>
        /// The address file manager service.
        /// Can be &quot;dns://host:port&quot; for dynamic dns discovery.
        /// </summary>
        public string? Address { get; set; }

        /// <summary>
        /// Use https
        /// </summary>
        public bool? Secure { get; set; }
    }
}
