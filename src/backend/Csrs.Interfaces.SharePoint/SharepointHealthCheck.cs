using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Csrs.Interfaces
{
    public class SharepointHealthCheck : IHealthCheck
    // reference https://docs.microsoft.com/en-us/aspnet/core/host-and-deploy/health-checks?view=aspnetcore-2.2
    {
        private readonly IServiceProvider _serviceProvider;

        public SharepointHealthCheck(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
        }

        public Task<HealthCheckResult> CheckHealthAsync(
        HealthCheckContext context,
        CancellationToken cancellationToken = default(CancellationToken))
        {
            SharePointFileManager sharepoint = _serviceProvider.GetService<SharePointFileManager>(); ;
            // Try and get the Account document library
            bool healthCheckResultHealthy;
            try
            {
                var result = sharepoint.GetDocumentLibrary("Account").GetAwaiter().GetResult();

                healthCheckResultHealthy = (result != null);
            }
            catch (Exception)
            {
                healthCheckResultHealthy = false;
            }

            if (healthCheckResultHealthy)
            {
                return Task.FromResult(HealthCheckResult.Healthy("Sharepoint is healthy."));
            }

            return Task.FromResult(HealthCheckResult.Unhealthy("Sharepoint is unhealthy."));
        }
    }
}
