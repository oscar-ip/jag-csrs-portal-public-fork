using System;
using Csrs.Services.FileManager.OpenShiftIntegration;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

namespace Microsoft.AspNetCore.Hosting;

public static class OpenShiftWebHostBuilderExtensions
{
    public static WebApplicationBuilder UseOpenShiftIntegration(this WebApplicationBuilder builder, Action<OpenShiftIntegrationOptions> configureOptions)
    {
        ArgumentNullException.ThrowIfNull(builder);
        ArgumentNullException.ThrowIfNull(configureOptions);

        if (PlatformEnvironment.IsOpenShift)
        {
            // Clear the urls. We'll explicitly configure Kestrel depending on the options.
            builder.WebHost.UseUrls();

            builder.WebHost.ConfigureServices(services =>
            {
                services.Configure(configureOptions);
                services.AddSingleton<OpenShiftCertificateLoader>();
                services.AddSingleton<IConfigureOptions<KestrelServerOptions>, KestrelOptionsSetup>();
                services.AddSingleton<IHostedService, OpenShiftCertificateExpiration>();
            });

        }

        return builder;
    }
}
