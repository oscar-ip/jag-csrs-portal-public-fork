using System.ComponentModel.DataAnnotations;

namespace Csrs.Api.Models
{
    public class Message
    {
        public Guid MessageGuid { get; set; }
        [Required]
        public Guid FileGuid { get; set; }
        [Required]
        public string? FileNo { get; set; }
        [Required]
        public string? Subject { get; set; }
        public string? Content { get; set; }

        public string? RecievingParty { get; set; }
        public string? Date { get; set; }
        public bool Attachment { get; set; }
        public List<Document>? Documents { get; set; }
        public bool IsRead { get; set; }
    }
}
