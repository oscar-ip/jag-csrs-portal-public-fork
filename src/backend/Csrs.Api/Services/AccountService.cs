using Csrs.Api.Models;
using Csrs.Api.Repositories;
using Csrs.Interfaces.Dynamics;
using Csrs.Interfaces.Dynamics.Models;
using Microsoft.Extensions.Caching.Memory;

namespace Csrs.Api.Services
{
    public class AccountService : IAccountService
    {
        private readonly IMemoryCache _cache;
        private readonly IDynamicsClient _dynamicsClient;
        private readonly ILogger<AccountService> _logger;

        public AccountService(
            IMemoryCache cache,
            IDynamicsClient dynamicsClient,
            ILogger<AccountService> logger)
        {
            _cache = cache ?? throw new ArgumentNullException(nameof(cache));
            _dynamicsClient = dynamicsClient ?? throw new ArgumentNullException(nameof(dynamicsClient));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<IList<LookupValue>> GetGendersAsync(CancellationToken cancellationToken)
        {
            IList<LookupValue>? values = await GetPicklistOptionSetMetadataAsync("ssg_csrsparty", "ssg_partygender", cancellationToken);
            return values;
        }

        public async Task<IList<LookupValue>> GetPreferredContactMethodsAsync(CancellationToken cancellationToken)
        {
            IList<LookupValue>? values = await GetPicklistOptionSetMetadataAsync("ssg_csrsparty", "ssg_preferredcontactmethod", cancellationToken);
            return values;
        }

        public async Task<IList<LookupValue>> GetIdentitiesAsync(CancellationToken cancellationToken)
        {
            IList<LookupValue>? values = await GetPicklistOptionSetMetadataAsync("ssg_csrsparty", "ssg_identity", cancellationToken);
            return values;
        }

        public async Task<IList<LookupValue>> GetProvincesAsync(CancellationToken cancellationToken)
        {
            IList<LookupValue>? values = await GetPicklistOptionSetMetadataAsync("ssg_csrsparty", "ssg_provinceterritory", cancellationToken);
            return values;
        }

        public async Task<IList<LookupValue>> GetReferralsAsync(CancellationToken cancellationToken)
        {
            IList<LookupValue>? values = await GetPicklistOptionSetMetadataAsync("ssg_csrsparty", "ssg_referral", cancellationToken);
            return values;
        }
        
        public async Task<Party?> GetPartyByBCeIdAsync(string bceidGuid, CancellationToken cancellationToken)
        {
            using var scope = _logger.AddBCeIdGuid(bceidGuid);

            var parties = await _dynamicsClient.GetPartyByBCeIdAsync(bceidGuid, cancellationToken);

            if (parties.Value == null || parties.Value.Count == 0)
            {
                return null; // not found
            }

            var party = await parties.Value[0].ToViewModelAsync(_dynamicsClient, _cache, cancellationToken);
            return party;
        }

        public async Task<Party> CreateOrUpdateAsync(Party party, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(party);

            MicrosoftDynamicsCRMssgCsrsparty dynamicsParty = party.ToDynamicsModel();

            if (dynamicsParty.SsgCsrspartyid is null)
            {
                dynamicsParty = await _dynamicsClient.Ssgcsrsparties.CreateAsync(dynamicsParty, cancellationToken: cancellationToken);
                party = await dynamicsParty.ToViewModelAsync(_dynamicsClient, _cache, cancellationToken);
            }
            else
            {
                await _dynamicsClient.Ssgcsrsparties.UpdateAsync(dynamicsParty.SsgCsrspartyid, dynamicsParty, cancellationToken);
            }

            return party;
        }

        private async Task<IList<LookupValue>> GetPicklistOptionSetMetadataAsync(string entityName, string attributeName, CancellationToken cancellationToken)
        {
            var metadata = await _dynamicsClient.GetPicklistOptionSetMetadataAsync(entityName, attributeName, _cache, cancellationToken);
            var values = metadata.ToViewModel();
            return values;
        }
    }
}
