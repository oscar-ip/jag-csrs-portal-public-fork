using Csrs.Api.Models.Dynamics;
using Csrs.Api.Services;

namespace Csrs.Api
{
    public class StartupHostedService : IHostedService
    {
        private readonly TimeSpan AllowedInitializeTime = TimeSpan.FromMinutes(1);
        private readonly TimeSpan RetryDelay = TimeSpan.FromSeconds(1);

        private readonly IHostApplicationLifetime _applicationLifetime;
        private readonly IOptionSetRepository _optionSetRepository;
        private readonly ILogger<StartupHostedService> _logger;

        public StartupHostedService(IHostApplicationLifetime applicationLifetime, IOptionSetRepository optionSetRepository, ILogger<StartupHostedService> logger)
        {
            _applicationLifetime = applicationLifetime ?? throw new ArgumentNullException(nameof(applicationLifetime));
            _optionSetRepository = optionSetRepository;
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            // The service will not be ready until this task is complete

            // we must load the status codes for all our entities otherwise this api
            // cannot query or update
            DateTime tryUntil = DateTime.UtcNow.Add(AllowedInitializeTime);

            while (!IsInitialized && DateTime.UtcNow <= tryUntil)
            {
                _logger.LogDebug("Initializing required entity status codes");

                bool success = true;

                success = success && await InitializeCsrsPartyStatusCodes(cancellationToken);
                success = success && await InitializeCsrsFileStatusCodes(cancellationToken);

                if (!success)
                {
                    _logger.LogInformation("One or more entity StatusCodes lookups did not initialize correctly, will retry in one second");
                    await Task.Delay(RetryDelay);
                }
            }

            if (!IsInitialized)
            {
                _logger.LogWarning("Cound not initialize one or more entity StatusCodes lookups, stopping the application");
                _applicationLifetime.StopApplication();
            }
        }

        public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;

        private bool IsInitialized => SSG_CsrsParty.StatusCodes.Initialized && SSG_CsrsFile.StatusCodes.Initialized;

        private async Task<bool> InitializeCsrsPartyStatusCodes(CancellationToken cancellationToken)
        {
            if (!SSG_CsrsParty.StatusCodes.Initialized)
            {
                try
                {
                    await SSG_CsrsParty.StatusCodes.InitializeAsync(_optionSetRepository, cancellationToken);
                    return true;
                }
                catch (Exception ex)
                {
                    _logger.LogInformation(ex, "Error initializing {EntityLogicalName} StatusCodes", SSG_CsrsParty.EntityLogicalName);
                    return false;
                }
            }

            return true;
        }

        private async Task<bool> InitializeCsrsFileStatusCodes(CancellationToken cancellationToken)
        {
            if (!SSG_CsrsFile.StatusCodes.Initialized)
            {
                try
                {
                    await SSG_CsrsFile.StatusCodes.InitializeAsync(_optionSetRepository, cancellationToken);
                    return true;
                }
                catch (Exception ex)
                {
                    _logger.LogInformation(ex, "Error initializing {EntityLogicalName} StatusCodes", SSG_CsrsFile.EntityLogicalName);
                    return false;
                }
            }

            return true;
        }
    }
}
