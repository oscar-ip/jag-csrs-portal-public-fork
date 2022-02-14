using Csrs.Api.Models;
using Csrs.Interfaces.Dynamics;
using Csrs.Interfaces.Dynamics.Models;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Csrs.TntegrationTest
{
    public class GetCommunicationMessageTest : DynamicsClientTestBase
    {
        [DebugOnlyFact]
        public async Task get_last_5_communication_messages_by_file_id()
        {
            IDynamicsClient dynamicsClient = _serviceProvider.GetRequiredService<IDynamicsClient>();
            string filter = string.Format("_ssg_csrsfile_value eq {0}", "20dfca66-656d-ea11-b818-00505683fbf4");
            List<string> select = new List<string> { "_ssg_csrsfile_value", "ssg_sentreceiveddate", "ssg_csrsmessage", "ssg_csrsmessageattachment", "ssg_csrsmessageread", "ssg_csrsmessagesubject", "statuscode", "_ssg_toparty_value" };
            List<string> orderby = new List<string> { "modifiedon desc" };

            //var actual = await dynamicsClient.Ssgcsrscommunicationmessages.GetAsync(select: select, orderby: orderby, filter: filter, cancellationToken: CancellationToken.None);

            MicrosoftDynamicsCRMssgCsrscommunicationmessageCollection dynamicsMessages = await dynamicsClient.Ssgcsrscommunicationmessages.GetAsync(select: select, orderby: orderby, filter: filter, cancellationToken: CancellationToken.None);

            List<Message> messages = new List<Message>();

            foreach (var message in dynamicsMessages.Value.ToList())
            {
                //TODO get attachment meta from fileManager
                //Temporary add empty array of documents
                messages.Add(ModelExtensions.ToViewModel(message, new List<Document>()));
            }

            Assert.NotNull(dynamicsMessages);
            Assert.NotNull(messages);
            Assert.NotEmpty(dynamicsMessages.Value);
        }

        [DebugOnlyFact]
        public async Task get_last_5_get_files_by_party()
        {
            IDynamicsClient dynamicsClient = _serviceProvider.GetRequiredService<IDynamicsClient>();

            string filter = string.Format("_ssg_payor_value eq {0} or _ssg_recipient_value eq {0}", "481019e4-7939-ea11-b814-00505683fbf4");
            List<string> select = new List<string> { "ssg_csrsfileid" };
            List<string> orderby = new List<string> { "modifiedon desc" };

            var actual = await dynamicsClient.Ssgcsrsfiles.GetAsync(top: 10, select: select, orderby: orderby, filter: filter, expand: null, cancellationToken: CancellationToken.None);
            Assert.NotNull(actual);
            Assert.NotEmpty(actual.Value);

        }

    }
}
