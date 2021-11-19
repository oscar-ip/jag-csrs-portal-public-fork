using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Csrs.Api.Controllers
{
    public abstract class CsrsControllerBase<TController> : ControllerBase
    {
        protected readonly IMediator _mediator;
        private readonly ILogger<TController> _logger;

        protected CsrsControllerBase(IMediator mediator, ILogger<TController> logger)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
    }
}
