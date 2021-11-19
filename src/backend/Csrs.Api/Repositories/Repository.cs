using Simple.OData.Client;

namespace Csrs.Api.Repositories
{
    public abstract class Repository
    {
        protected readonly IODataClient Client;
        
        protected Repository(IODataClient client)
        {
            Client = client ?? throw new ArgumentNullException(nameof(client));
        }
    }
}
