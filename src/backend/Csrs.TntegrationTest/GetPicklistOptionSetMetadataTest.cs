using System.Threading;
using System.Threading.Tasks;
using Csrs.Interfaces.Dynamics;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using System.Collections.Generic;
using Csrs.Api.Models;
using System.Linq;

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


        [DebugOnlyFact]
        public async Task can_get_bccourtlevel()
        {
            IMemoryCache cache = _serviceProvider.GetRequiredService<IMemoryCache>();
            IDynamicsClient dynamicsClient = _serviceProvider.GetRequiredService<IDynamicsClient>();

            Interfaces.Dynamics.Models.MicrosoftDynamicsCRMssgCsrsbccourtlevelCollection actual = 
                await dynamicsClient.Ssgcsrsbccourtlevels.GetAsync(/*top:2,*/ cancellationToken: CancellationToken.None);

            
            
            Assert.NotNull(actual);
            Assert.NotEmpty(actual.Value);
        }

        [DebugOnlyFact]
        public async Task can_get_bccourtlocation()
        {
            IMemoryCache cache = _serviceProvider.GetRequiredService<IMemoryCache>();
            IDynamicsClient dynamicsClient = _serviceProvider.GetRequiredService<IDynamicsClient>();

            Interfaces.Dynamics.Models.MicrosoftDynamicsCRMssgIjssbccourtlocationCollection actual =
               await dynamicsClient.Ssgijssbccourtlocations.GetAsync();

            List<CourtLookupValue> courtLocations = new List<CourtLookupValue>();

            foreach (Interfaces.Dynamics.Models.MicrosoftDynamicsCRMssgIjssbccourtlocation location in actual.Value)
            {

                courtLocations.Add(new CourtLookupValue
                {
                    Id = location.SsgIjssbccourtlocationid,
                    Value = location.SsgBccourtlocationname
                });
            }

            List<LookupValue> courts = new List<LookupValue>();

            foreach (Interfaces.Dynamics.Models.MicrosoftDynamicsCRMssgIjssbccourtlocation location in actual.Value)
            {
                LookupValue item = courts.Where(x => x.Id == location.SsgCourtlevel.Value && x.Value == location.SsgBccourtlocationname).FirstOrDefault();
                if (item is null)
                {
                    courts.Add(new LookupValue
                    {
                        Id = location.SsgCourtlevel.HasValue ? location.SsgCourtlevel.Value : -1,
                        Value = location.SsgBccourtlocationname
                    });
                }
            }

            Assert.NotNull(actual);
            Assert.NotEmpty(actual.Value);
        }

    }
}
