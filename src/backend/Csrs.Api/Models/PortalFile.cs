using System.ComponentModel.DataAnnotations;

namespace Csrs.Api.Models
{
    public class PortalFile
    {
        public Guid FileGuid { get; set; }
        [Required]
        public string? UserRole { get; set; }
        [Required]
        public string? Courtlocation { get; set; }
        [Required]
        public string? CourtFileNumber { get; set; }
        public bool OrderedbytheCourtField { get; set; }
        public string? Act { get; set; }
        public string? DateofOrder { get; set; }
        public string? Referrals { get; set; }
        public string? FMEP { get; set; }
        public bool SpecialExpenses { get; set; }
        public bool SplitParenting { get; set; }
        public bool SharedParenting { get; set; }
        public string? RecipientPartyFirstName { get; set; }
        public string? RecipientPartyMiddleName { get; set; }
        public string? RecipientPartyLastName { get; set; }
        public string? RecipientPartyDOB { get; set; }
        public string? RecipientPartyGender { get; set; }
        public string? RecipientPartyAddress { get; set; }
        public string? RecipientPartyPhonenumber { get; set; }
        public string? RecipientPartyEmailAddress { get; set; }
        public string? RecipientPartyIncomeAmountfromtheorder { get; set; }
        public bool RecipientPartyPatternofincome { get; set; }
        public bool PayorSameastheotherparty { get; set; }
        public string? ChildFirstName { get; set; }
        public string? ChildMiddleName { get; set; }
        public string? ChildLastName { get; set; }
        public string? ChildDOB { get; set; }
        public bool ChildIsDependent { get; set; }
        public string? StatusReason { get; set; }
    }
}
