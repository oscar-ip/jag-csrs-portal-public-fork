using Csrs.Interfaces.Dynamics;
using Csrs.Interfaces.Dynamics.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Rest;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Csrs.TntegrationTest
{
    public class GetFileTest : DynamicsClientTestBase
    {
        [DebugOnlyFact]
        public async Task get_entity_that_does_not_exist_throws_HttpOperationException_with_response_status_code_of_NotFound()
        {
            IDynamicsClient dynamicsClient = _serviceProvider.GetRequiredService<IDynamicsClient>();

            var actual = await Assert.ThrowsAsync<Microsoft.Rest.HttpOperationException>(async () =>
                {
                    string key = Guid.NewGuid().ToString("d");
                    List<string> select = new List<string> { "ssg_csrsfileid" };
                    await dynamicsClient.Ssgcsrsfiles.GetByKeyAsync(key, select: select, cancellationToken: CancellationToken.None);
                });

            Assert.NotNull(actual);
            Assert.Equal(HttpStatusCode.NotFound, actual.Response.StatusCode);
        }


        [DebugOnlyFact]
        public async Task get_last_5_modified_files()
        {
            IDynamicsClient dynamicsClient = _serviceProvider.GetRequiredService<IDynamicsClient>();

            List<string> select = new List<string> { "ssg_csrsfileid" };
            List<string> orderby = new List<string> { "createdon desc" };
            List<string> expand = new List<string> { "createdby", "modifiedby", "ownerid" };

            var actual = await dynamicsClient.Ssgcsrsfiles.GetAsync(top: 5, select: select, orderby: orderby, expand: expand, cancellationToken: CancellationToken.None);
            Assert.NotNull(actual);
            Assert.NotEmpty(actual.Value);
        }

        [DebugOnlyFact]
        public async Task get_file_by_file_number()
        {
            string fileNumber = "1118";

            IDynamicsClient dynamicsClient = _serviceProvider.GetRequiredService<IDynamicsClient>();

            string filter = $"ssg_filenumber eq '{fileNumber}'";
            var actual = await dynamicsClient.Ssgcsrsfiles.GetAsync(filter:filter, cancellationToken: CancellationToken.None);
            Assert.NotNull(actual);
            Assert.NotEmpty(actual.Value);
        }

        [DebugOnlyFact]
        public async Task get_last_5_modified_files_payor_and_recipient()
        {
            IDynamicsClient dynamicsClient = _serviceProvider.GetRequiredService<IDynamicsClient>();

            List<string> select = new List<string> { "ssg_csrsfileid", "_ssg_payor_value", "_ssg_recipient_value" };
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

        [DebugOnlyFact]
        public async Task create_new_file()
        {
            IDynamicsClient dynamicsClient = _serviceProvider.GetRequiredService<IDynamicsClient>();

            MicrosoftDynamicsCRMssgCsrsfile file = new MicrosoftDynamicsCRMssgCsrsfile();

            Assert.Null(file.SsgCsrsfileid);

            file = await dynamicsClient.Ssgcsrsfiles.CreateAsync(file);
            Assert.NotNull(file);
            Assert.NotNull(file.SsgCsrsfileid);

            await dynamicsClient.Ssgcsrsfiles.DeleteAsync(file.SsgCsrsfileid);
        }

        [DebugOnlyFact]
        public async Task delete_file()
        {
            IDynamicsClient dynamicsClient = _serviceProvider.GetRequiredService<IDynamicsClient>();
            try
            {
                await dynamicsClient.Ssgcsrsfiles.DeleteAsync("9351f6c3-b082-ec11-b831-00505683fbf4");
            }
            catch (HttpOperationException exception) when (exception.Response.StatusCode == HttpStatusCode.NotFound)
            {
                // dont fail if the requested file is not found
            }
        }
    }
}
