using Csrs.Api.Models;
using Csrs.Api.Repositories;
using Csrs.Interfaces.Dynamics;
using Csrs.Interfaces.Dynamics.Models;
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
        public async Task<IList<Message>> GetPartyMessages(string partyId)
        {

            _logger.LogDebug("Get party messages request recieved");

            MicrosoftDynamicsCRMssgCsrsfileCollection files = await _dynamicsClient.GetFilesByParty(partyId);

            List<Message> messages = new List<Message>();
            //This is inefficient. This may work better if we query only communication messages on Part To and Party From fields
            foreach (var file in files.Value.ToList())
            {
                MicrosoftDynamicsCRMssgCsrscommunicationmessageCollection dynamicsMessages = await _dynamicsClient.GetCommunicationMessagesByFile(file.SsgCsrsfileid);

                foreach (var message in dynamicsMessages.Value.ToList())
                {
                    //TODO get attachment meta from fileManager
                    //Temporary add empty array of documents
                    messages.Add(ModelExtensions.ToMessage(message, new List<Document>()));
                }

            }

            return messages;

        }

        public async Task SetMessageRead(string messageGuid)
        {

            _logger.LogDebug("Set party message read request recieved");

            throw new NotImplementedException();
        }

    }
}
