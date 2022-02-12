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
            string filter = $"substringof(ssg_bccourtlocationname, ‘Provincial’)";
            List<string> select = new List<string> { "ssg_ijssbccourtlocationid", "ssg_bccourtlocationname" };

            string cacheKey = "CourtLocations";

            if (!_cache.TryGetValue(cacheKey, out MicrosoftDynamicsCRMssgIjssbccourtlocationCollection locations))
            {
                locations = await _dynamicsClient.Ssgijssbccourtlocations.GetAsync(filter: filter, select: select, cancellationToken: cancellationToken);
                if (locations is not null && locations.Value is not null && locations.Value.Count != 0)
                {
                    _cache.Set(cacheKey, locations, TimeSpan.FromHours(1));
                }
            }

            IList<CourtLookupValue> courtLocatons = new List<CourtLookupValue>();

            foreach (MicrosoftDynamicsCRMssgIjssbccourtlocation location in locations.Value)
            {
                courtLocatons.Add(new CourtLookupValue
                {
                    Id = location.SsgIjssbccourtlocationid,
                    Value = location.SsgBccourtlocationname
                });
            }

            return courtLocatons;
        }
        public async Task<IList<CourtLookupValue>> GetCourtLevelsAsync(CancellationToken cancellationToken)
        {

            string filter = $"substringof(ssg_courtlevellabel, ‘Provincial’)";
            List<string> select = new List<string> { "ssg_csrsbccourtlevelid", "ssg_courtlevellabel" };

            string cacheKey = "CourtLevels";

            if (!_cache.TryGetValue(cacheKey, out MicrosoftDynamicsCRMssgCsrsbccourtlevelCollection levels))
            {
                levels = await _dynamicsClient.Ssgcsrsbccourtlevels.GetAsync(filter: filter, select: select, cancellationToken: cancellationToken);
                if (levels is not null && levels.Value is not null && levels.Value.Count != 0)
                {
                    _cache.Set(cacheKey, levels, TimeSpan.FromHours(1));
                }
            }

            IList<CourtLookupValue> courtLevels = new List<CourtLookupValue>();
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