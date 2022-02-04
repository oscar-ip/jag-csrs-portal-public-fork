using Csrs.Interfaces.Dynamics;
using Csrs.Interfaces.Dynamics.Models;
using Microsoft.Extensions.Caching.Memory;
using System.Globalization;
using System.ComponentModel;

namespace Csrs.Api.Models
{
    public static class ModelExtensions
    {

        public static async Task<Party> ToViewModelAsync(this MicrosoftDynamicsCRMssgCsrsparty dynamicsParty, IDynamicsClient dynamicsClient, IMemoryCache cache, CancellationToken cancellationToken)
        {
            Party party = new Party
            {
                PartyId = dynamicsParty.SsgCsrspartyid,
                FirstName = dynamicsParty.SsgFirstname,
                MiddleName = dynamicsParty.SsgMiddlename,
                LastName = dynamicsParty.SsgLastname,
                PreferredName = dynamicsParty.SsgPreferredname,
                Gender = await GetLookupValueAsync(dynamicsClient, "ssg_partygender", cache, dynamicsParty.SsgGender, cancellationToken),
                DateOfBirth = dynamicsParty.SsgDateofbirth is not null ? dynamicsParty.SsgDateofbirth.Value.Date.ToString() : null, // TODO

                AddressStreet1 = dynamicsParty.SsgStreet1,
                AddressStreet2 = dynamicsParty.SsgStreet2,
                City = dynamicsParty.SsgCity,
                Province = await GetLookupValueAsync(dynamicsClient, "ssg_provinceterritory", cache, dynamicsParty.SsgProvinceterritory, cancellationToken),
                PostalCode = dynamicsParty.SsgAreapostalcode,

                //BCeIDGuid = dynamicsParty.SsgBceidGuid,
                Email = dynamicsParty.SsgEmail,
                CellPhone = dynamicsParty.SsgCellphone,
                HomePhone = dynamicsParty.SsgHomephone,
                WorkPhone = dynamicsParty.SsgWorkphone,

                //OptOutElectronicDocuments = dynamicsParty.SSg

                Referral = await GetLookupValueAsync(dynamicsClient, "ssg_referral", cache, dynamicsParty.SsgReferral, cancellationToken),
                Identity = await GetLookupValueAsync(dynamicsClient, "ssg_identity", cache, dynamicsParty.SsgIdentity, cancellationToken)
            };

            return party;
        }

        private static DateTimeOffset GetDateFromString(/*string sdt*/)
        {



            CultureInfo provider = CultureInfo.InvariantCulture;
            // Parse date and time with custom specifier.
            string dateString = "15 Jun 2008 8:30 AM -08:00";
            string format = "dd MMM yyyy h:mm tt zzz";
            DateTimeOffset result = new DateTimeOffset();
            try
            {
                result = DateTimeOffset.ParseExact(dateString, format, provider);
            }
            catch (FormatException ex)
            {
                Console.WriteLine(ex.Message);
            }

            return result;

            /*
            string pattern = "yyyy-MM-ddThh:mm:ssZ";
            DateTime parsedDate = DateTime.MinValue;
            sdt = "2019-02-28 01:45:00.0000000 -08:00";
            */
            //if (!string.IsNullOrEmpty(sdt) && //DateFormatConverter()
            //                                  //DateTime.TryParseExact(sdt, pattern, null, DateTimeStyles.None, out parsedDate)) { }
            //    DateTime.TryParse(sdt, out parsedDate)) { }
            //
            //return new DateTimeOffset(parsedDate);//, TimeSpan.Zero);

            /*
            DateTimeFormatInfo fmt = new CultureInfo("en-CA").DateTimeFormat;
            string dateString = "Wed. 2 Feb 2022 1:00:00 +1:00";
            return DateTimeOffset.Parse(dateString, fmt);
            */
        }
        public static MicrosoftDynamicsCRMssgCsrsparty ToDynamicsModel(this Party party)
        {
            MicrosoftDynamicsCRMssgCsrsparty dynamicsParty = new MicrosoftDynamicsCRMssgCsrsparty
            {

                //Statecode = 0,
                //SsgStagingfilenumber = ssgStagingfilenumber;
                //SsgIncomeassistance = ssgIncomeassistance;
                SsgReferral = 867670000,
                SsgCellphone = party.CellPhone,
                SsgFirstname = party.FirstName,
                //SsgCsrspartyid = party.PartyId,
                //SsgDateofbirth = new DateTimeOffset(new DateTime(2022,02,02),TimeSpan.Zero),
                SsgLastname = party.LastName,
                //SsgReferencenumber = ssgReferencenumber,
                SsgPartygender = 867670000,
                //SsgBceidUserid = ssgBceidUserid,
                SsgPreferredcontactmethod = 867670000,
                SsgCity = party.City,
                SsgWorkphone = party.WorkPhone,
                SsgProvinceterritory = 867670000,
                SsgMiddlename = party.MiddleName,
                //SsgFullname = party.LastName + ", " + party.FirstName,
                SsgGender = 867670000,
                SsgHomephone = party.HomePhone,
                //Statuscode = 1,
                SsgStreet2 = party.AddressStreet2,
                //SsgStagingid = ssgStagingid,
                SsgAreapostalcode = party.PostalCode,
                SsgEmail = party.Email,
                //SsgBceidLastUpdate = ssgBceidLastUpdate,
                SsgPreferredname = party.PreferredName,
                //SsgPortalaccess = ssgPortalaccess,
                SsgIdentity = 867670000,
                //SsgBceidDisplayname = (party.FirstName[0] + party.LastName).ToUpper(),
                //SsgIdentityotherdetails = ssgIdentityotherdetails,
                SsgStreet1 = party.AddressStreet1
                //SsgBceidGuid = ssgBceidGuid
            };

            //if (!string.IsNullOrEmpty(party.DateOfBirth))
            //   dynamicsParty.SsgDateofbirth = GetDateFromString(party.DateOfBirth);

            return dynamicsParty;
        }

