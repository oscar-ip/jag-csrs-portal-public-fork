using System.ComponentModel.DataAnnotations;

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
        //[Required]
        //public string? Courtlocation { get; set; }
        //[Required]
        //public string? CourtFileNumber { get; set; }
        //public bool OrderedbytheCourtField { get; set; }
        //public string? Act { get; set; }
        //public string? DateofOrder { get; set; }
        //public string? Referrals { get; set; }
        //public string? FMEP { get; set; }
        //public bool SpecialExpenses { get; set; }
        //public bool SplitParenting { get; set; }
        //public bool SharedParenting { get; set; }

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
