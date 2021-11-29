using Csrs.Api.Controllers;
using System;
using Xunit;

namespace Csrs.Test.Controllers
{
    public class PortalMessageControllerTest : ControllerTest<PortalMessageController>
    {
        [Fact]
        public void CanCreateController()
        {
            var logger = GetMockLogger();
            var mediator = GetMockMediator();
            new PortalMessageController(mediator.Object, logger.Object);
        }
    }
}