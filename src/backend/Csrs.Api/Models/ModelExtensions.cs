using Csrs.Interfaces.Dynamics;
using Csrs.Interfaces.Dynamics.Models;
using Microsoft.Extensions.Caching.Memory;
using System.Globalization;
using System.ComponentModel;
using System.Linq;

namespace Csrs.Api.Models
{
    public static class ModelExtensions
    {
        public static async Task<Party> ToViewModelAsync(this MicrosoftDynamicsCRMssgCsrsparty dynamicsParty, IDynamicsClient dynamicsClient, IMemoryCache cache, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(dynamicsParty);
            ArgumentNullException.ThrowIfNull(dynamicsClient);
            ArgumentNullException.ThrowIfNull(cache);

            Party party = new Party
            {
                PartyId = dynamicsParty.SsgCsrspartyid,
                FirstName = dynamicsParty.SsgFirstname,
                MiddleName = dynamicsParty.SsgMiddlename,
                LastName = dynamicsParty.SsgLastname,
                PreferredName = dynamicsParty.SsgPreferredname,
                Gender = await GetLookupValueAsync(dynamicsClient, "ssg_partygender", cache, dynamicsParty.SsgGender, cancellationToken),
                DateOfBirth = dynamicsParty.SsgDateofbirth is not null ? dynamicsParty.SsgDateofbirth.Value.Date.ToString() : null, 
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

                Referral = await GetLookupValueAsync(dynamicsClient, "ssg_referral", cache, dynamicsParty.SsgReferral, cancellationToken),
                Identity = await GetLookupValueAsync(dynamicsClient, "ssg_identity", cache, dynamicsParty.SsgIdentity, cancellationToken),
                PreferredContactMethod = await GetLookupValueAsync(dynamicsClient, "ssg_preferredcontactmethod", cache, dynamicsParty.SsgPreferredcontactmethod, cancellationToken)
            };
            return party;
        }
    
        public static MicrosoftDynamicsCRMssgCsrsparty ToDynamicsModel(this Party party)
        {
            MicrosoftDynamicsCRMssgCsrsparty dynamicsParty = new MicrosoftDynamicsCRMssgCsrsparty
            {

                Statecode = 0,
                Statuscode = 1,
                //SsgIncomeassistance = ssgIncomeassistance;
                SsgReferral = party.Referral?.Id,
                SsgCellphone = party.CellPhone,
                SsgFirstname = party.FirstName,
                SsgDateofbirth = ToDateTimeOffset(party.DateOfBirth),
                SsgLastname = party.LastName,
                SsgReferencenumber = party.ReferenceNumber,
                SsgPartygender = party.Gender?.Id,
                SsgPreferredcontactmethod = party.PreferredContactMethod?.Id,
                SsgCity = party.City,
                SsgWorkphone = party.WorkPhone,
                SsgProvinceterritory = party.Province?.Id,
                SsgMiddlename = party.MiddleName,
                SsgGender = party.Gender?.Id,
                SsgHomephone = party.HomePhone,
                SsgStreet2 = party.AddressStreet2,
                SsgAreapostalcode = party.PostalCode,
                SsgEmail = party.Email,
                //SsgBceidLastUpdate = ssgBceidLastUpdate, ????
                SsgPreferredname = party.PreferredName,
                //SsgPortalaccess = ssgPortalaccess, ????
                SsgIdentity = party.Identity?.Id,
                //SsgIdentityotherdetails = ssgIdentityotherdetails, ???
                SsgStreet1 = party.AddressStreet1
            };
            return dynamicsParty;
        }

        public static async Task<MicrosoftDynamicsCRMssgCsrsfile> ToDynamicsModel(this File file, IDynamicsClient dynamicsClient, CancellationToken cancellationToken)
        {
            string courtlevelId = await GetCourtLevelValueAsync(dynamicsClient, file.BCCourtLevel, cancellationToken);
            string courtLocationId = await GetCourtLocationValueAsync(dynamicsClient, file.BCCourtLocation, cancellationToken);

            MicrosoftDynamicsCRMssgCsrsfile dynamicsFile = new MicrosoftDynamicsCRMssgCsrsfile
            {
                SsgCourtfiletype = file.CourtFileType?.Id,
                SsgFmepfileactive = ToBoolean(file.IsFMEPFileActive),
                SsgFmepfilenumber = file.FMEPFileNumber,

                SsgSafetyalert = ToBoolean(file.SafetyAlertRecipient),
                SsgSafetyconcerndescription = file.RecipientSafetyConcernDescription,
                SsgSafetyalertpayor = ToBoolean(file.SafetyAlertPayor),
                SsgPayorssafetyconcerndescription = file.PayorSafetyConcernDescription,

                SsgSection7expenses = GetSection7Expenses(file.Section7Expenses),
                SsgDateoforderorwa = ToDateTimeOffset(file.DateOfOrderOrWA),
                SsgIncomeonorder = ToDecimal(file.IncomeOnOrder),
                SsgPartyenrolled = GetPartyEnrolled(file.PartyEnrolled),

                //SsgSharedparenting = true; ???
                //SsgSplitparentingarrangement = true; ??
                //SsgRegistrationdate = ConvertToDTOffset(file.), ??
                //SsgStyleofcauseapplicant = "Applicant", ??
                //SsgStyleofcauserespondent = "Respondent", ??

                SsgBCCourtLevelODataBind = dynamicsClient.GetEntityURI("ssg_csrsfiles", courtlevelId),
                SsgBCCourtLocationODataBind = dynamicsClient.GetEntityURI("ssg_csrsfiles", courtLocationId),

            };
            return dynamicsFile;
        }

