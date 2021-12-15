using Csrs.Api.Features.Lookups;
using Csrs.Api.Models.Dynamics;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Csrs.Api.Controllers
{
    public class LookupController : CsrsControllerBase<LookupController>
    {
        public LookupController(IMediator mediator, ILogger<LookupController> logger) : base(mediator, logger)
        {
        }

        [HttpGet("CourtLevels")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<IList<SSG_CsrsBCCourtLevel>> GetCourtLevelsAsync(CancellationToken cancellationToken)
        {
            var response = await _mediator.Send(CourtLevels.Request.Default, cancellationToken);

            return response.Items;
        }

        [HttpGet("CourtLocations")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<IList<SSG_IJSSBCCourtlocation>> GetCourtLocationsAsync(CancellationToken cancellationToken)
        {
            var response = await _mediator.Send(CourtLocations.Request.Default, cancellationToken);

            return response.Items;
        }
    }
}
