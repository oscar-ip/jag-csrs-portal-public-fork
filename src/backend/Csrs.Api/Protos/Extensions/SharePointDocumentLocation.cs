using Csrs.Interfaces.Dynamics;

namespace Csrs.Api.Extensions
{
    public static class SharePointDocumentLocation
    {
        /// <summary>
        /// Returns the SharePoint document Location for a given entity record
        /// </summary>
        /// <param name="dynamicsClient"></param>
        /// <param name="entityName"></param>
        /// <param name="entityId"></param>
        /// <returns></returns>
        public static async Task<string> GetEntitySharePointDocumentLocationAsync(this IDynamicsClient dynamicsClient, string entityName, string entityId, CancellationToken cancellationToken)
        {
            string result = null;
            if (!Guid.TryParse(entityId, out Guid id))
            {
                return result; // null
            }

            try
            {
                switch (entityName.ToLower())
                {
                    case "file":
                        var file = await dynamicsClient.GetFileForSharePointDocumentLocation(entityId, cancellationToken);
                        var fileLocation = file.SsgCsrsfileSharePointDocumentLocations?.FirstOrDefault();
                        if (fileLocation is not null && !string.IsNullOrEmpty(fileLocation.Relativeurl)) result = fileLocation.Relativeurl;
                        break;

                    // todo: other entity types
                }
            }
            catch (Exception)
            {

                throw;
            }

            return result;
        }
    }
}
