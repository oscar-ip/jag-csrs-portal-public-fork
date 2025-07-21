using Csrs.Api.Models;

namespace Csrs.Api.Services
{
    public interface IMessageService
    {
        Task<IList<Message>> GetPartyMessages(string partyId, bool isSent, CancellationToken cancellationToken);
        Task SetMessageRead(string messageGuid, CancellationToken cancellationToken);

    }
}
