using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Csrs.Api.Controllers
{
    [ApiController]
    //[Authorize]
    [Route("api/[controller]")]
    public abstract class CsrsControllerBase<TController> : ControllerBase
    {
        protected readonly IMediator _mediator;
        protected readonly ILogger<TController> _logger;

        protected CsrsControllerBase(IMediator mediator, ILogger<TController> logger)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
    }
}
