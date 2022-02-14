using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Csrs.Api.Models
{
    public class Party
    {
        public string PartyId { get; set; }
        
        [Required]
        public string? FirstName { get; set; }
        public string? MiddleName { get; set; }
        [Required]
        public string? LastName { get; set; }
        public string? PreferredName { get; set; }
        [Required]
        [SwaggerSchema(Format = "date")]
        public string? DateOfBirth { get; set; }
        [Required]
        public LookupValue? Gender { get; set; }
        public string? AddressStreet1 { get; set; }
        public string? AddressStreet2 { get; set; }
        public string? City { get; set; }
        public LookupValue? Province { get; set; }
        public string? PostalCode { get; set; }
        public string? HomePhone { get; set; }
        public string? WorkPhone { get; set; }
        public string? CellPhone { get; set; }
        [Required]
        public string? Email { get; set; }
        public bool? OptOutElectronicDocuments { get; set; }
        public LookupValue? Identity { get; set; }
        public LookupValue? Referral { get; set; }

        public LookupValue? PreferredContactMethod { get; set; }
        public string? ReferenceNumber { get; set; }
    }
}
