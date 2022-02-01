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

    public static async Task<MicrosoftDynamicsCRMssgCsrsfile> GetFileForSharePointDocumentLocation(this IDynamicsClient dynamicsClient, string id, CancellationToken cancellationToken)
    {
        // get only the required fields for working with Sharepoint
        List<string> select = new List<string> { "ssg_csrsfileid" };
        List<string> expand = new List<string> { "ssg_csrsfile_SharePointDocumentLocations" };

        var entity = await dynamicsClient.Ssgcsrsfiles.GetByKeyAsync(id, select: select, expand: expand, cancellationToken: cancellationToken);

        return entity;
    }
}
