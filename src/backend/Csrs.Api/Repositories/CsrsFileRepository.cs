using Csrs.Api.Models.Dynamics;
using Simple.OData.Client;

namespace Csrs.Api.Repositories
{
    public class CsrsFileRepository : Repository<SSG_CsrsFile>, ICsrsFileRepository
    {
        public CsrsFileRepository(IODataClient client) : base(client)
        {
        }

        public async Task<List<SSG_CsrsFile>> GetFileSummaryByPartyAsync(Guid partyId, CancellationToken cancellationToken)
        {
            // trying to filter .Filter(_ => statusCodes.Contains(_.StatusCode)) causes bad request from dynamics,
            // so filter the results afterward, assumes there wont be a large number of files in other states

            int active = SSG_CsrsFile.StatusCodes.FromName(SSG_CsrsFile.Active)?.Value ?? -1;
            int draft = SSG_CsrsFile.StatusCodes.FromName(SSG_CsrsFile.Draft)?.Value ?? -1;

            var entries = await Client
                .For<SSG_CsrsFile>()
                .Filter(_ => _.Payor!.PartyId == partyId || _.Recipient!.PartyId == partyId)
                .Select(SSG_CsrsFile.AllProperties)
                .Expand(_ => _.Payor)
                .Expand(_ => _.Recipient)
                .FindEntriesAsync(cancellationToken);

            var files = entries
                .Where(_ => _.StatusCode == active || _.StatusCode == draft)
                .ToList();

            return files;
        }
    }
}
