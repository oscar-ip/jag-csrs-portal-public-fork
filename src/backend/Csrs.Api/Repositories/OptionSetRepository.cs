using Csrs.Api.Models;
using Csrs.Api.Models.Dynamics.OptionSets;

namespace Csrs.Api.Services
{
    public class OptionSetRepository : IOptionSetRepository
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<OptionSetRepository> _logger;

        public OptionSetRepository(HttpClient httpClient, ILogger<OptionSetRepository> logger)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<IList<LookupValue>> GetStatusCodesAsync(string entityName, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(entityName))
            {
                throw new ArgumentException($"'{nameof(entityName)}' cannot be null or empty.", nameof(entityName));
            }

            using var scope = _logger.BeginScope(new Dictionary<string, string> { { "EntityLogicalName", entityName }, { "AttributeLogicalName", "statuscode" } });
            _logger.LogDebug("Getting status codes values for entity");

            using var request = new HttpRequestMessage { Method = HttpMethod.Get };
            request.Headers.Accept.Add(System.Net.Http.Headers.MediaTypeWithQualityHeaderValue.Parse("application/json"));
            request.RequestUri = CreateStatusCodesUri(entityName, "statuscode");

            var response = await _httpClient.SendAsync(request, cancellationToken);

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogInformation("Failed to get option values");
                return Array.Empty<LookupValue>();
            }

            var optionSetMetadata = await response.Content.ReadFromJsonAsync<OptionSetMetadata>(cancellationToken: cancellationToken);
            return optionSetMetadata.GetOptionValues().ToList();
        }

        public async Task<IList<LookupValue>> GetPickListValuesAsync(string entityName, string attributeName, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(entityName))
            {
                throw new ArgumentException($"'{nameof(entityName)}' cannot be null or empty.", nameof(entityName));
            }

            if (string.IsNullOrEmpty(attributeName))
            {
                throw new ArgumentException($"'{nameof(attributeName)}' cannot be null or empty.", nameof(attributeName));
            }

            using var scope = _logger.BeginScope(new Dictionary<string, string> { { "EntityLogicalName", entityName }, { "AttributeLogicalName", attributeName } });
            _logger.LogDebug("Getting picklist values for entity");

            using var request = new HttpRequestMessage { Method = HttpMethod.Get };
            request.Headers.Accept.Add(System.Net.Http.Headers.MediaTypeWithQualityHeaderValue.Parse("application/json"));
            request.RequestUri = CreatePicklistUri(entityName, attributeName);

            var response = await _httpClient.SendAsync(request, cancellationToken);

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogInformation("Failed to get option values");
                return Array.Empty<LookupValue>();
            }

            var content = response.Content.ReadAsStringAsync(cancellationToken: cancellationToken);

            var optionSetMetadata = await response.Content.ReadFromJsonAsync<PicklistOptionSetMetadata>(cancellationToken: cancellationToken);

            return optionSetMetadata.GetOptionValues().ToList();
        }

        private static Uri CreatePicklistUri(string entityName, string attributeName)
        {

            var text = $"EntityDefinitions(LogicalName='{entityName}')/Attributes/Microsoft.Dynamics.CRM.PicklistAttributeMetadata?$select=LogicalName&$filter=LogicalName eq '{attributeName}'&$expand=OptionSet";
            //var text = $"EntityDefinitions(LogicalName='{entityName}')/Attributes(LogicalName='{attributeName}')/Microsoft.Dynamics.CRM.PicklistAttributeMetadata?$select=LogicalName&$filter=LogicalName eq '{attributeName}'&$expand=OptionSet";

            return new Uri(text, UriKind.RelativeOrAbsolute);
        }

        private static Uri CreateStatusCodesUri(string entityName, string attributeName)
        {
            var text = $"EntityDefinitions(LogicalName='{entityName}')/Attributes(LogicalName='{attributeName}')/Microsoft.Dynamics.CRM.StatusAttributeMetadata?$select=LogicalName&$expand=OptionSet";
            return new Uri(text, UriKind.RelativeOrAbsolute);
        }
    }
}
