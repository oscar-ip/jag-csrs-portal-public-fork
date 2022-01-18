using Csrs.Api;
using Csrs.Api.Authentication;
using Csrs.Api.Configuration;
using Csrs.Api.Models;
using Csrs.Api.Models.Dynamics;
using Csrs.Api.Repositories;
using Csrs.Api.Services;
using Csrs.Api.ApiGateway;
using Simple.OData.Client;
using System.Configuration;
using Grpc.Net.Client;
using Grpc.Core;
using Grpc.Net.Client.Configuration;
using Serilog;

namespace Microsoft.Extensions.DependencyInjection;

public static class WebApplicationBuilderExtensions
{
    /// <summary>
    /// Adds repository and dependant services.
    /// </summary>
    /// <param name="builder"></param>
    public static void AddServices(this WebApplicationBuilder builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        Serilog.ILogger logger = GetLogger();

        var configuration = builder.Configuration.Get<CsrsConfiguration>();
        OAuthConfiguration? oAuthOptions = configuration?.OAuth;

        if (string.IsNullOrEmpty(oAuthOptions?.ResourceUrl))
        {
            const string message = "OAuth configuration is not set";
            logger.Error(message);
            throw new ConfigurationErrorsException(message);
        }

        ApiGatewayOptions? apiGatewayOptions = configuration?.ApiGateway;
        if (string.IsNullOrEmpty(apiGatewayOptions?.BasePath))
        {
            const string message = "ApiGateWay configuration is not set";
            logger.Error(message);
            throw new ConfigurationErrorsException(message);
        }

        var services = builder.Services;

        logger.Debug("Setting up oAuthOptions and apiGatewayOptions");
        services.AddSingleton(oAuthOptions);
        services.AddSingleton(apiGatewayOptions);

        logger.Debug("Adding memory cache");
        services.AddMemoryCache();

        // Add OAuth Middleware
        services.AddTransient<OAuthHandler>();
        // Add ApiGateway Middleware
        services.AddTransient<ApiGatewayHandler>();

        // Register IOAuthApiClient and ODataClientSettings
        services.AddHttpClient<IOAuthApiClient, OAuthApiClient>(client =>
        {
            client.Timeout = TimeSpan.FromSeconds(15); // set the auth timeout
        });

        // Register httpClient for OdataClient with OAuthHandler
        services.AddHttpClient<ODataClientSettings>(client => 

        {
            client.BaseAddress = new Uri(apiGatewayOptions.BasePath);
            client.Timeout = TimeSpan.FromSeconds(30); // data timeout
        })
        .AddHttpMessageHandler<OAuthHandler>()
        .AddHttpMessageHandler<ApiGatewayHandler>();

        logger.Debug("Configuing IOptionSetRepository");
        services.AddHttpClient<IOptionSetRepository, OptionSetRepository>(client =>
        {
            client.BaseAddress = new Uri(apiGatewayOptions.BasePath);
            client.Timeout = TimeSpan.FromSeconds(110); // options timeout

        })
        .AddHttpMessageHandler<OAuthHandler>()
        .AddHttpMessageHandler<ApiGatewayHandler>();

        logger.Debug("Configuing FileManager Service");
        ConfigureFileManagerService(builder, configuration?.FileManager, logger);

        services.AddTransient<IODataClient>(provider =>
        {
            var settings = provider.GetRequiredService<ODataClientSettings>();
            settings.IgnoreUnmappedProperties = true;
            return new ODataClient(settings);
        });

        services.AddHttpContextAccessor();

        // Add services
        services.AddTransient<ITokenService, TokenService>();

        services.AddTransient<IAccountService, AccountService>();
        services.AddTransient<IFileService, FileService>();
        services.AddTransient<IUserService, UserService>();
        services.AddTransient<IChildService, ChildService>();

        // Add repositories
        services.AddTransient<ICourtLevelRepository, CourtLevelRepository>();
        services.AddTransient<ICourtLocationRepository, CourtLocationRepository>();

        services.AddTransient<ICsrsChildRepository, CsrsChildRepository>();
        services.AddTransient<ICsrsFeedbackRepository, CsrsFeedbackRepository>();
        services.AddTransient<ICsrsFileRepository, CsrsFileRepository>();
        services.AddTransient<ICsrsPartyRepository, CsrsPartyRepository>();

        // mappers
        services.AddTransient<IInsertOrUpdateFieldMapper<Csrs.Api.Models.File, SSG_CsrsFile>, FileInsertOrUpdateFieldMapper>();
        services.AddTransient<IInsertOrUpdateFieldMapper<Party, SSG_CsrsParty>, PartyInsertOrUpdateFieldMapper>();
    }

    private static void ConfigureFileManagerService(WebApplicationBuilder builder, FileManagerConfiguration? configuration, Serilog.ILogger logger)
    {
        if (string.IsNullOrWhiteSpace(configuration?.Address))
        {
            const string message = $"FileManager configuration is not set, {nameof(CsrsConfiguration.FileManager)}:{nameof(FileManagerConfiguration.Address)} is required.";
            logger.Error(message);
            throw new ConfigurationErrorsException(message);
        }

        string address = configuration.Address;

        // determine if we are using http or https
        ChannelCredentials credentials = ChannelCredentials.Insecure;

        bool? secure = configuration.Secure;
        if (secure.HasValue && secure.Value)
        {
            logger.Information("Using secure channel for File Manager service");
            credentials = ChannelCredentials.SecureSsl;
        }
        else
        {
            logger.Information("Using insecure channel for File Manager service");
            credentials = ChannelCredentials.Insecure;
        }

        logger.Information("Using file manager service {Address}", address);

        builder.Services.AddSingleton(services =>
        {
            var channel = GrpcChannel.ForAddress(address, new GrpcChannelOptions
            {
                Credentials = credentials,
                ServiceConfig = new ServiceConfig { LoadBalancingConfigs = { new RoundRobinConfig() } },
                ServiceProvider = services
            });

            return channel;
        });

        builder.Services.AddTransient(services =>
        {
            GrpcChannel channel = services.GetRequiredService<GrpcChannel>();
            return new Csrs.Services.FileManager.FileManager.FileManagerClient(channel);
        });
    }

    /// <summary>
    /// Gets a logger for application setup.
    /// </summary>
    private static Serilog.ILogger GetLogger()
    {
        var logger = new LoggerConfiguration()
            .WriteTo.Console()
            .WriteTo.Debug()
            .CreateLogger();

        return logger;
    }
}
