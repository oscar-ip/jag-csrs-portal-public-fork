using AutoFixture;
using Csrs.Api.Controllers;
using Csrs.Api.Features.Accounts;
using Csrs.Api.Models;
using Csrs.Api.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Csrs.Test.Controllers
{
    public class LookupControllerTest : ControllerTest<LookupController>
    {
    }

    public class AccountControllerTest : ControllerTest<AccountController>
    {
        [Fact]
        public void CanCreateController()
        {
            var logger = GetMockLogger();
            var mediator = GetMockMediator();

            new AccountController(mediator.Object, logger.Object);
        }

        //[Fact]
        //public void ControllerConstructorChecksParameters()
        //{
        //    var logger = GetMockLogger();
        //    var mediator = GetMockMediator();

        //    Assert.Throws<ArgumentNullException>(() => new AccountController(null!, logger.Object));
        //    Assert.Throws<ArgumentNullException>(() => new AccountController(mediator.Object, null!));
        //}

       [Theory]
       [MemberData(nameof(LookupsRequestTypes))]
        public async Task LookupsCallMediatorCorrectly(Lookups.Request expectedRequest, Expression<Func<AccountController, Task<IActionResult>>> action)
        {
            var logger = GetMockLogger();
            var mediator = GetMockMediator(true);

            Fixture fixture = new Fixture();

            var expected = fixture.CreateMany<OptionValue>().ToList();

            mediator
                .Setup(_ => _.Send(It.Is<Lookups.Request>(request => request == expectedRequest), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Lookups.Response(expected))
                .Verifiable("Correct request was not sent.");

            var sut = new AccountController(mediator.Object, logger.Object);

            await action.Compile()(sut);
        }

        public static IEnumerable<object[]> LookupsRequestTypes
        {
            get
            {
                Expression<Func<AccountController, Task<IActionResult>>> action;

                action = controller => controller.GetGendersAsync(CancellationToken.None);
                yield return new object[] { Lookups.Request.Gender, action };

                action = controller => controller.GetProvincesAsync(CancellationToken.None);
                yield return new object[] { Lookups.Request.Province, action };

                action = controller => controller.GetIdentitesAsync(CancellationToken.None);
                yield return new object[] { Lookups.Request.Identity, action };

                action = controller => controller.GetReferralsAsync(CancellationToken.None);
                yield return new object[] { Lookups.Request.Referral, action };
            }
        }

        [Fact]
        public async Task GetProfileShouldCreateCorrectRequest()
        {
            var logger = GetMockLogger();
            var mediator = GetMockMediator(true);
            var httpContextMock = new Mock<HttpContext>();

            var user = CreateUser(Guid.NewGuid());

            mediator
                .Setup(_ => _.Send(It.Is<Profile.Request>(request => request.User == user), It.IsAny<CancellationToken>()))
                .ReturnsAsync(Profile.Response.Empty)
                .Verifiable("Correct request was not sent.");

            httpContextMock.Setup(_ => _.User).Returns(user);

            var sut = new AccountController(mediator.Object, logger.Object);
            sut.ControllerContext.HttpContext = httpContextMock.Object;

            var actual = await sut.GetProfileAsync(CancellationToken.None);

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

            var sut = new AccountController(mediator.Object, logger.Object);

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

            var sut = new AccountController(mediator.Object, logger.Object);

            var actual = await sut.UpdateProfileAsync(new PortalAccount());

            mediator.Verify();
        }
    }
}
