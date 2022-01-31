using Csrs.Interfaces.Dynamics;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Csrs.TntegrationTest
{
    public class GetFileTest : DynamicsClientTestBase
    {
        [DebugOnlyFact]
        public async Task get_last_5_modified_files()
        {
            IDynamicsClient dynamicsClient = _serviceProvider.GetRequiredService<IDynamicsClient>();

            List<string> select = new List<string> { "ssg_csrsfileid" };
            List<string> orderby = new List<string> { "modifiedon desc" };

            var actual = await dynamicsClient.Ssgcsrsfiles.GetAsync(top: 5, select: select, orderby: orderby, cancellationToken: CancellationToken.None);
            Assert.NotNull(actual);
            Assert.NotEmpty(actual.Value);
        }

        [DebugOnlyFact]
        public async Task get_file_by_payor()
        {
            IDynamicsClient dynamicsClient = _serviceProvider.GetRequiredService<IDynamicsClient>();

            string filter = $"_ssg_payor_value eq f418274f-b10f-ea11-b814-00505683fbf4";
            List<string> expand = new List<string> { "ssg_Payor", "ssg_Recipient", "ssg_csrsfile_ssg_csrschild_ChildsFileNumber" };

            var actual = await dynamicsClient.Ssgcsrsfiles.GetAsync(top: 5, filter: filter, expand: expand, cancellationToken: CancellationToken.None);
            Assert.NotNull(actual);
            Assert.NotEmpty(actual.Value);
        }

        [DebugOnlyFact]
        public async Task get_file_by_recipient()
        {
            IDynamicsClient dynamicsClient = _serviceProvider.GetRequiredService<IDynamicsClient>();

            string filter = $"_ssg_recipient_value eq ca571bc8-5b11-ea11-b814-00505683fbf4";
            List<string> expand = new List<string> { "ssg_Payor", "ssg_Recipient", "ssg_csrsfile_ssg_csrschild_ChildsFileNumber" };

            var actual = await dynamicsClient.Ssgcsrsfiles.GetAsync(top: 5, filter: filter, expand: expand, cancellationToken: CancellationToken.None);
            Assert.NotNull(actual);
            Assert.NotEmpty(actual.Value);
        }
    }
}
