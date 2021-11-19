using Csrs.Api.Controllers;
using System;
using Xunit;

namespace Csrs.Test.Controllers
{
    public class PortalUserRequestControllerTest : ControllerTest<PortalUserRequestController>
    {
        [Fact]
        public void CanCreateController()
        {
            var logger = GetMockLogger();
            var mediator = GetMockMediator();
            new PortalUserRequestController(mediator.Object, logger.Object);
        }

        [Fact]
        public void ControllerConstructorChecksParameters()
        {
            var logger = GetMockLogger();
            var mediator = GetMockMediator();

            Assert.Throws<ArgumentNullException>(() => new PortalUserRequestController(null, logger.Object));
            Assert.Throws<ArgumentNullException>(() => new PortalUserRequestController(mediator.Object, null));
        }
    }
}