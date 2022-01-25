using Csrs.Api.Controllers;
using Xunit;

namespace Csrs.Test.Controllers
{
    public class UserRequestControllerTest : ControllerTest<UserRequestController>
    {
        [Fact]
        public void CanCreateController()
        {
            var logger = GetMockLogger();
            var mediator = GetMockMediator();
            new UserRequestController(mediator.Object, logger.Object);
        }
    }
}