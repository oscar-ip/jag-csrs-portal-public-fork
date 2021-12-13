using Csrs.Api.Models.Dynamics;

namespace Csrs.Api.Repositories
{
    public class FileInsertOrUpdateFieldMapper : IInsertFieldMapper<Models.File, SSG_CsrsFile>
    {
        public Dictionary<string, object?> GetFieldsForInsert(Models.File model)
        {
            ArgumentNullException.ThrowIfNull(model);

            Dictionary<string, object?> entry = new();

            //entry.Add(SSG_CsrsFile.Attributes.ssg, model, _ => _.);

            return entry;
        }
    }
}
