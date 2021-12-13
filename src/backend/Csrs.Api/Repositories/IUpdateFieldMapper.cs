namespace Csrs.Api.Repositories
{
    /// <summary>
    /// Creates dictionary for updating fields in dynamics based on the supplied model and existing entity.
    /// </summary>
    /// <typeparam name="TModel"></typeparam>
    /// <typeparam name="TEntity"></typeparam>
    public interface IUpdateFieldMapper<TModel, TEntity> 
    {
        Dictionary<string, object?> GetFieldsForUpdate(TModel model, TEntity entity);
    }
}
