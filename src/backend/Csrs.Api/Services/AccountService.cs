using AutoMapper;
using Csrs.Api.Models;
using Csrs.Api.Models.Dynamics;
using Csrs.Api.Repositories;
using Microsoft.Extensions.Caching.Memory;
using System.Diagnostics;

namespace Csrs.Api.Services
{
    public class AccountService : IAccountService
    {
        private readonly IMemoryCache _cache;
        private readonly ICsrsPartyRepository _partyRepository;
        private readonly IMapper _mapper;
        private readonly IInsertOrUpdateFieldMapper<Party, SSG_CsrsParty> _partyInsertOrUpdateFieldMapper;
        private readonly ILogger<AccountService> _logger;

        public AccountService(
            IMemoryCache cache,
            ICsrsPartyRepository partyRepository,
            IMapper mapper,
            IInsertOrUpdateFieldMapper<Party, SSG_CsrsParty> partyInsertOrUpdateFieldMapper,
            ILogger<AccountService> logger)
        {
            _cache = cache ?? throw new ArgumentNullException(nameof(cache));
            _partyRepository = partyRepository ?? throw new ArgumentNullException(nameof(partyRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _partyInsertOrUpdateFieldMapper = partyInsertOrUpdateFieldMapper ?? throw new ArgumentNullException(nameof(partyInsertOrUpdateFieldMapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<IList<LookupValue>> GetGendersAsync(CancellationToken cancellationToken)
        {
            IList<LookupValue>? values = await GetPickListAsync("GenderPickList", _partyRepository.GetGendersAsync, cancellationToken);
            return values;
        }

        public async Task<IList<LookupValue>> GetPreferredContactMethodsAsync(CancellationToken cancellationToken)
        {
            IList<LookupValue>? values = await GetPickListAsync("PreferredContactMethodsPickList", _partyRepository.GetPreferredContactMethodsAsync, cancellationToken);
            return values;
        }

        public async Task<IList<LookupValue>> GetIdentitiesAsync(CancellationToken cancellationToken)
        {
            IList<LookupValue>? values = await GetPickListAsync("IdentitiesPickList", _partyRepository.GetIdentitiesAsync, cancellationToken);
            return values;
        }

        public async Task<IList<LookupValue>> GetProvincesAsync(CancellationToken cancellationToken)
        {
            IList<LookupValue>? values = await GetPickListAsync("ProvincesPickList", _partyRepository.GetProvincesAsync, cancellationToken);
            return values;
        }

        public async Task<IList<LookupValue>> GetReferralsAsync(CancellationToken cancellationToken)
        {
            IList<LookupValue>? values = await GetPickListAsync("ReferralsPickList", _partyRepository.GetReferralsAsync, cancellationToken);
            return values;
        }

        public async Task<Party?> GetPartyAsync(Guid partyId, CancellationToken cancellationToken)
        {
            if (partyId == Guid.Empty)
            {
                _logger.LogDebug("Cannot lookup party with an empty id");
                return null;
            }

            using var scope = _logger.AddPartyId(partyId);

            var csrsParty = await _partyRepository.GetAsync(partyId, SSG_CsrsParty.AllProperties, cancellationToken);
            if (csrsParty == null)
            {
                _logger.LogDebug("Party not found");
                return null;
            }

            var party = await MapAsync(csrsParty, cancellationToken);
            return party;
        }

        public async Task<Party?> GetPartyByBCeIdAsync(Guid bceidGuid, CancellationToken cancellationToken)
        {
            using var scope = _logger.AddBCeIdGuid(bceidGuid);

            List<SSG_CsrsParty>? parties = await _partyRepository.GetByBCeIdAsync(bceidGuid, SSG_CsrsParty.AllProperties, cancellationToken);

            SSG_CsrsParty? csrsParty = GetNewestParty(parties);
            if (csrsParty?.PartyId is null)
            {
                return null;
            }

            var party = await MapAsync(csrsParty, cancellationToken);
            return party;
        }

        public async Task<Party> CreateOrUpdateAsync(Party party, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(party);

            SSG_CsrsParty? csrsParty = null;

            // if the caller had a party id specificed, attempt to look up that party
            if (party.PartyId != Guid.Empty)
            {
                csrsParty = await _partyRepository.GetAsync(party.PartyId, SSG_CsrsParty.AllProperties, cancellationToken);
                if (csrsParty is null)
                {
                    using var scope = _logger.AddPartyId(party.PartyId);
                    _logger.LogInformation("Could not find party. will insert a new party, party id will be reset");
                    party.PartyId = Guid.Empty;
                }
            }

            if (party.PartyId == Guid.Empty)
            {
                _logger.LogInformation("Source party does not have a PartyId, inserting");

                Dictionary<string, object>? fields = _partyInsertOrUpdateFieldMapper.GetFieldsForInsert(party);


                csrsParty = await _partyRepository.InsertAsync(fields, cancellationToken);
            }
            else
            {
                Debug.Assert(csrsParty != null);

                using var scope = _logger.AddPartyId(party.PartyId);
                _logger.LogInformation("Source party exists, updating");
                Dictionary<string, object>? fields = _partyInsertOrUpdateFieldMapper.GetFieldsForUpdate(party, csrsParty);
                csrsParty = await _partyRepository.UpdateAsync(csrsParty.Key, fields, cancellationToken);
            }

            party = _mapper.Map<Party>(csrsParty);

            return party;
        }

        private SSG_CsrsParty? GetNewestParty(List<SSG_CsrsParty>? parties)
        {
            if (parties == null || parties.Count == 0)
            {
                _logger.LogDebug("No parties found");
                return null;
            }

            if (parties.Count == 1)
            {
                // happy path
                _logger.LogDebug("Single party found");
                return parties[0];
            }

            _logger.LogInformation("{Count} parties found with same BCeID identifier, returning the last modified entry", parties.Count);

            //
            // TODO: most recent needs to be based on BCeIdLastUpdate not ModifiedOn
            //


            // Take the last modified one, assumes two records will not be updated at the exact same time
            SSG_CsrsParty? csrsParty = parties.OrderByDescending(_ => _.ModifiedOn).First();

            // double check there are not two or more records with the exact same modification date, otherwise, the system may behave oddly
            int count = parties.Count(_ => _.ModifiedOn == csrsParty.ModifiedOn);
            if (count != 1)
            {
                // this should not happen
                _logger.LogWarning("{Count} parties found with same BCeID identifier and have the same last modified date, user may not get consistent results", count);
            }

            return csrsParty;
        }

        private async Task<Party> MapAsync(SSG_CsrsParty csrsParty, CancellationToken cancellationToken)
        {
            var party = _mapper.Map<Party>(csrsParty);

            // todo: these lookups should be cached
            if (!string.IsNullOrEmpty(csrsParty.Identity) && int.TryParse(csrsParty.Identity, out int identityId))
            {
                IList<LookupValue>? identities = await GetIdentitiesAsync(cancellationToken);
                party.Identity = identities.SingleOrDefault(_ => _.Id == identityId);
            }

            if (!string.IsNullOrEmpty(csrsParty.Gender) && int.TryParse(csrsParty.Gender, out int genderId))
            {
                IList<LookupValue>? genders = await GetGendersAsync(cancellationToken);
                party.Gender = genders.SingleOrDefault(_ => _.Id == genderId);
            }

            if (!string.IsNullOrEmpty(csrsParty.Referral) && int.TryParse(csrsParty.Referral, out int referralId))
            {
                IList<LookupValue>? referrals = await GetReferralsAsync(cancellationToken);
                party.Referral = referrals.SingleOrDefault(_ => _.Id == referralId);
            }

            if (!string.IsNullOrEmpty(csrsParty.Province) && int.TryParse(csrsParty.Province, out int provinceId))
            {
                IList<LookupValue>? provinces = await GetProvincesAsync(cancellationToken);
                party.Province = provinces.SingleOrDefault(_ => _.Id == provinceId);
            }

            return party;
        }


        private async Task<IList<LookupValue>> GetPickListAsync(string cacheKey, Func<CancellationToken, Task<IList<LookupValue>>> fetchPickList, CancellationToken cancellationToken)
        {
            IList<LookupValue>? values = _cache.Get<IList<LookupValue>>(cacheKey);
            if (values is not null)
            {
                return values;
            }

            values = await fetchPickList(cancellationToken);

            if (values is not null)
            {
                var random = new Random();
                var seconds = random.Next(-120, 120);
                // cache for 1 hour +/- two minutes
                var absolteExpiration = DateTime.UtcNow.AddSeconds(60 * 60 + seconds);

                _cache.Set(cacheKey, values, absolteExpiration);
            }
            else
            {
                values = Array.Empty<LookupValue>();
            }

            return values;
        }
    }

}
