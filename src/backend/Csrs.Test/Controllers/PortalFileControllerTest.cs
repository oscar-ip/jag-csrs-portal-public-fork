using Csrs.Api.Controllers;
using Xunit;

namespace Csrs.Test.Controllers
{
    public class PortalFileControllerTest : ControllerTest<FileController>
    {
        [Fact]
        public void CanCreateController()
        {
            var logger = GetMockLogger();
            var mediator = GetMockMediator();
            new FileController(mediator.Object, logger.Object);
        }
    }
}