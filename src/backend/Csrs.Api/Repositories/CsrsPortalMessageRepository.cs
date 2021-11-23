using Csrs.Api.Models.Dynamics;
using Simple.OData.Client;

namespace Csrs.Api.Repositories
{
    public class CsrsPortalMessageRepository : Repository<SSG_CsrsPortalMessage>, ICsrsPortalMessageRepository
    {
        public CsrsPortalMessageRepository(IODataClient client) : base(client)
        {
        }

        public async Task MarkReadAsync(Guid id, CancellationToken cancellationToken)
        {
            Dictionary<string, object> updateFields = new();
            updateFields.Add(SSG_CsrsPortalMessage.Attributes.ssg_csrsmessageread, true);

            await Client
                .For<SSG_CsrsPortalMessage>()
                .Key(id)
                .Set(updateFields)
                .UpdateEntryAsync(cancellationToken);
        }
    }
}
