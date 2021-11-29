using Csrs.Api.Models;

namespace Csrs.Api.Services
{
    /// <summary>
    /// Defines an interface to get status codes or picket list values.
    /// </summary>
    public interface IOptionSetRepository
    {
        /// <summary>
        /// Gets the status code values for an entity.
        /// </summary>
        /// <param name="entityName"></param>
        /// <param name="cancellationToken"></param>
        /// <exception cref="ArgumentException"><paramref name="entityName"/> is null or empty.</exception>
        /// <returns></returns>
        Task<IList<OptionValue>> GetStatusCodesAsync(string entityName, CancellationToken cancellationToken);

        /// <summary>
        /// Get the pick list values for an entity attribute.
        /// </summary>
        /// <param name="entityName"></param>
        /// <param name="attributeName"></param>
        /// <param name="cancellationToken"></param>
        /// <exception cref="ArgumentException"><paramref name="entityName"/> or <paramref name="attributeName"/> is null or empty.</exception>
        /// <returns></returns>
        Task<IList<OptionValue>> GetPickListValuesAsync(string entityName, string attributeName, CancellationToken cancellationToken);
    }
}
