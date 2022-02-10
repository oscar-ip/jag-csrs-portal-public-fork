using System;
using Csrs.Api.ApiGateway;
using Csrs.Api.Authentication;
using Csrs.Api.Configuration;
using Csrs.Interfaces.Dynamics;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Csrs.TntegrationTest
{
    public abstract class DynamicsClientTestBase
    {
        protected ServiceProvider _serviceProvider;

        protected DynamicsClientTestBase()
        {
            ConfigurationManager configuration = new ConfigurationManager();
            configuration.AddUserSecrets<DynamicsClientTestBase>(true);

            var csrsConfiguration = configuration.Get<CsrsConfiguration>();

            IServiceCollection services = new ServiceCollection();
            services.AddSingleton<OAuthConfiguration>(csrsConfiguration.OAuth);
            services.AddSingleton<ApiGatewayOptions>(csrsConfiguration.ApiGateway);

            services.AddMemoryCache();
            // Add OAuth Middleware
            services.AddTransient<OAuthHandler>();
            // Add ApiGateway Middleware
            services.AddTransient<ApiGatewayHandler>();

            // Register IOAuthApiClient and ODataClientSettings
            services.AddHttpClient<IOAuthApiClient, OAuthApiClient>(client =>
            {
                //client.Timeout = TimeSpan.FromSeconds(15); // set the auth timeout
                client.Timeout = TimeSpan.FromSeconds(150); 
            });

            services.AddHttpClient<IDynamicsClient, DynamicsClient>(client =>
            {
                //client.BaseAddress = new Uri(csrsConfiguration.ApiGateway.BasePath);
                //client.Timeout = TimeSpan.FromSeconds(30); // data timeout
                client.BaseAddress = new Uri(csrsConfiguration.OAuth.ResourceUrl);
                client.Timeout = TimeSpan.FromSeconds(30); // data timeout
            })
            .AddHttpMessageHandler<OAuthHandler>()
            .AddHttpMessageHandler<ApiGatewayHandler>();

            services.AddTransient<ITokenService, TokenService>();


            _serviceProvider = services.BuildServiceProvider();
        }
    }
}
