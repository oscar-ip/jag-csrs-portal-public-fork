using Csrs.Api.Models;
using Csrs.Api.Services;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Csrs.Api.Controllers
{
    public class LookupController : CsrsControllerBase<LookupController>
    {
        private readonly ILookupService _lookupService;

        public LookupController(ILookupService lookupService, IMediator mediator, ILogger<LookupController> logger)
            : base(mediator, logger)
        {
            _lookupService = lookupService ?? throw new ArgumentNullException(nameof(lookupService));
        }

        /// <summary>
        /// Gets the valid court level values.
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet("CourtLevels")]
        [ProducesResponseType(typeof(IList<CourtLookupValue>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        public async Task<IActionResult> GetCourtLevelsAsync(CancellationToken cancellationToken)
        {
            IList<CourtLookupValue>? values = await _lookupService.GetCourtLevelsAsync(cancellationToken);
            return Ok(values);
        }

        /// <summary>
        /// Gets the valid court locations values.
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet("CourtLocations")]
        [ProducesResponseType(typeof(IList<CourtLookupValue>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        public async Task<IActionResult> GetCourtLocationsAsync(CancellationToken cancellationToken)
        {
            IList<CourtLookupValue>? values = await _lookupService.GetCourtLocationsAsync(cancellationToken);
            return Ok(values);
        }
    }
}
