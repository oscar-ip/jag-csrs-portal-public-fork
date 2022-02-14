using Csrs.Interfaces.Dynamics.Models;

namespace Csrs.Interfaces.Dynamics;

public partial interface IDynamicsClient
{
    Task<PicklistOptionSetMetadata> GetPicklistOptionSetMetadataAsync(string entityName, string attributeName, CancellationToken cancellationToken);

    /// <summary>
    /// Gets the entity reference.
    /// </summary>
    /// <param name="entityType"></param>
    /// <param name="id"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException">if <paramref name="entityType"/> or <paramref name="id"/> is null.</exception>
    /// <exception cref="FormatException"><paramref name="id"/> cannot be parsed as a <see cref="System.Guid"/></exception>
    string GetEntityURI(string entityType, string id);

    /// <summary>
    /// Gets the entity reference for a SSG CSRS BC Court Level
    /// </summary>
    /// <param name="entity">The entity to get reference for</param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"><paramref name="entity"/> or <see cref="MicrosoftDynamicsCRMssgCsrsbccourtlevel.SsgCsrsbccourtlevelid"/> is null.</exception>
    /// <exception cref="FormatException"><see cref="MicrosoftDynamicsCRMssgCsrsbccourtlevel.SsgCsrsbccourtlevelid"/> cannot be parsed as a <see cref="System.Guid"/></exception>
    string GetEntityReference(MicrosoftDynamicsCRMssgCsrsbccourtlevel entity) => GetEntityURI("ssg_csrsbccourtlevels", entity?.SsgCsrsbccourtlevelid!);

    /// <summary>
    /// Gets the entity reference for a SSG CSRS File
    /// </summary>
    /// <param name="entity">The entity to get reference for</param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"><paramref name="entity"/> or <see cref="MicrosoftDynamicsCRMssgCsrsfile.SsgCsrsfileid"/> is null.</exception>
    /// <exception cref="FormatException"><see cref="MicrosoftDynamicsCRMssgCsrsfile.SsgCsrsfileid"/> cannot be parsed as a <see cref="System.Guid"/></exception>
    string GetEntityReference(MicrosoftDynamicsCRMssgCsrsfile entity) => GetEntityURI("ssg_csrsfiles", entity?.SsgCsrsfileid!);
    string GetEntityReference(FileId entity) => GetEntityURI("ssg_csrsfiles", entity.ToString());

    /// <summary>
    /// Gets the entity reference for a SSG CSRS Party
    /// </summary>
    /// <param name="entity">The entity to get reference for</param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"><paramref name="entity"/> or <see cref="MicrosoftDynamicsCRMssgCsrsparty.SsgCsrspartyid"/> is null.</exception>
    /// <exception cref="FormatException"><see cref="MicrosoftDynamicsCRMssgCsrsparty.SsgCsrspartyid"/> cannot be parsed as a <see cref="System.Guid"/></exception>
    string GetEntityReference(MicrosoftDynamicsCRMssgCsrsparty entity) => GetEntityURI("ssg_csrsparties", entity?.SsgCsrspartyid!);
    string GetEntityReference(PartyId entity) => GetEntityURI("ssg_csrsparties", entity.ToString());

    /// <summary>
    /// Gets the entity reference for a SSG CSRS BC Court Location
    /// </summary>
    /// <param name="entity">The entity to get reference for</param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"><paramref name="entity"/> or <see cref="MicrosoftDynamicsCRMssgIjssbccourtlocation.SsgIjssbccourtlocationid"/> is null.</exception>
    /// <exception cref="FormatException"><see cref="MicrosoftDynamicsCRMssgIjssbccourtlocation.SsgIjssbccourtlocationid"/> cannot be parsed as a <see cref="System.Guid"/></exception>
    string GetEntityReference(MicrosoftDynamicsCRMssgIjssbccourtlocation entity) => GetEntityURI("ssg_ijssbccourtlocations", entity?.SsgIjssbccourtlocationid!);
}
