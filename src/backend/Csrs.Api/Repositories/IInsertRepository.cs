namespace Csrs.Api.Repositories
{
    public interface IInsertRepository<TEntity>
    {
        Task<TEntity> InsertAsync(TEntity entity, CancellationToken cancellationToken); 
    }
}
