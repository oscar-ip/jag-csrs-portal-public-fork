using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Csrs.Api.Health
{
    public static class HealthCheckExtensions
    {
        /// <summary>
        /// Adds the health check endpoints.
        /// </summary>
        /// <param name="app"></param>
        public static void AddHealthChecks(this IApplicationBuilder app)
        {
            app.UseEndpoints(endpoints =>
            {
                // this endpoint returns HTTP 200 if all "liveness" checks have passed, otherwise, it returns HTTP 500
                endpoints.MapHealthChecks("/self", new HealthCheckOptions()
                {
                    Predicate = registration => registration.Tags.Contains(HealthCheckType.Liveness)
                });

                // this endpoint returns HTTP 200 if all "readiness" checks have passed, otherwise, it returns HTTP 500
                endpoints.MapHealthChecks("/ready", new HealthCheckOptions()
                {
                    Predicate = registration => registration.Tags.Contains(HealthCheckType.Readiness)
                });
            });
        }

        /// <summary>
        /// Adds the liveness and readiness health checks for the application.
        /// </summary>
        /// <param name="builder"></param>
        public static void AddHealthChecks(this WebApplicationBuilder builder)
        {
            builder.Services.AddHealthChecks()
                .AddCheck("self", () => HealthCheckResult.Healthy(), tags: new[] { HealthCheckType.Liveness })
                // TODO: change to add various readiness checks
                .AddCheck("ready", () => HealthCheckResult.Healthy(), tags: new[] { HealthCheckType.Readiness })
                ;
        }
    }
}
