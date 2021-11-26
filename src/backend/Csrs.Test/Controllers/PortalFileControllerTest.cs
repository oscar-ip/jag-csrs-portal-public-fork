using Csrs.Api.Controllers;
using System;
using Xunit;

namespace Csrs.Test.Controllers
{
    public class PortalFileControllerTest : ControllerTest<PortalFileController>
    {
        [Fact]
        public void CanCreateController()
        {
            var logger = GetMockLogger();
            var mediator = GetMockMediator();
            new PortalFileController(mediator.Object, logger.Object);
        }

        [Fact]
        public void ControllerConstructorChecksParameters()
        {
            var logger = GetMockLogger();
            var mediator = GetMockMediator();

            Assert.Throws<ArgumentNullException>(() => new PortalFileController(null!, logger.Object));
            Assert.Throws<ArgumentNullException>(() => new PortalFileController(mediator.Object, null!));
        }
    }
}