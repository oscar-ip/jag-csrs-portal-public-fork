using Csrs.Api.Models;
using Csrs.Api.Models.Dynamics;
using Csrs.Api.Services;
using Simple.OData.Client;

namespace Csrs.Api.Repositories
{
    public class CsrsPartyRepository : Repository<SSG_CsrsParty>, ICsrsPartyRepository
    {
        private readonly IOptionSetRepository _optionSetService;

        public CsrsPartyRepository(IODataClient client, IOptionSetRepository optionSetService) : base(client)
        {
            _optionSetService = optionSetService ?? throw new ArgumentNullException(nameof(optionSetService));
        }

        public async Task<IList<OptionValue>> GetGenderPicklistAsync(CancellationToken cancellationToken)
        {
            var values = await _optionSetService.GetPickListValuesAsync(SSG_CsrsParty.EntityLogicalName, SSG_CsrsParty.Attributes.ssg_gender, cancellationToken);
            return values;
        }

        public async Task<IList<OptionValue>> GetIdentityPicklistAsync(CancellationToken cancellationToken)
        {
            var values = await _optionSetService.GetPickListValuesAsync(SSG_CsrsParty.EntityLogicalName, SSG_CsrsParty.Attributes.ssg_identity, cancellationToken);
            return values;
        }

        public async Task<IList<OptionValue>> GetProvincePicklistAsync(CancellationToken cancellationToken)
        {
            var values = await _optionSetService.GetPickListValuesAsync(SSG_CsrsParty.EntityLogicalName, SSG_CsrsParty.Attributes.ssg_provinceterritory, cancellationToken);
            return values;
        }

        public async Task<IList<OptionValue>> GetReferralPicklistAsync(CancellationToken cancellationToken)
        {
            var values = await _optionSetService.GetPickListValuesAsync(SSG_CsrsParty.EntityLogicalName, SSG_CsrsParty.Attributes.ssg_referral, cancellationToken);
            return values;
        }
    }
}