        public static MicrosoftDynamicsCRMssgCsrschild ToDynamicsModel(this Child child)
        {
            MicrosoftDynamicsCRMssgCsrschild dynamicsChild = new MicrosoftDynamicsCRMssgCsrschild
            {
                SsgFirstname = child.FirstName,
                SsgMiddlename = child.MiddleName,
                SsgLastname = child.LastName,
                SsgDateofbirth = ToDateTimeOffset(child.DateOfBirth),
                SsgChildisadependent = GetChildIsDependent(child.ChildIsDependent)
            };

            return dynamicsChild;
        }

        public static IList<LookupValue> ToViewModel(this PicklistOptionSetMetadata metadata)
        {
            IList<LookupValue> viewModel = AsViewModel(metadata).ToList();
            return viewModel;
        }

        public static Message ToViewModel(MicrosoftDynamicsCRMssgCsrscommunicationmessage inMessage, List<Document> documents)
        {
            Message message = new Message();

            message.MessageId = inMessage.SsgCsrscommunicationmessageid;
            message.FileId = inMessage._ssgCsrsfileValue;
            message.Attachment = inMessage.SsgCsrsmessageattachment;
            message.Status = inMessage.Statuscode;
            message.Documents = documents;
            message.RecievingParty = inMessage._ssgTopartyValue;
            message.Content = inMessage.SsgCsrsmessage;
            message.Subject = inMessage.SsgCsrsmessagesubject;
            message.Date = inMessage.SsgSentreceiveddate;
            message.IsRead = inMessage.SsgCsrsmessageread;

            return message;
        }

        private static DateTimeOffset? ToDateTimeOffset(string? value)
        {
            if (value is null)
            {
                return null;
            }

            if (DateTimeOffset.TryParse(value, out DateTimeOffset result))
            {
                return result;
            }

            return null;
        }

        private static Decimal? ToDecimal(string? value)
        {
            if (value is null)
            {
                return null;
            }

            if (Decimal.TryParse(value, out Decimal result))
            {
                return result;
            }

            return null;
        }
        private static int? GetSection7Expenses(string? value)
        {
            if (value is null)
            {
                return null;
            }

            return value switch
            {
                "Yes" => (int)Section7Expenses.Yes,
                "No" => (int)Section7Expenses.No,
                _ => (int)Section7Expenses.IDontKnow,
            };
        }

        private static int? GetPartyEnrolled(string? value)
        {
            if (value is null)
            {
                return null;
            }

            return value switch
            {
                "Recipient" => (int)PartyEnrolled.Recipient,
                "Payor" => (int)PartyEnrolled.Payor,
                _ => null
            };
        }

        private static bool? ToBoolean(string? value)
        {
            if (value is null)
            {
                return null;
            }

            return value switch
            {
                "Yes" => true,
                "No" => false,
                _ => null
            };
        }

        private static int? GetChildIsDependent(string? value)
        {
            if (value is null)
            {
                return null;
            }

            return value switch
            {
                "Yes" => (int)ChildIsDependent.Yes,
                "No" => (int)ChildIsDependent.No,
                _ => (int)ChildIsDependent.IDontKnow
            };
        }

        private static async Task<LookupValue?> GetLookupValueAsync(IDynamicsClient dynamicsClient, string attributeName, IMemoryCache cache, int? id, CancellationToken cancellationToken)
        {
            if (id is null)
            {
                return null;
            }

            var metadata = await dynamicsClient.GetPicklistOptionSetMetadataAsync("ssg_csrsparty", attributeName, cache, cancellationToken);
            var values = metadata.ToViewModel();
            var value = values.Where(_ => _.Id == id.Value).SingleOrDefault();
            return value;
        }

        private static async Task<string?> GetCourtLocationValueAsync(IDynamicsClient dynamicsClient, string? label, CancellationToken cancellationToken)
        {
            if (label is null)
            {
                return null;
            }

            // TODO: this needs caching!!!
            var values =  await dynamicsClient.GetCourtLocationsAsync(cancellationToken);

            if (values is null || values.Value is null)
            {
                return null;
            }

            var valuesbyName = values.Value.ToLookup(_ => _.SsgBccourtlocationname, StringComparer.OrdinalIgnoreCase);

            return valuesbyName[label].FirstOrDefault()?.SsgIjssbccourtlocationid;
        }

        private static async Task<string?> GetCourtLevelValueAsync(IDynamicsClient dynamicsClient, string? label, CancellationToken cancellationToken)
        {
            if (label is null)
            {
                return null;
            }

            // TODO: this needs caching!!!
            var values = await dynamicsClient.GetCourtLevelsAsync(cancellationToken);

            if (values is null || values.Value is null)
            {
                return null;
            }

            var valuesbyName = values.Value.ToLookup(_ => _.SsgCourtlevellabel, StringComparer.OrdinalIgnoreCase);

            return valuesbyName[label].FirstOrDefault()?.SsgCsrsbccourtlevelid;
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
