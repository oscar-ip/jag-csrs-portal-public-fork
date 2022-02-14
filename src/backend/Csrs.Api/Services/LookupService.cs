using Csrs.Api.Models;
using Csrs.Api.Repositories;
using Csrs.Interfaces.Dynamics;
using Csrs.Interfaces.Dynamics.Models;
using Microsoft.Extensions.Caching.Memory;

namespace Csrs.Api.Services
{
    public class LookupService : ILookupService
    {
        private readonly IMemoryCache _cache;
        private readonly IDynamicsClient _dynamicsClient;
        private readonly ILogger<LookupService> _logger;

        public LookupService(
            IMemoryCache cache,
            IDynamicsClient dynamicsClient,
            ILogger<LookupService> logger)
        {
            _cache = cache ?? throw new ArgumentNullException(nameof(cache));
            _dynamicsClient = dynamicsClient ?? throw new ArgumentNullException(nameof(dynamicsClient));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<IList<CourtLookupValue>> GetCourtLocationsAsync(CancellationToken cancellationToken)
        {
            MicrosoftDynamicsCRMssgIjssbccourtlocationCollection locations =
                new MicrosoftDynamicsCRMssgIjssbccourtlocationCollection();

            try
            {
                locations = await _dynamicsClient.Ssgijssbccourtlocations.GetAsync(cancellationToken: cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception is AccountService.GetCourtLocationsAsync");
                throw ex;
            }

            List<CourtLookupValue> courtLocations = new List<CourtLookupValue>();

            foreach (MicrosoftDynamicsCRMssgIjssbccourtlocation location in locations.Value)
            {
                CourtLookupValue item =
                    courtLocations.Where(x => x.Value == location.SsgBccourtlocationname).FirstOrDefault();

                if (item is null)
                {
                    courtLocations.Add(new CourtLookupValue
                    {
                        Id = location.SsgIjssbccourtlocationid,
                        Value = location.SsgBccourtlocationname
                    });
                }
            }
            return courtLocations;
        }
        public async Task<IList<CourtLookupValue>> GetCourtLevelsAsync(CancellationToken cancellationToken)
        {
            MicrosoftDynamicsCRMssgCsrsbccourtlevelCollection levels =
                new MicrosoftDynamicsCRMssgCsrsbccourtlevelCollection();

            try
            {
                levels = await _dynamicsClient.Ssgcsrsbccourtlevels.GetAsync(top: 2, cancellationToken: cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception is AccountService.GetCourtLevelsAsync");
                throw ex;
            }

            List<CourtLookupValue> courtLevels = new List<CourtLookupValue>();

            foreach (MicrosoftDynamicsCRMssgCsrsbccourtlevel level in levels.Value)
            {
                courtLevels.Add(new CourtLookupValue
                {
                    Id = level.SsgCsrsbccourtlevelid,
                    Value = level.SsgCourtlevellabel
                });
            }
            return courtLevels;
        }
    }
}