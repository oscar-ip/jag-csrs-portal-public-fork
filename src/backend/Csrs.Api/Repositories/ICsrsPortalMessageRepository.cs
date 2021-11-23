using Csrs.Api.Models.Dynamics;

namespace Csrs.Api.Repositories
{
    public interface ICsrsPortalMessageRepository : IRepository<SSG_CsrsPortalMessage>
    {
        Task MarkReadAsync(Guid id, CancellationToken cancellationToken);
    }
}
