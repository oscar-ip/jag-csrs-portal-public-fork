namespace Csrs.Api.Configuration
{
    public class JwtConfiguration
    {
        /// <summary>
        /// The client id/resource of the application
        /// </summary>
        public string? Audience { get; set; }

        /// <summary>
        /// The authentication server, 
        /// ie http://localhost:5002/auth/realms/csrs-portal or https://dev.oidc.gov.bc.ca/auth/realms/onestopauth-basic
        /// </summary>
        public Uri? Authority { get; set; }
    }
}
