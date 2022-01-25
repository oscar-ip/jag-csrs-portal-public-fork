namespace Csrs.Api.Repositories
{
    /// <summary>
    /// Creates dictionary for inserting fields in dynamics based on the supplied model.
    /// </summary>
    /// <typeparam name="TModel"></typeparam>
    /// <typeparam name="TEntity"></typeparam>
    public interface IInsertFieldMapper<TModel, TEntity>
    {
        Dictionary<string, object?> GetFieldsForInsert(TModel model);
    }
}
