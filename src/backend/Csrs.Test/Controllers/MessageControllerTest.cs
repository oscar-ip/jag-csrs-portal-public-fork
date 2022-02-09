using Csrs.Api.Controllers;
using Csrs.Api.Features.Messages;
using Csrs.Api.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Csrs.Test.Controllers
{
    public class MessageControllerTest : ControllerTest<MessageController>
    {
        [Fact]
        public void CanCreateController()
        {
            var logger = GetMockLogger();
            var mediator = GetMockMediator();
            new MessageController(mediator.Object, logger.Object);
        }

        [Fact]
        public async Task GetListShouldCreateCorrectRequest()
        {
            var logger = GetMockLogger();
            var mediator = GetMockMediator(true);
            var httpContextMock = new Mock<HttpContext>();

            List<Message> messages = new List<Message>();

            messages.Add(new Message());


            Guid id = Guid.NewGuid();
            var user = CreateUser(id);

            mediator
                .Setup(_ => _.Send(It.IsAny<List.Request>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List.Response(messages))
                .Verifiable("Correct request was sent.");

            httpContextMock.Setup(_ => _.User).Returns(user);

            var sut = new MessageController(mediator.Object, logger.Object);
            sut.ControllerContext.HttpContext = httpContextMock.Object;

            var actual = await sut.GetAsync(CancellationToken.None);

            Assert.IsType<OkObjectResult>(actual);

            mediator.Verify();
        }

        [Fact]
        public async Task GetShouldReturnNotFoundRequest()
        {
            var logger = GetMockLogger();
            var mediator = GetMockMediator(true);
            var httpContextMock = new Mock<HttpContext>();

            Guid id = Guid.NewGuid();
            var user = CreateUser(id);

            mediator
                .Setup(_ => _.Send(It.IsAny<List.Request>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(List.Response.Empty)
                .Verifiable("Correct request was not sent.");

            httpContextMock.Setup(_ => _.User).Returns(user);

            var sut = new MessageController(mediator.Object, logger.Object);
            sut.ControllerContext.HttpContext = httpContextMock.Object;

            var actual = await sut.GetAsync(CancellationToken.None);

            Assert.IsType<NotFoundResult>(actual);

            mediator.Verify();

        }

    }
}