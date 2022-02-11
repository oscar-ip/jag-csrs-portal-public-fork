using Csrs.Interfaces.Dynamics;
using Csrs.Interfaces.Dynamics.Models;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Csrs.TntegrationTest
{
    public class GetSharepointDocumentTest : DynamicsClientTestBase
    {
        [DebugOnlyFact]
        public async Task get_all_sharepoint_document_locations()
        {
            IDynamicsClient dynamicsClient = _serviceProvider.GetRequiredService<IDynamicsClient>();


            var actual = await dynamicsClient.Sharepointdocumentlocations.GetAsync();
            Assert.NotNull(actual);
            Assert.NotEmpty(actual.Value);
        }
    }
}
