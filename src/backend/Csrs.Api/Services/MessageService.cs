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
        public async Task<IList<Message>> GetPartyMessages(string partyId, bool isSent, CancellationToken cancellationToken)
        {
            _logger.LogDebug("IsSent={IsSent}, Get party messages request received for PartyId={PartyId}", isSent, partyId);

            // CSRS-561 
            // DO NOT show outbox messages, there is a new requirement, messages need to be filtered, user should not be seeing agent messages
            // This if statement is to not show anything temporarily as we implement the filter feature
            if (isSent)
            {
                return new List<Message>();
            }

            try
            {
                MicrosoftDynamicsCRMssgCsrsfileCollection files = await _dynamicsClient.GetFilesByParty(partyId, isSent, cancellationToken);

                if (files == null || files.Value == null)
                {
                    _logger.LogWarning("IsSent={IsSent}, No files returned for PartyId={PartyId}", isSent, partyId);
                    return new List<Message>();
                }

                _logger.LogDebug("IsSent={IsSent}, Retrieved {FileCount} files for PartyId={PartyId}", isSent, files?.Value?.Count ?? 0, partyId);

                List<Message> messages = new List<Message>();
                foreach (var file in files.Value)
                {
                    if (file == null)
                    {
                        _logger.LogWarning("IsSent={IsSent}, Encountered null file object for PartyId={PartyId}", isSent, partyId);
                        continue;
                    }

                    _logger.LogDebug("IsSent={IsSent}, Processing FileId={FileId}", isSent, file.SsgCsrsfileid);

                    MicrosoftDynamicsCRMssgCsrscommunicationmessageCollection dynamicsMessages = await _dynamicsClient.GetCommunicationMessagesByFile(file.SsgCsrsfileid, partyId, isSent, cancellationToken);

                    if (dynamicsMessages == null || dynamicsMessages.Value == null)
                    {
                        _logger.LogWarning("IsSent={IsSent}, No communication messages found for FileId={FileId}, PartyId={PartyId}", isSent, file.SsgCsrsfileid, partyId);
                        continue;
                    }

                    _logger.LogDebug("IsSent={IsSent}, Retrieved {MessageCount} messages for FileId={FileId}", isSent, dynamicsMessages?.Value?.Count ?? 0, file.SsgCsrsfileid);

                    foreach (var message in dynamicsMessages.Value)
                    {

                        if (message == null)
                        {
                            _logger.LogWarning("IsSent={IsSent}, Encountered null message for FileId={FileId}, PartyId={PartyId}", isSent, file.SsgCsrsfileid, partyId);
                            continue;
                        }

                        _logger.LogDebug("IsSent={IsSent}, Processing MessageId={MessageId}, Subject={Subject}", isSent,
                            message.SsgCsrscommunicationmessageid,
                            message.SsgCsrsmessagesubject);

                        IList<FileSystemItem> attachments = new List<FileSystemItem>();

                        //message.SsgCsrsmessageattachment is not Set in Dynamics

                        //Get documents from fileManager
                        try
                        {
                            _logger.LogDebug(
                                "IsSent={IsSent}, Fetching attachments for MessageId={MessageId}, Entity=ssg_csrscommunicationmessage, Subject={Subject}",
                                isSent,
                                message.SsgCsrscommunicationmessageid,
                                message.SsgCsrsmessagesubject
                            );

                            attachments = await _documentService.GetAttachmentList(message.SsgCsrscommunicationmessageid, "ssg_csrscommunicationmessage", message.SsgCsrsmessagesubject, cancellationToken);

                            if (attachments == null || attachments.Count == 0)
                            {
                                _logger.LogDebug(
                                    "IsSent={IsSent}, No attachments found for MessageId={MessageId}, Subject={Subject}",
                                    isSent,
                                    message.SsgCsrscommunicationmessageid,
                                    message.SsgCsrsmessagesubject
                                );
                                attachments = Array.Empty<FileSystemItem>();
                            }
                            else
                            {
                                _logger.LogDebug(
                                    "IsSent={IsSent}, Retrieved {AttachmentCount} attachments for MessageId={MessageId}, Subject={Subject}",
                                    isSent,
                                    attachments.Count,
                                    message.SsgCsrscommunicationmessageid,
                                    message.SsgCsrsmessagesubject
                                );

                                if (_logger.IsEnabled(LogLevel.Debug))
                                {
                                    foreach (var attachment in attachments)
                                    {
                                        _logger.LogDebug(
                                            "IsSent={IsSent}, Attachment for MessageId={MessageId}:\nId={AttachmentId},\nName={AttachmentName},\nSize={AttachmentSize}",
                                            isSent,
                                            message.SsgCsrscommunicationmessageid,
                                            attachment.Id,
                                            attachment.Name,
                                            attachment.Size
                                        );
                                    }
                                }
                            }
                        }
                        catch (HttpOperationException ex)
                        {
                            _logger.LogDebug(
                                ex,
                                "IsSent={IsSent}, No Attachment Retrieved for MessageId={MessageId}, Subject={Subject}: {Message}",
                                isSent,
                                message.SsgCsrscommunicationmessageid,
                                message.SsgCsrsmessagesubject,
                                ex.Message
                            );
                            attachments = Array.Empty<FileSystemItem>();
                        }
                        catch (Exception ex)
                        {
                            _logger.LogError(
                                ex,
                                "IsSent={IsSent}, ERROR OCCURRED getting attachment list for MessageId={MessageId}, Subject={Subject}: {Message}", 
                                isSent,
                                message.SsgCsrscommunicationmessageid,
                                message.SsgCsrsmessagesubject,
                                ex.Message
                            );
                            attachments = Array.Empty<FileSystemItem>();
                        }

                        //Temporary add empty array of documents
                        messages.Add(ModelExtensions.ToViewModel(message, attachments));
                    }
                }

                _logger.LogDebug("IsSent={IsSent}, Returning {MessageCount} messages for PartyId={PartyId}", isSent, messages.Count, partyId);

                return messages;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "IsSent={IsSent}, Unhandled exception in GetPartyMessages for PartyId={PartyId}", isSent, partyId);

                throw; // rethrow exception stack
            }
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
