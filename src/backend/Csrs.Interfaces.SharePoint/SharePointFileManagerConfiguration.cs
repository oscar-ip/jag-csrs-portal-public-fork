using System;

namespace Csrs.Interfaces
{
    public class SharePointFileManagerConfiguration
    {
        /// <summary>
        /// The OAuth endpoint that issues the access tokens.
        /// </summary>
        public Uri AuthorizationUri { get; set; }

        /// <summary>
        /// The resource the access token is for.
        /// </summary>
        public Uri Resource { get; set; }

        public string Username { get; set; }
        public string Password { get; set; }

        /// <summary>
        /// Gets or sets the relying party identifier used in ADFS SAML based
        /// authentication.
        /// </summary>
        public string RelyingPartyIdentifier { get; set; }

        /// <summary>
        /// Gets or sets the optional API gateway host.
        /// </summary>
        /// <value>
        /// The API gateway host.
        /// </value>
        public string ApiGatewayHost { get; set; }

        /// <summary>
        /// Gets or sets the API Gateway policy. If no policy is configured
        /// the API gateway will not be used.
        /// </summary>
        public string ApiGatewayPolicy { get; set; }
    }

}
