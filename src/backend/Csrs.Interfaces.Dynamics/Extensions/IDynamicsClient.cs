using Csrs.Interfaces.Dynamics.Models;

namespace Csrs.Interfaces.Dynamics;

public partial interface IDynamicsClient
{
    Task<PicklistOptionSetMetadata> GetPicklistOptionSetMetadataAsync(string entityName, string attributeName, CancellationToken cancellationToken);
}
