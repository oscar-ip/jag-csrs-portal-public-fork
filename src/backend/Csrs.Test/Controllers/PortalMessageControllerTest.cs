using Csrs.Api.Controllers;
using Xunit;

namespace Csrs.Test.Controllers
{
    public class PortalMessageControllerTest : ControllerTest<MessageController>
    {
        [Fact]
        public void CanCreateController()
        {
            var logger = GetMockLogger();
            var mediator = GetMockMediator();
            new MessageController(mediator.Object, logger.Object);
        }
    }
}