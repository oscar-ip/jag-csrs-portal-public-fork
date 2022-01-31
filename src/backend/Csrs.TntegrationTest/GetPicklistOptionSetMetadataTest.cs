using System.Threading;
using System.Threading.Tasks;
using Csrs.Interfaces.Dynamics;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Csrs.TntegrationTest
{


    public class GetPicklistOptionSetMetadataTest : DynamicsClientTestBase
    {
        [DebugOnlyFact]
        public async Task can_get_party_genders()
        {
            IMemoryCache cache = _serviceProvider.GetRequiredService<IMemoryCache>();
            IDynamicsClient dynamicsClient = _serviceProvider.GetRequiredService<IDynamicsClient>();

            var actual = await dynamicsClient.GetPicklistOptionSetMetadataAsync("ssg_csrsparty", "ssg_partygender", cache, CancellationToken.None);
            Assert.NotNull(actual);
            Assert.NotEmpty(actual.Value);
        }

        [DebugOnlyFact]
        public async Task can_get_party_referrals()
        {
            IMemoryCache cache = _serviceProvider.GetRequiredService<IMemoryCache>();
            IDynamicsClient dynamicsClient = _serviceProvider.GetRequiredService<IDynamicsClient>();

            var actual = await dynamicsClient.GetPicklistOptionSetMetadataAsync("ssg_csrsparty", "ssg_referral", cache, CancellationToken.None);
            Assert.NotNull(actual);
            Assert.NotEmpty(actual.Value);
        }

        [DebugOnlyFact]
        public async Task can_get_party_provinces()
        {
            IMemoryCache cache = _serviceProvider.GetRequiredService<IMemoryCache>();
            IDynamicsClient dynamicsClient = _serviceProvider.GetRequiredService<IDynamicsClient>();

            var actual = await dynamicsClient.GetPicklistOptionSetMetadataAsync("ssg_csrsparty", "ssg_provinceterritory", cache, CancellationToken.None);
            Assert.NotNull(actual);
            Assert.NotEmpty(actual.Value);
        }

        [DebugOnlyFact]
        public async Task can_get_party_identities()
        {
            IMemoryCache cache = _serviceProvider.GetRequiredService<IMemoryCache>();
            IDynamicsClient dynamicsClient = _serviceProvider.GetRequiredService<IDynamicsClient>();

            var actual = await dynamicsClient.GetPicklistOptionSetMetadataAsync("ssg_csrsparty", "ssg_identity", cache, CancellationToken.None);
            Assert.NotNull(actual);
            Assert.NotEmpty(actual.Value);
        }

        [DebugOnlyFact]
        public async Task can_get_child_is_a_dependent()
        {
            IMemoryCache cache = _serviceProvider.GetRequiredService<IMemoryCache>();
            IDynamicsClient dynamicsClient = _serviceProvider.GetRequiredService<IDynamicsClient>();

            var actual = await dynamicsClient.GetPicklistOptionSetMetadataAsync("ssg_csrschild", "ssg_childisadependent", cache, CancellationToken.None);
            Assert.NotNull(actual);
            Assert.NotEmpty(actual.Value);
        }
    }
}