        public static async Task<File> ToViewModelAsync(MicrosoftDynamicsCRMssgCsrsfile dynamicsFile, IDynamicsClient dynamicsClient, IMemoryCache cache, CancellationToken cancellationToken)
        {
            return null;
        }

        public static MicrosoftDynamicsCRMssgCsrsfile ToDynamicsModel(this File file)
        {
            MicrosoftDynamicsCRMssgCsrsfile dynamicsFile = new MicrosoftDynamicsCRMssgCsrsfile
            {
                //SsgCourtfilenumber = ssgCourtfilenumber;
                //SsgDateoforderorwa = ssgDateoforderorwa;
                /*
                SsgIncomeonorderBase = ssgIncomeonorderBase;
                SsgTerminationdate = ssgTerminationdate;
                SsgSplitparentingarrangement = ssgSplitparentingarrangement;
                SsgRecipientsincomeonorderBase = ssgRecipientsincomeonorderBase;
                SsgSection7recipientsproportion = ssgSection7recipientsproportion;
                SsgDateordercommences = ssgDateordercommences;
                SsgSection7payorsamountBase = ssgSection7payorsamountBase;
                SsgStyleofcauseapplicant = ssgStyleofcauseapplicant;
                SsgAutonumber = ssgAutonumber;
                SsgSection7totalamount = ssgSection7totalamount;
                SsgPayorssafetyconcerndescription = ssgPayorssafetyconcerndescription;
                SsgSubmissiondate = ssgSubmissiondate;
                SsgSection7expenses = ssgSection7expenses;
                SsgOffsetchildsupportamountonorderBase = ssgOffsetchildsupportamountonorderBase;
                SsgSafetyalert = ssgSafetyalert;
                SsgAct = ssgAct;
                SsgFmepfilenumber = ssgFmepfilenumber;
                Importsequencenumber = importsequencenumber;
                Utcconversiontimezonecode = utcconversiontimezonecode;
                SsgFileclosedatenrollment = ssgFileclosedatenrollment;
                SsgCourtfiletype = ssgCourtfiletype;
                SsgDaysofmonthpayable = ssgDaysofmonthpayable;
                SsgCsrsfileid = ssgCsrsfileid;
                SsgSharedparenting = ssgSharedparenting;
                SsgRecipientschildsupportonorder = ssgRecipientschildsupportonorder;
                SsgChildsupportonorderBase = ssgChildsupportonorderBase;
                SsgFilealreadyexists = ssgFilealreadyexists;
                Timezoneruleversionnumber = timezoneruleversionnumber;
                SsgOffsetchildsupportamountonorder = ssgOffsetchildsupportamountonorder;
                SsgIncomeonorder = ssgIncomeonorder;
                SsgSafetyalertpayor = ssgSafetyalertpayor;
                SsgRecalculationorderedbythecourt = ssgRecalculationorderedbythecourt;
                SsgNumberofrecalculations = ssgNumberofrecalculations;
                SsgCourtlocation = ssgCourtlocation;
                SsgSafetyconcerndescription = ssgSafetyconcerndescription;
                Versionnumber = versionnumber;
                SsgRecipientschildsupportonorderBase = ssgRecipientschildsupportonorderBase;
                SsgSection7recipientsamountBase = ssgSection7recipientsamountBase;
                SsgSection7payorsamount = ssgSection7payorsamount;
                SsgChildsupportonorder = ssgChildsupportonorder;
                SsgFmepfileactive = ssgFmepfileactive;
                SsgRecipientsincomeneeded = ssgRecipientsincomeneeded;
                SsgFilenumber = ssgFilenumber;
                SsgSpecialexpenseswithdrawndate = ssgSpecialexpenseswithdrawndate;
                SsgRegistrationdate = ssgRegistrationdate;
                Statuscode = statuscode;
                SsgSection7recipientsamount = ssgSection7recipientsamount;
                Overriddencreatedon = overriddencreatedon;
                Statecode = statecode;
                SsgSection7payorsproportion = ssgSection7payorsproportion;
                Exchangerate = exchangerate;
                SsgSection7totalamountBase = ssgSection7totalamountBase;
                SsgPartyenrolled = ssgPartyenrolled;
                SsgRecipientsincomeonorder = ssgRecipientsincomeonorder;
                SsgRecipient = ssgRecipient;
                SsgPayor = ssgPayor;
                SsgBCCourtLocation = ssgBCCourtLocation;
                SsgBCCourtLevel = ssgBCCourtLevel;

                public string FileNumber { get; set; }
                public string PartyEnrolled { get; set; }
                public string CourtFileType { get; set; }
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
                public string FMEPFileNumber { get; set; } */

            };

            return  dynamicsFile;
        }

