using Csrs.Api.Controllers;
using Csrs.Api.Features.PortalFeedbacks;
using Csrs.Api.Models;
using Moq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Csrs.Test.Controllers
{
    public class PortalFeedbackControllerTest : ControllerTest<FeedbackController>
    {
        [Fact]
        public void CanCreateController()
        {
            var logger = GetMockLogger();
            var mediator = GetMockMediator();
            new FeedbackController(mediator.Object, logger.Object);
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

            var sut = new FeedbackController(mediator.Object, logger.Object);

            var actual = await sut.CreateAsync(new Feedback());

            mediator.Verify();
        }
    }
}