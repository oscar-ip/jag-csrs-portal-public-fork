using System.ComponentModel.DataAnnotations;

namespace Csrs.Api.Configuration
{
    public class OAuthConfiguration
    {
        [Required]
        public string? AuthorizationUrl { get; set; }

        [Required]
        public string? ResourceUrl { get; set; }

        [Required]
        public string? ClientId { get; set; }

        [Required]
        public string? Secret { get; set; }

        [Required]
        public string? Username { get; set; }

        [Required]
        public string? Password { get; set; }
    }
}
