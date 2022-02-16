using Csrs.Api.Configuration;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Serilog;

namespace Microsoft.Extensions.DependencyInjection;

public static class AuthenticationExtensions
{
    private const string ConfigurationKey = "Jwt";

    public static void AddJwtBearerAuthentication(this WebApplicationBuilder builder)
    {
        builder.Services.Configure<JwtBearerOptions>(options => builder.Configuration.GetSection(ConfigurationKey).Bind(options));
        LogConfiguration(builder);

        builder.Services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            options.Events = new JwtBearerEvents
            {
                OnAuthenticationFailed = c =>
                {
                    var logger = Log.Logger;
                    if (c.Exception != null)
                    {
                        logger.Debug(c.Exception, "Authentication Failed Context: {@AuthenticationFailedContext}", c);
                    }
                    else
                    {
                        logger.Debug("Authentication Failed Context: {@AuthenticationFailedContext}", c);
                    }

                    c.NoResult();

                    c.Response.StatusCode = 401;
                    c.Response.ContentType = "text/plain";

                    if (builder.Environment.IsDevelopment() && c.Exception is not null)
                    {
                        return c.Response.WriteAsync(c.Exception.ToString());
                    }

                    return c.Response.WriteAsync("An error occured processing your authentication.");
                }
            };
        });
    }

    private static void LogConfiguration(WebApplicationBuilder builder)
    {
        var options = new JwtBearerOptions();
        Bind(builder, options);
        Serilog.ILogger logger = GetLogger();
        logger.Information("JWT Authentication settings {@JwtBearerOptions}", new { options.Audience, options.Authority, options.MetadataAddress });
    }

    /// <summary>
    /// Binds the Jwt configuration to the options
    /// </summary>
    /// <param name="builder"></param>
    /// <param name="options"></param>
    private static void Bind(WebApplicationBuilder builder, JwtBearerOptions options) => builder.Configuration.Bind(ConfigurationKey, options);

    /// <summary>
    /// Gets a logger for application setup.
    /// </summary>
    private static Serilog.ILogger GetLogger()
    {
        var logger = new LoggerConfiguration()
            .WriteTo.Console()            
            .WriteTo.Debug()            
            .MinimumLevel.Debug()
            .CreateLogger();

        return logger;
    }
}
