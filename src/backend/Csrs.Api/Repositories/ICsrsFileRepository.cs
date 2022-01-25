using Csrs.Api.Models.Dynamics;

namespace Csrs.Api.Repositories
{
    public interface ICsrsFileRepository : IRepository<SSG_CsrsFile>
    {
        /// <summary>
        /// Gets the files where the specified party is either Payor or Recipient.
        /// </summary>
        /// <returns></returns>
        Task<List<SSG_CsrsFile>> GetFileSummaryByPartyAsync(Guid partyId, CancellationToken cancellationToken);
    }
}
