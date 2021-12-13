using Csrs.Api.Models;
using Csrs.Api.Models.Dynamics;

namespace Csrs.Api.Repositories
{
    /// <summary>
    /// Maps a child into the dictionary suitable for insert or update in dynamics.
    /// This component does not perfom any data validation.
    /// </summary>
    public class ChildInsertOrUpdateFieldMapper : IInsertFieldMapper<Child, SSG_CsrsChild>
    {
        public Dictionary<string, object> GetFieldsForInsert(Child model)
        {
            ArgumentNullException.ThrowIfNull(model);

            Dictionary<string, object?> entry = new();

            entry.Add(SSG_CsrsChild.Attributes.ssg_firstname, model, _ => _.FirstName);
            entry.Add(SSG_CsrsChild.Attributes.ssg_lastname, model, _ => _.LastName);
            entry.Add(SSG_CsrsChild.Attributes.ssg_middlename, model, _ => _.MiddleName);
            entry.Add(SSG_CsrsChild.Attributes.ssg_dateofbirth, model, _ => _.DateOfBirth);

            return entry;
        }
    }

}
