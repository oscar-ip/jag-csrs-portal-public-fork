using Csrs.Api.Models;
using Csrs.Api.Repositories;
using Csrs.Interfaces.Dynamics;
using Csrs.Interfaces.Dynamics.Models;
using Microsoft.Rest;
using Microsoft.Extensions.Caching.Memory;

namespace Csrs.Api.Services
{
    public class MessageService : IMessageService
    {

        private readonly IDynamicsClient _dynamicsClient;
        private readonly ILogger<MessageService> _logger;

        public MessageService(
            IDynamicsClient dynamicsClient,
            ILogger<MessageService> logger)
        {
            _dynamicsClient = dynamicsClient ?? throw new ArgumentNullException(nameof(dynamicsClient));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
        public async Task<IList<Message>> GetPartyMessages(string partyId, CancellationToken cancellationToken)
        {

            _logger.LogDebug("Get party messages request recieved");

            MicrosoftDynamicsCRMssgCsrsfileCollection files = await _dynamicsClient.GetFilesByParty(partyId, cancellationToken);

            List<Message> messages = new List<Message>();
            //This is inefficient. This may work better if we query only communication messages on Part To and Party From fields
            foreach (var file in files.Value)
            {
                MicrosoftDynamicsCRMssgCsrscommunicationmessageCollection dynamicsMessages = await _dynamicsClient.GetCommunicationMessagesByFile(file.SsgCsrsfileid, cancellationToken);

                foreach (var message in dynamicsMessages.Value)
                {
                    //TODO get attachment meta from fileManager
                    if (message.SsgCsrsmessageattachment is not null && message.SsgCsrsmessageattachment.Value)
                    {
                        //Get documents from fileManager
                    }
                    //Temporary add empty array of documents
                    messages.Add(ModelExtensions.ToViewModel(message, new List<Document>()));
                }

            }

            return messages;

        }

        public async Task SetMessageRead(string messageGuid, CancellationToken cancellationToken)
        {
            _logger.LogDebug("Set party message read request recieved");

            var select = new List<string>() {"ssg_csrsmessageread"};

            var communicationMessage = await _dynamicsClient.Ssgcsrscommunicationmessages.GetByKeyAsync(messageGuid, select, null, cancellationToken);
            if (communicationMessage is null) {
                _logger.LogInformation("No associated message Guid, cannot set message to read");
                throw new HttpOperationException("Incorrect message Guid");
            }

            if (communicationMessage.SsgCsrsmessageread == false) {
                communicationMessage.SsgCsrsmessageread = true;

                await _dynamicsClient.Ssgcsrscommunicationmessages.UpdateAsync(messageGuid, communicationMessage, cancellationToken);
            }

        }

    }
}
