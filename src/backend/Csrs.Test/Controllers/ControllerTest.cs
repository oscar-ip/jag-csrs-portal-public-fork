using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace Csrs.Test.Controllers
{
    public abstract class ControllerTest<TController> where TController : ControllerBase
    {
        protected Mock<ILogger<TController>> GetMockLogger(bool strict = false) => new Mock<ILogger<TController>>(strict ? MockBehavior.Strict : MockBehavior.Default);
        protected Mock<IMediator> GetMockMediator(bool strict = false) => new Mock<IMediator>(strict ? MockBehavior.Strict : MockBehavior.Default);
    }
}
