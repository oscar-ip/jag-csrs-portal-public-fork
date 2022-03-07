using Csrs.Api.Models;
using Csrs.Api.Repositories;
using Csrs.Interfaces.Dynamics;
using Csrs.Interfaces.Dynamics.Models;
using Microsoft.Rest;
using Microsoft.Extensions.Caching.Memory;
using Csrs.Services.FileManager;

namespace Csrs.Api.Services
{
    public class MessageService : IMessageService
    {

        private readonly IDynamicsClient _dynamicsClient;
        private readonly IDocumentService _documentService;
        private readonly ILogger<MessageService> _logger;

        public MessageService(
            IDynamicsClient dynamicsClient,
            IDocumentService documentService,
            ILogger<MessageService> logger)
        {
            _dynamicsClient = dynamicsClient ?? throw new ArgumentNullException(nameof(dynamicsClient));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _documentService = documentService ?? throw new ArgumentNullException(nameof(documentService));
        }
        public async Task<IList<Message>> GetPartyMessages(string partyId, CancellationToken cancellationToken)
        {

            _logger.LogDebug("Get party messages request recieved");

            MicrosoftDynamicsCRMssgCsrsfileCollection files = await _dynamicsClient.GetFilesByParty(partyId, cancellationToken);

            List<Message> messages = new List<Message>();
            foreach (var file in files.Value)
            {
                MicrosoftDynamicsCRMssgCsrscommunicationmessageCollection dynamicsMessages = await _dynamicsClient.GetCommunicationMessagesByFile(file.SsgCsrsfileid, partyId, cancellationToken);

                foreach (var message in dynamicsMessages.Value)
                {
                    IList<FileSystemItem> attachments = new List<FileSystemItem>();

                    //message.SsgCsrsmessageattachment is not Set in Dynamics
                    
                    //Get documents from fileManager
                    try
                    {
                        attachments = await _documentService.GetAttachmentList(message.SsgCsrscommunicationmessageid, "ssg_csrscommunicationmessage", message.SsgCsrsmessagesubject, cancellationToken);
                    }catch (HttpOperationException ex)
                    {
                        _logger.LogInformation($"No Attachment Retrieved for Message {message.SsgCsrscommunicationmessageid} "+ex.Message);
                        attachments = Array.Empty<FileSystemItem>();
                    }catch (Exception ex)
                    {
                        _logger.LogInformation($"ERROR OCCURED getting attachment list for message {message.SsgCsrscommunicationmessageid} " + ex.Message);
                        attachments = Array.Empty<FileSystemItem>();
                    }
                    
                    //Temporary add empty array of documents
                    messages.Add(ModelExtensions.ToViewModel(message, attachments));
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
