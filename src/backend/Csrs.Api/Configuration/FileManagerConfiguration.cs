namespace Csrs.Api.Configuration
{
    public class FileManagerConfiguration
    {
        public string Address { get; set; }

        /// <summary>
        /// Use https
        /// </summary>
        public bool? Secure { get; set; }
    }
}
