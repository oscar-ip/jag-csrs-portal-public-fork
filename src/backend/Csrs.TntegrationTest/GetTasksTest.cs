using Csrs.Interfaces.Dynamics;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Csrs.TntegrationTest
{
    public class GetTasksTest : DynamicsClientTestBase
    {
        [DebugOnlyFact]
        public async Task get_latest_tasks()
        {
            IDynamicsClient dynamicsClient = _serviceProvider.GetRequiredService<IDynamicsClient>();
            
            List<string> orderby = new List<string> { "createdon desc" };
            List<string> expand = new List<string> { "createdby", "modifiedby", "ownerid", "owninguser" };

            var actual = await dynamicsClient.Tasks.GetAsync(top: 10, orderby: orderby, expand: expand);

            Assert.NotNull(actual);

        }

    }
}
