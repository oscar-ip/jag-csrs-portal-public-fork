using Microsoft.Extensions.DependencyInjection;
using System.Net.Http.Json;
using Csrs.Interfaces.Dynamics.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Rest;
using System.Net;

namespace Csrs.Interfaces.Dynamics;

public partial class DynamicsClient
{
    private readonly ILogger<DynamicsClient> _logger;

    [ActivatorUtilitiesConstructor]
    public DynamicsClient(HttpClient httpClient, ILogger<DynamicsClient> logger)
    {
        Initialize();

        HttpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        _logger = logger ?? throw new ArgumentNullException(nameof(httpClient));

        BaseUri = httpClient.BaseAddress;
    }

    public async Task<PicklistOptionSetMetadata> GetPicklistOptionSetMetadataAsync(string entityName, string attributeName, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(entityName);
        ArgumentNullException.ThrowIfNull(attributeName);

        // Tracing
        bool shouldTrace = ServiceClientTracing.IsEnabled;
        string invocationId = null;
        if (shouldTrace)
        {
            invocationId = ServiceClientTracing.NextInvocationId.ToString();
            Dictionary<string, object> tracingParameters = new Dictionary<string, object>();
            tracingParameters.Add("cancellationToken", cancellationToken);
            ServiceClientTracing.Enter(invocationId, this, "GetPicklistOptionSetMetadata", tracingParameters);
        }

        if (string.IsNullOrEmpty(entityName))
        {
            throw new ArgumentException($"'{nameof(entityName)}' cannot be null or empty.", nameof(entityName));
        }

        if (string.IsNullOrEmpty(attributeName))
        {
            throw new ArgumentException($"'{nameof(attributeName)}' cannot be null or empty.", nameof(attributeName));
        }

        using var request = new HttpRequestMessage { Method = HttpMethod.Get };
        request.Headers.Accept.Add(System.Net.Http.Headers.MediaTypeWithQualityHeaderValue.Parse("application/json"));
        request.RequestUri = CreatePicklistUri(entityName, attributeName);

        var response = await HttpClient.SendAsync(request, cancellationToken);
        HttpStatusCode statusCode = response.StatusCode;

        string content = null;
        if (!response.IsSuccessStatusCode)
        {
            var ex = new HttpOperationException(string.Format("Operation returned an invalid status code '{0}'", statusCode));
            if (response.Content != null)
            {
                content = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            } else
            {
                content = string.Empty;
            }

            ex.Request = new HttpRequestMessageWrapper(request, string.Empty);
            ex.Response = new HttpResponseMessageWrapper(response, content);
            if (shouldTrace)
            {
                ServiceClientTracing.Error(invocationId, ex);
            }

            request.Dispose();
            response?.Dispose();

            using var scope = _logger.BeginScope(new Dictionary<string, object> { { "HttpStatusCode", statusCode } });
            _logger.LogInformation("Failed to fetch picklist options for {EntityName}/{AttributeName}", entityName, attributeName);

            throw ex;
        }

        var metadata = await response.Content.ReadFromJsonAsync<PicklistOptionSetMetadata>(cancellationToken: cancellationToken);

        if (shouldTrace)
        {
            ServiceClientTracing.Exit(invocationId, metadata);
        }

        return metadata;
    }

    private static Uri CreatePicklistUri(string entityName, string attributeName)
    {
        var text = $"EntityDefinitions(LogicalName='{entityName}')/Attributes/Microsoft.Dynamics.CRM.PicklistAttributeMetadata?$select=LogicalName&$filter=LogicalName eq '{attributeName}'&$expand=OptionSet";
        //var text = $"EntityDefinitions(LogicalName='{entityName}')/Attributes(LogicalName='{attributeName}')/Microsoft.Dynamics.CRM.PicklistAttributeMetadata?$select=LogicalName&$filter=LogicalName eq '{attributeName}'&$expand=OptionSet";

        return new Uri(text, UriKind.RelativeOrAbsolute);
    }

    public string GetEntityURI(string entityType, string id)
    {
        ArgumentNullException.ThrowIfNull(entityType);
        ArgumentNullException.ThrowIfNull(id);

        if (!Guid.TryParse(id, out Guid key))
        {
            throw new FormatException("Entity id is not a valid guid");
        }

        string uri = BaseUri + entityType + "(" + key.ToString("d") + ")";
        return uri;
    }
}
