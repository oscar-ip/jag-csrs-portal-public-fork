using Csrs.Api.Features.Accounts;
using Csrs.Api.Models;
using Csrs.Api.Services;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Net;

namespace Csrs.Api.Controllers
{
    public class AccountController : CsrsControllerBase<AccountController>
    {
        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService, IMediator mediator, ILogger<AccountController> logger)
            : base(mediator, logger)
        {
            _accountService = accountService ?? throw new ArgumentNullException(nameof(accountService));
        }

        /// <summary>
        /// Gets the valid gender values for an account.
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet("Genders")]
        [ProducesResponseType(typeof(IList<LookupValue>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        public async Task<IActionResult> GetGendersAsync(CancellationToken cancellationToken)
        {            
            IList<LookupValue>? values = await _accountService.GetGendersAsync(cancellationToken);
            return Ok(values);
        }

        /// <summary>
        /// Gets the valid province values for an account.
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet("Provinces")]
        [ProducesResponseType(typeof(IList<LookupValue>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        public async Task<IActionResult> GetProvincesAsync(CancellationToken cancellationToken)
        {
            IList<LookupValue>? values = await _accountService.GetProvincesAsync(cancellationToken);
            return Ok(values);
        }

        /// <summary>
        /// Gets the valid identity values for an account.
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet("Identities")]
        [ProducesResponseType(typeof(IList<LookupValue>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        public async Task<IActionResult> GetIdentitesAsync(CancellationToken cancellationToken)
        {
            IList<LookupValue>? values = await _accountService.GetIdentitiesAsync(cancellationToken);
            return Ok(values);
        }

        /// <summary>
        /// Gets the valid referral values for an account.
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet("Referrals")]
        [ProducesResponseType(typeof(IList<LookupValue>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        public async Task<IActionResult> GetReferralsAsync(CancellationToken cancellationToken)
        {
            IList<LookupValue>? values = await _accountService.GetReferralsAsync(cancellationToken);
            return Ok(values);
        }

        /// <summary>
        /// Gets the preferred contact methods values for an account.
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet("PreferredContactMethods")]
        [ProducesResponseType(typeof(IList<LookupValue>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        public async Task<IActionResult> GetPreferredContactMethodsAsync(CancellationToken cancellationToken)
        {
            IList<LookupValue>? values = await _accountService.GetPreferredContactMethodsAsync(cancellationToken);
            return Ok(values);
        }

        /// <summary>
        /// Gets the current user's file summary.
        /// </summary>
        /// <response code="200">The user and their files were found.</response>
        /// <response code="401">The request is not authorized. Ensure correct authentication header is present.</response>
        /// <response code="404">The user was not found</response>
        [HttpGet]
        [ProducesResponseType(typeof(AccountFileSummary), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        public async Task<IActionResult> GetAsync(CancellationToken cancellationToken)
        {
            Profile.Request request = new(User);
            Profile.Response? response = await _mediator.Send(request, cancellationToken);

            return response != Profile.Response.Empty ? Ok(response.AccountFileFileSummary) : NotFound();
        }

        /// <summary>
        /// Creates a new file.
        /// </summary>
        /// <returns></returns>
        /// <response code="200">The user and file were created.</response>
        /// <response code="400">The request was not well formed.</response>
        /// <response code="401">The request is not authorized. Ensure correct authentication header is present.</response>
        /// <response code="409">There is already a draft file associated with the current user/account.</response>
        [HttpPost("Create")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType((int)HttpStatusCode.Conflict)]
        public async Task<IActionResult> CreateAsync([Required] NewFileRequest newFileRequest, CancellationToken cancellationToken)
        {
            if (newFileRequest == null || newFileRequest.User == null || newFileRequest.File == null)
            {
                return BadRequest();
            }

            NewAccountAndFile.Request request = new(newFileRequest.User, newFileRequest.File);
            NewAccountAndFile.Response? response = await _mediator.Send(request, cancellationToken);

            return Ok(response);
        }

    }
}
