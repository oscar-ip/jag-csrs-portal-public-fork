using Csrs.Interfaces.Dynamics;
using Csrs.Interfaces.Dynamics.Models;
using Microsoft.Extensions.Caching.Memory;

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

                BCeIDGuid = dynamicsParty.SsgBceidGuid,
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

        public static MicrosoftDynamicsCRMssgCsrsparty ToDynamicsModel(this Party party)
        {
            MicrosoftDynamicsCRMssgCsrsparty dynamicsParty = new MicrosoftDynamicsCRMssgCsrsparty
            {
                SsgCsrspartyid = party.PartyId,
                SsgFirstname = party.FirstName,
                SsgMiddlename = party.MiddleName,
                SsgLastname = party.LastName,
                SsgPreferredname = party.PreferredName,
                SsgGender = party.Gender?.Id,
                SsgDateofbirth = party.DateOfBirth != null ? DateTimeOffset.Parse(party.DateOfBirth) : null,

                SsgStreet1 = party.AddressStreet1,
                SsgStreet2 = party.AddressStreet2,
                SsgCity = party.City,
                SsgProvinceterritory = party.Province?.Id,
                SsgAreapostalcode = party.PostalCode,

                // SsgBceidGuid

                SsgEmail = party.Email,
                SsgCellphone = party.CellPhone,
                SsgHomephone = party.HomePhone,
                SsgWorkphone = party.WorkPhone,

                // OptOutElectronicDocuments
                SsgReferral = party.Referral?.Id,
                SsgIdentity = party.Identity?.Id
            };

            return dynamicsParty;
        }

        public static async Task<File> ToViewModelAsync(MicrosoftDynamicsCRMssgCsrsfile dynamicsFile, IDynamicsClient dynamicsClient, IMemoryCache cache, CancellationToken cancellationToken)
        {
            return null;
        }

        public static MicrosoftDynamicsCRMssgCsrsfile ToDynamicsModel(this File file)
        {
            return null;
        }

        public static async Task<Child> ToViewModelAsync(MicrosoftDynamicsCRMssgCsrschild dynamicsChild, IDynamicsClient dynamicsClient, IMemoryCache cache, CancellationToken cancellationToken)
        {
            return null;
        }

        public static MicrosoftDynamicsCRMssgCsrschild ToDynamicsModel(this Child child)
        {
            MicrosoftDynamicsCRMssgCsrschild dynamicsChild = new MicrosoftDynamicsCRMssgCsrschild
            {
                SsgCsrschildid = child.ChildId,
                SsgFirstname = child.FirstName,
                SsgMiddlename = child.MiddleName,
                SsgLastname = child.LastName,
                SsgDateofbirth = child.DateOfBirth is not null ? DateTimeOffset.Parse(child.DateOfBirth) : null,
                SsgChildisadependent = null                
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
