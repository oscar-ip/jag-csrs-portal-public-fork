using System.ComponentModel.DataAnnotations;

namespace Csrs.Api.Models
{
    public class Message
    {
        public string MessageId { get; set; }
        [Required]
        public string FileId { get; set; }
        [Required]
        public string? Subject { get; set; }
        public string? Content { get; set; }

        public string? RecievingParty { get; set; }
        public DateTimeOffset? Date { get; set; }
        public bool? Attachment { get; set; }
        public List<Document>? Documents { get; set; }
        public int? Status { get; set; }

        public bool? IsRead { get; set; }
    }
}