        public static async Task<Child> ToViewModelAsync(MicrosoftDynamicsCRMssgCsrschild dynamicsChild, IDynamicsClient dynamicsClient, IMemoryCache cache, CancellationToken cancellationToken)
        {
            return null;
        }

        public static MicrosoftDynamicsCRMssgCsrschild ToDynamicsModel(this Child child)
        {
            MicrosoftDynamicsCRMssgCsrschild dynamicsChild = new MicrosoftDynamicsCRMssgCsrschild
            {
                //SsgCsrschildid = child.ChildId,
                SsgFirstname = child.FirstName,
                SsgMiddlename = child.MiddleName,
                SsgLastname = child.LastName,
                //SsgDateofbirth = child.DateOfBirth is not null ? DateTimeOffset.Parse(child.DateOfBirth) : null,
                //SsgChildisadependent = 867670000
            };

            return dynamicsChild;
        }

        public static IList<LookupValue> ToViewModel(this PicklistOptionSetMetadata metadata)
        {
            IList<LookupValue> viewModel = AsViewModel(metadata).ToList();
            return viewModel;
        }

        private static async Task<LookupValue?> GetLookupValueAsync(IDynamicsClient dynamicsClient, string attributeName, IMemoryCache cache, int? id, CancellationToken cancellationToken)
        {
            if (id is null)
            {
                return null;
            }

            var metadata = await dynamicsClient.GetPicklistOptionSetMetadataAsync("ssg_csrsparty", attributeName, cache, cancellationToken);
            var values = metadata.ToViewModel();
            var value = values.SingleOrDefault(_ => _.Id == id.Value);
            return value;
        }

        private static IEnumerable<LookupValue> AsViewModel(PicklistOptionSetMetadata metadata)
        {
            if (metadata is null || metadata.Value is null || metadata.Value.Count == 0)
            {
                yield break;
            }

            foreach (var value in metadata.Value)
            {
                if (value is null || value.OptionSet is null || value.OptionSet.Options is null || value.OptionSet.Options.Count == 0)
                {
                    continue;
                }

                foreach (var option in value.OptionSet.Options)
                {
                    if (option.Label is null || option.Label.UserLocalizedLabel is null || option.Label.UserLocalizedLabel.Label is null)
                    {
                        continue;
                    }

                    yield return new LookupValue { Id = option.Value, Value = option.Label.UserLocalizedLabel.Label };
                }
            }
        }
    }
}
