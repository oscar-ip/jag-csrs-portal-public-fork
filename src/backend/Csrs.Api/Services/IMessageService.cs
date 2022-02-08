using Csrs.Api.Models;

namespace Csrs.Api.Services
{
    public interface IMessageService
    {
        Task<IList<Message>> GetPartyMessages(string partyId);
        Task SetMessageRead(string messageGuid);

    }
}
