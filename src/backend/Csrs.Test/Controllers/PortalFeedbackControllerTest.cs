using Csrs.Api.Controllers;
using Csrs.Api.Features.PortalFeedbacks;
using Csrs.Api.Models;
using Moq;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Csrs.Test.Controllers
{
    public class PortalFeedbackControllerTest : ControllerTest<PortalFeedbackController>
    {
        [Fact]
        public void CanCreateController()
        {
            var logger = GetMockLogger();
            var mediator = GetMockMediator();
            new PortalFeedbackController(mediator.Object, logger.Object);
        }

        [Fact]
        public void ControllerConstructorChecksParameters()
        {
            var logger = GetMockLogger();
            var mediator = GetMockMediator();

            Assert.Throws<ArgumentNullException>(() => new PortalFeedbackController(null, logger.Object));
            Assert.Throws<ArgumentNullException>(() => new PortalFeedbackController(mediator.Object, null));
        }

        [Fact]
        public async Task CreateShouldCreateCorrectRequest()
        {
            var logger = GetMockLogger();
            var mediator = GetMockMediator(true);

            mediator
                .Setup(_ => _.Send(It.IsAny<Create.Request>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Create.Response())
                .Verifiable("Correct request was not sent.");

            var sut = new PortalFeedbackController(mediator.Object, logger.Object);

            var actual = await sut.CreateAsync(new PortalFeedback());

            mediator.Verify();
        }
    }
}