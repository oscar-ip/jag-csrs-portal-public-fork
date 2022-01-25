namespace Csrs.Api.Repositories
{
    /// <summary>
    /// Combines insert and update into a single interface
    /// </summary>
    /// <typeparam name="TModel"></typeparam>
    /// <typeparam name="TEntity"></typeparam>
    public interface IInsertOrUpdateFieldMapper<TModel, TEntity> : IInsertFieldMapper<TModel, TEntity>, IUpdateFieldMapper<TModel, TEntity>
    {
    }
}
