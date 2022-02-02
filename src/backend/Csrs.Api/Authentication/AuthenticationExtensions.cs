using Csrs.Api.Configuration;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace Microsoft.Extensions.DependencyInjection;

public static class AuthenticationExtensions
{
    private const string ConfigurationKey = "Jwt";

    public static void AddJwtBearerAuthentication(this WebApplicationBuilder builder)
    {
        builder.Services.Configure<JwtBearerOptions>(options => builder.Configuration.GetSection(ConfigurationKey).Bind(options));
        builder.Services.Configure<JwtAccessTokenConfiguration>(options => builder.Configuration.GetSection("JwtAccessToken").Bind(options));

        if (builder.Environment.IsDevelopment())
        {
            var options = new JwtBearerOptions();
            Bind(builder, options);
            if (string.IsNullOrEmpty(options.Audience) || string.IsNullOrEmpty(options.Authority))
            {
                return; // no configuration
            }
        }

        builder.Services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            Bind(builder, options);

            // update the MetadataAddress if needed
            options.MetadataAddress = options.GetMetadataAddress();

            //For docker keycloak which does not use ssl
            if  (options.GetMetadataAddress().Contains("http://")) { 
                options.RequireHttpsMetadata = false;
            }

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

    public static string? GetMetadataAddress(this JwtBearerOptions options)
    {
        // https://dev.oidc.gov.bc.ca/auth/realms/onestopauth-basic/.well-known/uma2-configuration
        if (string.IsNullOrEmpty(options?.MetadataAddress))
        {
            if (!string.IsNullOrEmpty(options?.Authority))
            {
                return options.Authority.EndsWith("/")
                    ? options.Authority + ".well-known/uma2-configuration"
                    : options.Authority + "/" + ".well-known/uma2-configuration";
            }
        }
        else
        {
            return options?.MetadataAddress;
        }

        return null;
    }

    /// <summary>
    /// Binds the Jwt configuration to the options
    /// </summary>
    /// <param name="builder"></param>
    /// <param name="options"></param>
    private static void Bind(WebApplicationBuilder builder, JwtBearerOptions options) => builder.Configuration.Bind(ConfigurationKey, options);
}
