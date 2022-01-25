using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;

namespace Csrs.Api.Models
{
    public class Document
    {
        public Guid FileGuid { get; set; }
        
        [Required]
        public Guid RegardingGuid { get; set; }
        
        [Required]
        public string? FileCategory { get; set; }

        [Required]
        public string? FileName { get; set; }
        
        [Required]
        [SwaggerSchema(Format = "binary")]
        public string? Body { get; set; }
        
        public string? FileTag { get; set; }
    }
}
