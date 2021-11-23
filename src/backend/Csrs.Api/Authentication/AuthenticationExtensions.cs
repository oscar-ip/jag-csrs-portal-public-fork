using Csrs.Api.Configuration;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Configuration;

namespace Microsoft.Extensions.DependencyInjection;

public static class AuthenticationExtensions
{
    public static void AddJwtBearerAuthentication(this WebApplicationBuilder builder)
    {
        var configuration = builder.Configuration.Get<CsrsConfiguration>()?.Jwt;

        if (configuration is null || configuration.Authority is null || string.IsNullOrWhiteSpace(configuration.Audience))
        {
            if (!builder.Environment.IsDevelopment())
            {
                // better error message
                throw new ConfigurationErrorsException("Jwt is not configured correctly");
            }
            else
            {
                return; // for development, no auth if not configured?
            }
        }

        builder.Services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            string audience = configuration.Audience;
            string authority = configuration.Authority.ToString();

            if (!authority.EndsWith("/", StringComparison.InvariantCulture))
            {
                authority += "/";
            }

                // KeyCloak metadata address https://www.keycloak.org/docs/latest/authorization_services/index.html
                string metadataAddress = authority + ".well-known/uma2-configuration";

            options.Authority = authority;
            options.Audience = audience;
            options.MetadataAddress = metadataAddress;

            options.Events = new JwtBearerEvents
            {
                OnAuthenticationFailed = c =>
                {
                    c.NoResult();

                    c.Response.StatusCode = 401;
                    c.Response.ContentType = "text/plain";

                    if (builder.Environment.IsDevelopment())
                    {
                        return c.Response.WriteAsync(c.Exception.ToString());
                    }

                    return c.Response.WriteAsync("An error occured processing your authentication.");
                }
            };
        });
    }
}
