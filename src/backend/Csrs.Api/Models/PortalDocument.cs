using System.ComponentModel.DataAnnotations;

namespace Csrs.Api.Models
{
    public class PortalDocument
    {
        public Guid FileGuid { get; set; }
        
        [Required]
        public Guid RegardingGuid { get; set; }
        
        [Required]
        public string? FileCategory { get; set; }

        [Required]
        public string? FileName { get; set; }
        
        [Required]
        public string? DocumentBody { get; set; }
        
        public string? FileTag { get; set; }
    }
}
