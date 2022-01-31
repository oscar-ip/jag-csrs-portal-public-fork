using AutoFixture;
using Csrs.Api.Controllers;
using Csrs.Api.Features.Accounts;
using Csrs.Api.Models;
using Csrs.Api.Services;
using Csrs.Interfaces.Dynamics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
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
            var accountServiceMock = new Mock<IAccountService>();

            new AccountController(accountServiceMock.Object, mediator.Object, logger.Object);
        }

        //[Fact]
        //public void ControllerConstructorChecksParameters()
        //{
        //    var logger = GetMockLogger();
        //    var mediator = GetMockMediator();

        //    Assert.Throws<ArgumentNullException>(() => new AccountController(null!, logger.Object));
        //    Assert.Throws<ArgumentNullException>(() => new AccountController(mediator.Object, null!));
        //}

        [Fact]
        public async Task GetProfileShouldCreateCorrectRequest()
        {
            var logger = GetMockLogger();
            var mediator = GetMockMediator(true);
            var httpContextMock = new Mock<HttpContext>();
            var accountServiceMock = new Mock<IAccountService>();

            Guid id = Guid.NewGuid();
            var user = CreateUser(id);

            mediator
                .Setup(_ => _.Send(It.Is<Profile.Request>(request => request.User == user), It.IsAny<CancellationToken>()))
                .ReturnsAsync(Profile.Response.Empty)
                .Verifiable("Correct request was not sent.");

            httpContextMock.Setup(_ => _.User).Returns(user);

            var sut = new AccountController(accountServiceMock.Object, mediator.Object, logger.Object);
            sut.ControllerContext.HttpContext = httpContextMock.Object;

            var actual = await sut.GetAsync(CancellationToken.None);

            mediator.Verify();
        }

        [Theory]
        [MemberData(nameof(GetChildrenTestCase))]
        public async Task CreateShouldCreateCorrectRequest(IList<Child> children, IList<Child> expectedChildren)
        {
            var logger = GetMockLogger();
            var mediator = GetMockMediator(true);
            var accountServiceMock = new Mock<IAccountService>();

            Fixture fixture = new Fixture();
            var user = CreateUser(Guid.NewGuid());
            var account = fixture.Create<Party>();
            var file = fixture.Create<File>();
            file.Children = children;

            mediator
                .Setup(_ => _.Send(It.Is<NewAccountAndFile.Request>(_ => _.Applicant == account && _.File == file), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new NewAccountAndFile.Response(string.Empty, string.Empty, string.Empty))
                .Verifiable("Correct request was not sent.");

            var httpContextMock = new Mock<HttpContext>();
            httpContextMock.Setup(_ => _.User).Returns(user);

            var sut = new AccountController(accountServiceMock.Object, mediator.Object, logger.Object);
            sut.ControllerContext.HttpContext = httpContextMock.Object;

            var actual = await sut.CreateAsync(new NewFileRequest { User = account, File = file }, CancellationToken.None);

            mediator.Verify();
        }

        public static IEnumerable<object[]> GetChildrenTestCase()
        {
            Fixture fixture = new Fixture();

            List<Child> children = null!;

            yield return new object[] { children, Array.Empty<Child>() }; // null children, expected the request to have empty

            children = new List<Child>();
            yield return new object[] { children, children }; // no children

            for (int i = 1; i <= 5; i++)
            {
                children = fixture.CreateMany<Child>(i).ToList();
                yield return new object[] { children, children }; // no children

            }
        }
    }
}
