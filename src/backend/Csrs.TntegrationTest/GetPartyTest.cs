using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Csrs.Api.Repositories;
using Csrs.Interfaces.Dynamics;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Csrs.TntegrationTest
{
    public class GetPartyTest : DynamicsClientTestBase
    {
        [DebugOnlyFact]
        public async Task get_party()
        {
            IDynamicsClient dynamicsClient = _serviceProvider.GetRequiredService<IDynamicsClient>();

            string filter = $"ssg_bceid_userid ne null";
            //string filter = $"ssg_csrspartyid eq 'fc7702c0-0d89-ec11-b831-00505683fbf4'";
            List<string> expand = new List<string> { "createdby" };
            var actual = await dynamicsClient.Ssgcsrsparties.GetAsync(top: 5, filter: filter, expand: expand, cancellationToken: CancellationToken.None);
            Assert.NotNull(actual);
        }

        [DebugOnlyFact]
        public async Task get_party_by_bceid()
        {
            IDynamicsClient dynamicsClient = _serviceProvider.GetRequiredService<IDynamicsClient>();
            //var actual = await dynamicsClient.GetPartyByBCeIdAsync("f418274f-b10f-ea11-b814-00505683fbf4", cancellationToken: CancellationToken.None);
            var actual = await dynamicsClient.GetPartyByBCeIdAsync("5beb7384-eb6d-4fd4-8918-c3bb1a5c", cancellationToken: CancellationToken.None);
            Assert.NotNull(actual);
        }
    }
}
