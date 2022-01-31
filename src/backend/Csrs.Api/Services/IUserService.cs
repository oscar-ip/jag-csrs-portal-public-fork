namespace Csrs.Api.Services
{
    /// <summary>
    /// Provides access to current user's information.
    /// </summary>
    public interface IUserService
    {
        /// <summary>
        /// Gets the BCeID Guid from the current user.
        /// </summary>
        /// <returns>The user's BCeID Guid or <see cref="Guid.Empty"/> if not set.</returns>
        string GetBCeIDUserId();
    }
}
