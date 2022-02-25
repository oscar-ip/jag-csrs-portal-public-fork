using Csrs.Api;
using Csrs.Api.Authentication;
using Csrs.Api.Configuration;
using Csrs.Api.Models;
using Csrs.Api.Services;
using Csrs.Api.ApiGateway;
using System.Configuration;
using Grpc.Net.Client;
using Grpc.Core;
using Grpc.Net.Client.Configuration;
using Serilog;
using Csrs.Interfaces.Dynamics;
using System.Net;

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

        // Register IOAuthApiClient
        services.AddHttpClient<IOAuthApiClient, OAuthApiClient>(client =>
        {
            client.Timeout = TimeSpan.FromSeconds(15); // set the auth timeout
        });
        services.AddSingleton(new DynamicsClientOptions { NativeOdataResourceUrl = oAuthOptions.ResourceUrl });
        services.AddHttpClient<IDynamicsClient, DynamicsClient>(client =>
        {

            client.BaseAddress = new Uri(apiGatewayOptions.BasePath);
            client.Timeout = TimeSpan.FromSeconds(30); // data timeout
            //client.BaseAddress = new Uri(oAuthOptions.ResourceUrl);
            //client.Timeout = TimeSpan.FromSeconds(300); // data timeout

        })
        .AddHttpMessageHandler<OAuthHandler>()
        .AddHttpMessageHandler<ApiGatewayHandler>();

        logger.Debug("Configuing FileManager Service");
        ConfigureFileManagerService(builder, configuration?.FileManager, logger);

        services.AddHttpContextAccessor();

        // Add services
        services.AddTransient<ITokenService, TokenService>();
        services.AddTransient<IMessageService, MessageService>();
        services.AddTransient<IAccountService, AccountService>();
        services.AddTransient<IFileService, FileService>();
        services.AddTransient<IUserService, UserService>();
        services.AddTransient<ILookupService, LookupService>();
        services.AddTransient<IDocumentService, DocumentService>();
        services.AddTransient<ITaskService, TaskService>();

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
        ChannelCredentials credentials;

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
        AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", true);
        builder.Services.AddSingleton(services =>
        {
            var httpHandler = new HttpClientHandler();
            // Return "true" to allow certificates that are untrusted/invalid
            if (false)
            {
                httpHandler.ServerCertificateCustomValidationCallback =
                    HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;
            }

            var httpClient = new HttpClient(httpHandler);
            // set default request version to HTTP 2.  Note that Dotnet Core does not currently respect this setting for all requests.
            httpClient.DefaultRequestVersion = HttpVersion.Version20;

            var channel = GrpcChannel.ForAddress(address, new GrpcChannelOptions
            {
                Credentials = credentials,
                ServiceConfig = new ServiceConfig { LoadBalancingConfigs = { new RoundRobinConfig() } },
                ServiceProvider = services,
                HttpClient = httpClient

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
