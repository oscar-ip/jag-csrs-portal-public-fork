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

        public string FileNumber { get; set; }
        public string PartyEnrolled { get; set; } //y
        public string CourtFileType { get; set; } //y
        public string BCCourtLevel { get; set; } // default value is "Provincial";
        public string BCCourtLocation { get; set; }
        public string DateOfOrderOrWA { get; set; }
        public string PayorIncomeAmountOnOrder { get; set; } = "0";
        public string RecalculationOrderedByCourt { get; set; } = "false";
        public string Section7Expenses { get; set; }
        public string SafetyAlertRecipient { get; set; } = "false";
        public string RecipientSafetyConcernDescription { get; set; }
        public string SafetyAlertPayor { get; set; } = "false";
        public string PayorSafetyConcernDescription { get; set; }
        public string IsFMEPFileActive { get; set; } = "false";
        public string FMEPFileNumber { get; set; }

        //File Number ssg_filenumber  Text Field, max: 100 Char.
        //--
        //Party Enrolled  ssg_partyenrolled	
        //        Picklist:
        //        867670000: Payor
        //        867670001: Recipient
        //--
        //Court  File Type ssg_courtfiletype	
        //        "Picklist:
        //        867670000: Court Order
        //        867670001: Written Agreement
        //--
        //(Field not currently on the Portal application, but must be set to "Provincial").	
        //    BC Court Level ssg_bccourtlevel	"Lookup: 
        //        Targets: ssg_csrsbccourtlevel entity: Provincial and Supreme. "
        //--
        //    BC Court Location ssg_bccourtlocation	
        //    "Lookup: 
        //    Targets: ssg_ijssbccourtlocation entity. 
        //--
        //Date of Order or WA ssg_dateoforderorwa "DateTime; 
        //    Format: DateOnly"
        //--
        //Payor's Income Amount on Order ssg_incomeonorder   "Money type field; 
        //    Minimum value: 0
        //    Maximum value: 922337203685477
        //    Precision: 2"
        //--
        //Recalculation Ordered by the Court ssg_recalculationorderedbythecourt	Boolean field; 
        //        True: Yes
        //        False: No
        //        Default Value: False"
        //--
        //Section 7 Expenses? ssg_section7expenses	
        //        "Picklist:
        //        867670000: Yes
        //        867670001: No
        //        867670002: I do not know
        //--
        //Safety Concern Yes/No* (IF Situation is: Recipient)	
        //    Safety Alert - Recipient ssg_safetyalert	"Boolean field; 
        //        True: Yes
        //        False: No
        //        Default Value: False"
        //--
        //(IF Situation is: Recipient)
        //    Recipient's Safety Concern Description	ssg_safetyconcerndescription	"Text field; 
        //--
        //Safety Concern Yes/No* (IF Situation is: Payor)	
        //    Safety Alert - Payor ssg_safetyalertpayor	"Boolean field; 
        //        True: Yes
        //        False: No
        //        Default Value: False"
        //--
        //(IF Situation is: Payor)
        //    Payor's Safety Concern Description	ssg_payorssafetyconcerndescription	"Text field; 
        //--
        //FMEP File Active? ssg_fmepfileactive	"Boolean field; 
        //        True: Yes
        //        False: No
        //        Default Value: False"
        //--
        //FMEP File Number	ssg_fmepfilenumber	"Text field; 





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
