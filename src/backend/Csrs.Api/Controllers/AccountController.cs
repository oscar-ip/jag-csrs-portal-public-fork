using Csrs.Api.Features.Accounts;
using Csrs.Api.Models;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Net;

namespace Csrs.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AccountController : CsrsControllerBase<AccountController>
    {
        public AccountController(IMediator mediator, ILogger<AccountController> logger)
            : base(mediator, logger)
        {
        }

        /// <summary>
        /// Gets the valid gender values for an account.
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet("Genders")]
        [ProducesResponseType(typeof(IList<OptionValue>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetGendersAsync(CancellationToken cancellationToken)
        {
            Lookups.Response? response = await _mediator.Send(Lookups.Request.Gender, cancellationToken);
            return Ok(response.Items);
        }

        /// <summary>
        /// Gets the valid province values for an account.
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet("Provinces")]
        [ProducesResponseType(typeof(IList<OptionValue>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetProvincesAsync(CancellationToken cancellationToken)
        {
            Lookups.Response? response = await _mediator.Send(Lookups.Request.Province, cancellationToken);
            return Ok(response.Items);
        }

        /// <summary>
        /// Gets the valid identity values for an account.
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet("Identities")]
        [ProducesResponseType(typeof(IList<OptionValue>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetIdentitesAsync(CancellationToken cancellationToken)
        {
            Lookups.Response? response = await _mediator.Send(Lookups.Request.Identity, cancellationToken);
            return Ok(response.Items);
        }

        /// <summary>
        /// Gets the valid referral values for an account.
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet("Referrals")]
        [ProducesResponseType(typeof(IList<OptionValue>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetReferralsAsync(CancellationToken cancellationToken)
        {
            Lookups.Response? response = await _mediator.Send(Lookups.Request.Referral, cancellationToken);
            return Ok(response.Items);
        }

        /// <summary>
        /// Gets the profile for the currently logged in user.
        /// </summary>
        /// <response code="200">The account was found</response>
        /// <response code="401">The request is not authorized. Ensure correct authentication header is present.</response>
        /// <response code="404">The account was not found</response>
        [HttpGet]
        [ProducesResponseType(typeof(PortalAccount), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        public async Task<IActionResult> GetProfileAsync(CancellationToken cancellationToken)
        {
            Profile.Request request = new(User);
            Profile.Response? response = await _mediator.Send(request, cancellationToken);

            return response.Account != null ? Ok(response.Account) : NotFound();
        }

        [HttpPost("Signup")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<string> SignupAsync([Required] PortalAccount account)
        {
            Signup.Request request = new(account);
            Signup.Response? response = await _mediator.Send(request);
            return response.Id.ToString();
        }

        [HttpPatch("UpdateProfile")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<string> UpdateProfileAsync([Required]PortalAccount account)
        {
            UpdateProfile.Request request = new UpdateProfile.Request(account);
            UpdateProfile.Response? response = await _mediator.Send(request);
            return response.Account.PartyId.ToString();
        }
    }
}
