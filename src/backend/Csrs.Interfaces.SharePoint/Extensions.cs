using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Csrs.Interfaces
{
    public static class Extensions
    {
        /// <summary>
        /// Adds the requires services to be able to use SharePointFileManager.
        /// </summary>
        /// <param name="services"></param>
        public static void AddSharePointIntegration(this IServiceCollection services)
        {
            services.AddSingleton(GetSharePointFileManagerConfiguration);
            services.AddScoped<SharePointFileManager>();

            // SAML services
            // token caches are singleton because they maintain a per instance prefix
            // that can be changed to effectively clear the cache
            services.AddSingleton<ITokenCache<SamlTokenParameters, string>, SamlTokenTokenCache>();
            services.AddTransient<ISamlAuthenticator, SamlAuthenticator>();

            services.AddMemoryCache();
        }

        private static SharePointFileManagerConfiguration GetSharePointFileManagerConfiguration(IServiceProvider serviceProvider)
        {
            IConfiguration configuration = serviceProvider.GetService<IConfiguration>();

            var sharePointFileManagerConfiguration = new SharePointFileManagerConfiguration
            {
                ApiGatewayHost = configuration["APIGATEWAY_HOST"],
                ApiGatewayPolicy = configuration["APIGATEWAY_POLICY"],
                RelyingPartyIdentifier = configuration["RELYING_PARTY_IDENTIFIER"],
                AuthorizationUri = new Uri(configuration["AUTHORIZATION_URI"]),
                Resource = new Uri(configuration["RESOURCE"]),
                Username = configuration["SHAREPOINT_USERNAME"],
                Password = configuration["SHAREPOINT_PASSWORD"],
            };

            return sharePointFileManagerConfiguration;
        }
    }
}
