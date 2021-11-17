using System.ComponentModel.DataAnnotations;

namespace Csrs.Api.Models
{
    public class PortalUserRequest
    {
        [Required]
        public string? FileNo { get; set; }
        [Required]
        public string? RequestType { get; set; }
        [Required]
        public string? RequestMessage { get; set; }
    }
}
