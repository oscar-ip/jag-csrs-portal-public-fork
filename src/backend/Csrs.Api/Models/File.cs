using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Csrs.Api.Models
{
    public class AccountFileSummary
    {
        /// <summary>
        /// The details of the current user's account.
        /// </summary>
        [Required]
        public Party? User { get; set; }

        public IList<FileSummary> Files { get; set; }
    }

    public class FileSummary
    {
        public string? FileId { get; set; }

        /// <summary>
        /// The file status, Draft or Active.
        /// </summary>
        public FileStatus? Status { get; set; }

        /// <summary>
        /// The role the user plays on this file. The other party will have the other role.
        /// </summary>
        [Required]
        public PartyRole UsersRole { get; set; }
    }

    public class File : FileSummary
    {
        public string FileId { get; set; }
        public string? FileNumber { get; set; }
        public string? PartyEnrolled { get; set; } 
        public LookupValue? CourtFileType { get; set; } 
        public string? BCCourtLevel { get; set; } // default value is "Provincial";
        public string? BCCourtLocation { get; set; }

        [SwaggerSchema(Format = "date")]
        public string? DateOfOrderOrWA { get; set; }

        public string? IncomeOnOrder { get; set; }

        //public int? PayorIncomeAmountOnOrder { get; set; } = 0;
        //public bool? RecalculationOrderedByCourt { get; set; } = false;
        public string? Section7Expenses { get; set; }
        public string? SafetyAlertRecipient { get; set; } = "No";
        public string? RecipientSafetyConcernDescription { get; set; }

        public string? SafetyAlertPayor { get; set; } = "No";
        public string? PayorSafetyConcernDescription { get; set; }

        public string? IsFMEPFileActive { get; set; } = "No";
        public string? FMEPFileNumber { get; set; }

        /// <summary>
        /// The other party on the file. The other party could be recipient or payer.
        /// The other party cannot have the same role as the applying party.
        /// </summary>
        public Party? OtherParty { get; set; }

        /// <summary>
        /// The children on this file.
        /// </summary>
        public IList<Child>? Children { get; set; }
    }

    public enum FileStatus
    {
        Unknown  = 0,
        Draft,
        Active,
    }
}
