using Csrs.Api.Models;
using Csrs.Interfaces.Dynamics;
using Microsoft.Extensions.Caching.Memory;

namespace Csrs.Api.Services
{
    public class MessageService : IMessageService
    {

        private readonly IMemoryCache _cache;
        private readonly IDynamicsClient _dynamicsClient;
        private readonly ILogger<MessageService> _logger;

        public MessageService(
            IMemoryCache cache,
            IDynamicsClient dynamicsClient,
            ILogger<MessageService> logger)
        {
            _cache = cache ?? throw new ArgumentNullException(nameof(cache));
            _dynamicsClient = dynamicsClient ?? throw new ArgumentNullException(nameof(dynamicsClient));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
        public Task<IList<Message>> GetPartyMessages(String partyId)
        {
            throw new NotImplementedException();
        }

        public Task SetMessageRead(Guid messageGuid)
        {
            throw new NotImplementedException();
        }

    }
}
