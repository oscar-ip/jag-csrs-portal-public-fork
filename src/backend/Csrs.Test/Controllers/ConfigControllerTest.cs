using Csrs.Api.Controllers;
using Csrs.Api.Features.Feedbacks;
using Csrs.Api.Models;
using Moq;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Csrs.Test.Controllers
{
    public class ConfigControllerTest : ControllerTest<ConfigController>
    {
        [Fact]
        public void CanCreateController()
        {
            var logger = GetMockLogger();
            var mediator = GetMockMediator();
            var config = GetMockConfig();
            new ConfigController(mediator.Object, logger.Object, config.Object);
        }

        [Fact]
        public List<string> CreateShouldCreateCorrectRequest()
        {
            var logger = GetMockLogger();
            var mediator = GetMockMediator(true);
            var config = GetMockConfig();

            mediator
                .Setup(_ => _.Send(It.IsAny<Create.Request>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Create.Response())
                .Verifiable("Correct request was not sent.");

            var sut = new ConfigController(mediator.Object, logger.Object, config.Object);

            var actual = sut.GetAppSettings();

            mediator.Verify();

            return actual;
        }
    }
}