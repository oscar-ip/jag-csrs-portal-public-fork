using Csrs.Api.Models.Dynamics;

namespace Csrs.Api.Repositories
{
    public interface ICsrsFileRepository : IInsertRepository<SSG_CsrsFile>, IUpdateRepository<SSG_CsrsFile>
    {
        Task<SSG_CsrsFile?> GetAsync(Guid id, CancellationToken cancellationToken);
    }
}
