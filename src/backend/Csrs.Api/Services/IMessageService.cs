using Csrs.Api.Models;

namespace Csrs.Api.Services
{
    public interface IMessageService
    {
        Task<IList<Message>> GetPartyMessages(String partyId);
        Task SetMessageRead(Guid messageGuid);

    }
}
