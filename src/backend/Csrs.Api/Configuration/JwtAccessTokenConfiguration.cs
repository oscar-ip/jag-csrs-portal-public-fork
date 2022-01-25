using System.ComponentModel.DataAnnotations;

namespace Csrs.Api.Configuration
{
    public class JwtAccessTokenConfiguration
    {
        [Required]
        public string ClientId { get; set; }
    }
}
