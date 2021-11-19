namespace Csrs.Api.Repositories
{
    public interface IDeleteRepository<TEntity>
    {
        Task DeleteAsync(TEntity entity, CancellationToken cancellationToken);
    }
}
