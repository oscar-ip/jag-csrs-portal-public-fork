using Csrs.Api.Models;

namespace Csrs.Api.Services
{
    public class MessageService : IMessageService
    {
        public Task<IList<Message>> GetPartyMessages(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task SetMessageRead(Guid MessageGuid)
        {
            throw new NotImplementedException();
        }

    }
}
