using Csrs.Api.Models.Dynamics;

namespace Csrs.Api.Repositories
{
    public class FileInsertOrUpdateFieldMapper : IInsertOrUpdateFieldMapper<Models.File, SSG_CsrsFile>
    {
        public Dictionary<string, object?> GetFieldsForInsert(Models.File model)
        {
            ArgumentNullException.ThrowIfNull(model);

            Dictionary<string, object?> entry = new();

            //entry.Add(SSG_CsrsFile.Attributes.ssg, model, _ => _.);

            return entry;
        }

        public Dictionary<string, object?> GetFieldsForUpdate(Models.File model, SSG_CsrsFile entity)
        {
            throw new NotImplementedException();
        }
    }
}
