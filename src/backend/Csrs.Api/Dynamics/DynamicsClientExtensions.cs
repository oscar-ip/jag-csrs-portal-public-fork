using Csrs.Interfaces.Dynamics.Models;
using Microsoft.Extensions.Caching.Memory;

namespace Csrs.Interfaces.Dynamics;

public static class DynamicsClientExtensions
{
    public static async Task<PicklistOptionSetMetadata> GetPicklistOptionSetMetadataAsync(
        this IDynamicsClient dynamicsClient,
        string entityName,
        string attributeName,
        IMemoryCache cache,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(dynamicsClient);
        ArgumentNullException.ThrowIfNull(entityName);
        ArgumentNullException.ThrowIfNull(attributeName);
        ArgumentNullException.ThrowIfNull(cache);

        string cacheKey = $"{entityName}-{attributeName}-Picklist";

        if (!cache.TryGetValue(cacheKey, out PicklistOptionSetMetadata metadata))
        {
            metadata = await dynamicsClient.GetPicklistOptionSetMetadataAsync(entityName, attributeName, cancellationToken);
            if (metadata is not null && metadata.Value is not null && metadata.Value.Count != 0)
            {
                cache.Set(cacheKey, metadata, TimeSpan.FromHours(1));
            }
        }

        if (metadata is null)
        {
            metadata = new PicklistOptionSetMetadata();
        }

        if (metadata.Value is null)
        {
            metadata.Value = new List<OptionSetMetadata>();
        }

        return metadata;
    }
}
