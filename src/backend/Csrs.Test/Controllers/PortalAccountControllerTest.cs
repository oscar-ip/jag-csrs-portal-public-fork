using Csrs.Api.Controllers;
using Csrs.Api.Features.PortalAccounts;
using Csrs.Api.Models;
using Moq;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Csrs.Test.Controllers
{
    public class PortalAccountControllerTest : ControllerTest<PortalAccountController>
    {
        [Fact]
        public void CanCreateController()
        {
            var logger = GetMockLogger();
            var mediator = GetMockMediator();
            new PortalAccountController(mediator.Object, logger.Object);
        }

        [Fact]
        public void ControllerConstructorChecksParameters()
        {
            var logger = GetMockLogger();
            var mediator = GetMockMediator();

            Assert.Throws<ArgumentNullException>(() => new PortalAccountController(null, logger.Object));
            Assert.Throws<ArgumentNullException>(() => new PortalAccountController(mediator.Object, null));
        }

        [Fact]
        public async Task GetProfileShouldCreateCorrectRequest()
        {
            var logger = GetMockLogger();
            var mediator = GetMockMediator(true);

            mediator
                .Setup(_ => _.Send(It.IsAny<Profile.Request>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Profile.Response(Array.Empty<PortalAccount>()))
                .Verifiable("Correct request was not sent.");

            var sut = new PortalAccountController(mediator.Object, logger.Object);

            var actual = await sut.GetProfileAsync(Guid.Empty);

            mediator.Verify();
        }

        [Fact]
        public async Task SignupShouldCreateCorrectRequest()
        {
            var logger = GetMockLogger();
            var mediator = GetMockMediator(true);

            mediator
                .Setup(_ => _.Send(It.IsAny<Signup.Request>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Signup.Response(Guid.NewGuid()))
                .Verifiable("Correct request was not sent.");

            var sut = new PortalAccountController(mediator.Object, logger.Object);

            var actual = await sut.SignupAsync(new PortalAccount());

            mediator.Verify();
        }

        [Fact]
        public async Task UpdateProfileAsyncShouldCreateCorrectRequest()
        {
            var logger = GetMockLogger();
            var mediator = GetMockMediator(true);

            mediator
                .Setup(_ => _.Send(It.IsAny<UpdateProfile.Request>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new UpdateProfile.Response(new PortalAccount()))
                .Verifiable("Correct request was not sent.");

            var sut = new PortalAccountController(mediator.Object, logger.Object);

            var actual = await sut.UpdateProfileAsync(new PortalAccount());

            mediator.Verify();
        }
    }
}
