using Csrs.Api.Models;

namespace Csrs.Api.Services
{
    public interface IMessageService
    {
        Task<IList<Message>> GetPartyMessages(CancellationToken cancellationToken);
        Task SetMessageRead(Guid MessageGuid);

    }
}
