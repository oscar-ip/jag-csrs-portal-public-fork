using Csrs.Api.Authentication;
using Csrs.Api.Configuration;
using Csrs.Api.Repositories;
using Simple.OData.Client;
using System.Configuration;

namespace Microsoft.Extensions.DependencyInjection;

public static class RepositoryExtensions
{
    /// <summary>
    /// Adds repository and dependant services.
    /// </summary>
    /// <param name="builder"></param>
    public static void AddRepositories(this WebApplicationBuilder builder)
    {
        var configuration = builder.Configuration.Get<CsrsConfiguration>();
        OAuthConfiguration? oAuthOptions = configuration?.OAuth;

        if (oAuthOptions is null || oAuthOptions.ResourceUrl is null)
        {
            throw new ConfigurationErrorsException("OAuth configuration is not set");
        }

        var services = builder.Services;

        services.AddSingleton(oAuthOptions);

        services.AddMemoryCache();

        // Add OAuth Middleware
        services.AddTransient<OAuthHandler>();

        // Register IOAuthApiClient and ODataClientSettings
        services.AddHttpClient<IOAuthApiClient, OAuthApiClient>();

        // Register httpClient for OdataClient with OAuthHandler
        services.AddHttpClient<ODataClientSettings>(cfg => { cfg.BaseAddress = new Uri(oAuthOptions.ResourceUrl); })
            .AddHttpMessageHandler<OAuthHandler>();

        services.AddTransient<IODataClient>(provider =>
        {
            var settings = provider.GetRequiredService<ODataClientSettings>();
            settings.IgnoreUnmappedProperties = true;
            return new ODataClient(settings);
        });

        // Add other Services
        services.AddTransient<ITokenService, TokenService>();

        services.AddTransient<ICsrsFileRepository, CsrsFileRepository>();
        services.AddTransient<ICsrsPartyRepository, CsrsPartyRepository>();
    }
}
