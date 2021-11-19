namespace Csrs.Api.Repositories
{
    public interface IUpdateRepository<TEntity>
    {
        Task<TEntity> UpdateAsync(TEntity entity, CancellationToken cancellationToken);
    }
}
