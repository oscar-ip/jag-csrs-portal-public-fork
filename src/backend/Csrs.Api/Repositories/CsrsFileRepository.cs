using Csrs.Api.Models.Dynamics;
using Simple.OData.Client;

namespace Csrs.Api.Repositories
{
    public class CsrsFileRepository : Repository<SSG_CsrsFile>, ICsrsFileRepository
    {
        public CsrsFileRepository(IODataClient client, ILogger<CsrsFileRepository> logger) : base(client, logger)
        {
        }

        public async Task<List<SSG_CsrsFile>> GetFileSummaryByPartyAsync(Guid partyId, CancellationToken cancellationToken)
        {
            // trying to filter .Filter(_ => statusCodes.Contains(_.StatusCode)) causes bad request from dynamics,
            // so filter the results afterward, assumes there wont be a large number of files in other states

            var entries = await Client
                .For<SSG_CsrsFile>()
                .Filter(_ => _.Payor!.PartyId == partyId || _.Recipient!.PartyId == partyId)
                .Filter(_ => _.StatusCode == SSG_CsrsFile.Active.Id || _.StatusCode == SSG_CsrsFile.Draft.Id)
                .Select(SSG_CsrsFile.AllProperties)
                .Expand(_ => _.Payor)
                .Expand(_ => _.Recipient)
                .FindEntriesAsync(cancellationToken);

            var files = entries.ToList();
            //    .Where(_ => _.StatusCode == SSG_CsrsFile.Active.Id || _.StatusCode == SSG_CsrsFile.Draft.Id).ToList();

            return files;
        }
    }
}
