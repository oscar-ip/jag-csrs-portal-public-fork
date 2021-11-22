using System.ComponentModel.DataAnnotations;

namespace Csrs.Api.Models
{
    public class PortalAccount
    {
        public Guid PartyGuid { get; set; }
        [Required]
        public Guid BCeIDGuid { get; set; }
        [Required]
        public string? FirstName { get; set; }
        public string? MiddleName { get; set; }
        [Required]
        public string? LastName { get; set; }
        public string? PreferredName { get; set; }
        [Required]
        public DateTime DateOfBirth { get; set; }
        [Required]
        public string? Gender { get; set; }
        public string? AddressStreet1 { get; set; }
        public string? AddressStreet2 { get; set; }
        public string? City { get; set; }
        public string? Province { get; set; }
        public string? PostalCode { get; set; }
        public string? HomePhone { get; set; }
        public string? WorkPhone { get; set; }
        public string? CellPhone { get; set; }
        [Required]
        public string? Email { get; set; }
        public bool OptOutElectronicDocuments { get; set; }
        public string? Identity { get; set; }
        public string? Referral { get; set; }
    }
}
