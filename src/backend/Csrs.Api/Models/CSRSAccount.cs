using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Csrs.Api.Models
{
    public class CSRSAccount
    {
        /// <summary>
        /// The input FileNumber parameter to search CSRS account.
        /// </summary>
        [Required]
        public string FileNumber { get; set; }

        /// <summary>
        /// The input Password parameter to search CSRS account.
        /// </summary>
        [Required]
        public string ReferenceNumber { get; set; }

    }

    public class CSRSPartyFileIds
    {
        /// <summary>
        /// The output Party Id of CSRS account.
        /// </summary>
        public string? PartyId { get; set; }

        /// <summary>
        /// The output File Id of CSRS account.
        /// </summary>
        public string? FileId { get; set; }

    }

    public class CSRSAccountFile 
    {
        public string FileId { get; set; }
        public string? FileNumber { get; set; }
        public string? PartyEnrolled { get; set; }
        public string? SafetyAlertRecipient { get; set; } = "No";
        public string? RecipientSafetyConcernDescription { get; set; }
        public string? SafetyAlertPayor { get; set; } = "No";
        public string? PayorSafetyConcernDescription { get; set; }
        public string? IsFMEPFileActive { get; set; } = "No";
        public string? FMEPFileNumber { get; set; }
    }

    public class CSRSAccountRequest
    {
        /// <summary>
        /// The details of the CSRS account. User and file information of the CSRS account will be updated.
        /// </summary>
        [Required]
        public Party? User { get; set; }

        /// <summary>
        /// The CSRS account file to update.
        /// </summary>
        [Required]
        public CSRSAccountFile? CSRSAccountFile { get; set; }
    }



}
