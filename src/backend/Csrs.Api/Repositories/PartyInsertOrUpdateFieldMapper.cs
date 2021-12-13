using Csrs.Api.Models;
using Csrs.Api.Models.Dynamics;

namespace Csrs.Api.Repositories
{
    /// <summary>
    /// Maps a party into the dictionary suitable for insert or update in dynamics.
    /// This component does not perfom any data validation.
    /// </summary>
    public class PartyInsertOrUpdateFieldMapper : IInsertOrUpdateFieldMapper<Party, SSG_CsrsParty>
    {
        public Dictionary<string, object?> GetFieldsForInsert(Party model)
        {
            ArgumentNullException.ThrowIfNull(model);

            Dictionary<string, object?> entry = new();

            if (model.BCeIDGuid != Guid.Empty)
            {
                entry.Add(SSG_CsrsParty.Attributes.ssg_bceid_guid, model.BCeIDGuid.ToString("d"));
                entry.Add(SSG_CsrsParty.Attributes.ssg_bceid_last_update, DateTime.Now); // requirement was local time
            }

            entry.Add(SSG_CsrsParty.Attributes.ssg_firstname, model, _ => _.FirstName);
            entry.Add(SSG_CsrsParty.Attributes.ssg_middlename, model, _ => _.MiddleName);
            entry.Add(SSG_CsrsParty.Attributes.ssg_lastname, model, _ => _.LastName);

            // TODO: this should be date?
            entry.Add(SSG_CsrsParty.Attributes.ssg_dateofbirth, model, _ => _.DateOfBirth);

            entry.Add(SSG_CsrsParty.Attributes.ssg_gender, model, _ => _.Gender);
            entry.Add(SSG_CsrsParty.Attributes.ssg_identity, model, _ => _.Identity);
            entry.Add(SSG_CsrsParty.Attributes.ssg_referral, model, _ => _.Referral);

            entry.Add(SSG_CsrsParty.Attributes.ssg_email, model, _ => _.Email);
            entry.Add(SSG_CsrsParty.Attributes.ssg_homephone, model, _ => _.HomePhone);
            entry.Add(SSG_CsrsParty.Attributes.ssg_workphone, model, _ => _.WorkPhone);
            entry.Add(SSG_CsrsParty.Attributes.ssg_cellphone, model, _ => _.CellPhone);

            entry.Add(SSG_CsrsParty.Attributes.ssg_street1, model, _ => _.AddressStreet1);
            entry.Add(SSG_CsrsParty.Attributes.ssg_street2, model, _ => _.AddressStreet2);
            entry.Add(SSG_CsrsParty.Attributes.ssg_city, model, _ => _.City);
            entry.Add(SSG_CsrsParty.Attributes.ssg_provinceterritory, model, _ => _.Province);
            entry.Add(SSG_CsrsParty.Attributes.ssg_areapostalcode, model, _ => _.PostalCode);

            // lookup ?
            //entry.Add(SSG_CsrsParty.Attributes.ssg_csrsoptoutedocuments, model.OptOutElectronicDocuments);

            entry.Add(SSG_CsrsParty.Attributes.statecode, 0);
            entry.Add(SSG_CsrsParty.Attributes.statuscode, SSG_CsrsParty.Active.Id);

            return entry;
        }

        public Dictionary<string, object?> GetFieldsForUpdate(Party model, SSG_CsrsParty entity)
        {
            Dictionary<string, object?> entry = new Dictionary<string, object>();

            entry.Add(SSG_CsrsParty.Attributes.ssg_firstname, model, _ => _.FirstName, entity, _ => _.FirstName);
            entry.Add(SSG_CsrsParty.Attributes.ssg_middlename, model, _ => _.MiddleName, entity, _ => _.MiddleName);
            entry.Add(SSG_CsrsParty.Attributes.ssg_lastname, model, _ => _.LastName, entity, _ => _.LastName);

            // TODO: this should be date?
            if (!string.IsNullOrEmpty(model.DateOfBirth)) entry.Add(SSG_CsrsParty.Attributes.ssg_dateofbirth, model.DateOfBirth);

            entry.Add(SSG_CsrsParty.Attributes.ssg_gender, model, _ => _.Gender);
            entry.Add(SSG_CsrsParty.Attributes.ssg_identity, model, _ => _.Identity);
            entry.Add(SSG_CsrsParty.Attributes.ssg_referral, model, _ => _.Referral);

            entry.Add(SSG_CsrsParty.Attributes.ssg_email, model, _ => _.Email, entity, _ => _.Email);
            entry.Add(SSG_CsrsParty.Attributes.ssg_homephone, model, _ => _.HomePhone, entity, _ => _.HomePhone);
            entry.Add(SSG_CsrsParty.Attributes.ssg_workphone, model, _ => _.WorkPhone, entity, _ => _.WorkPhone);
            entry.Add(SSG_CsrsParty.Attributes.ssg_cellphone, model, _ => _.CellPhone, entity, _ => _.CellPhone);

            entry.Add(SSG_CsrsParty.Attributes.ssg_street1, model, _ => _.AddressStreet1, entity, _ => _.AddressStreet1);
            entry.Add(SSG_CsrsParty.Attributes.ssg_street2, model, _ => _.AddressStreet2, entity, _ => _.AddressStreet2);
            entry.Add(SSG_CsrsParty.Attributes.ssg_city, model, _ => _.City, entity, _ => _.City);

            entry.Add(SSG_CsrsParty.Attributes.ssg_provinceterritory, model, _ => _.Province);
            entry.Add(SSG_CsrsParty.Attributes.ssg_areapostalcode, model, _ => _.PostalCode, entity, _ => _.PostalCode);

            // lookup ?
            //entry.Add(SSG_CsrsParty.Attributes.ssg_csrsoptoutedocuments, model.OptOutElectronicDocuments);

            return entry;
        }

    }

}
