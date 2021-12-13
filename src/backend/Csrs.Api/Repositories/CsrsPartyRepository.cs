using Csrs.Api.Models;
using Csrs.Api.Models.Dynamics;
using Csrs.Api.Services;
using Simple.OData.Client;
using System.Linq.Expressions;

namespace Csrs.Api.Repositories
{
    public class CsrsPartyRepository : Repository<SSG_CsrsParty>, ICsrsPartyRepository
    {
        private readonly IOptionSetRepository _optionSetService;

        public CsrsPartyRepository(IODataClient client, IOptionSetRepository optionSetService, ILogger<CsrsPartyRepository> logger) : base(client, logger)
        {
            _optionSetService = optionSetService ?? throw new ArgumentNullException(nameof(optionSetService));
        }

        public async Task<List<SSG_CsrsParty>> GetByBCeIdAsync(Guid bceidGuid, Expression<Func<SSG_CsrsParty, object>> properties, CancellationToken cancellationToken)
        {
            var client = Client.For<SSG_CsrsParty>();

            string guidValue = bceidGuid.ToString("d"); // format with dashes : xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx

            using var scope = Logger.BeginScope(new Dictionary<string, object> {
                { SSG_CsrsParty.Attributes.ssg_bceid_guid, guidValue },
                { SSG_CsrsParty.Attributes.statuscode, Active } });

            Logger.LogDebug("Looking for party by bceid Guid");

            IEnumerable<SSG_CsrsParty> entries = await client
                .Filter(_ => _.BCeIdGuid == guidValue && _.StatusCode == Active)
                .Select(properties)
                .FindEntriesAsync(cancellationToken);

            return entries.ToList();
        }

        public async Task<IList<LookupValue>> GetGendersAsync(CancellationToken cancellationToken)
        {
            Logger.LogDebug("Getting gender lookup values");

            var values = await _optionSetService.GetPickListValuesAsync(SSG_CsrsParty.EntityLogicalName, SSG_CsrsParty.Attributes.ssg_partygender, cancellationToken);
            return values;
        }

        public async Task<IList<LookupValue>> GetIdentitiesAsync(CancellationToken cancellationToken)
        {
            Logger.LogDebug("Getting identity lookup values");

            var values = await _optionSetService.GetPickListValuesAsync(SSG_CsrsParty.EntityLogicalName, SSG_CsrsParty.Attributes.ssg_identity, cancellationToken);
            return values;
        }

        public async Task<IList<LookupValue>> GetProvincesAsync(CancellationToken cancellationToken)
        {
            Logger.LogDebug("Getting province lookup values");

            var values = await _optionSetService.GetPickListValuesAsync(SSG_CsrsParty.EntityLogicalName, SSG_CsrsParty.Attributes.ssg_provinceterritory, cancellationToken);
            return values;
        }

        public async Task<IList<LookupValue>> GetReferralsAsync(CancellationToken cancellationToken)
        {
            Logger.LogDebug("Getting referral lookup values");

            var values = await _optionSetService.GetPickListValuesAsync(SSG_CsrsParty.EntityLogicalName, SSG_CsrsParty.Attributes.ssg_referral, cancellationToken);
            return values;
        }
    }
}
