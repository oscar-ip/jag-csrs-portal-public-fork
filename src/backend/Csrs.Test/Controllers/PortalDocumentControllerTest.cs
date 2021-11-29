using Csrs.Api.Controllers;
using Csrs.Api.Features.PortalDocuments;
using Csrs.Api.Models;
using Moq;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Csrs.Test.Controllers
{
    public class PortalDocumentControllerTest : ControllerTest<PortalDocumentController>
    {
        [Fact]
        public void CanCreateController()
        {
            var logger = GetMockLogger();
            var mediator = GetMockMediator();
            new PortalDocumentController(mediator.Object, logger.Object);
        }

        [Fact]
        public async Task ListDocumentsShouldCreateCorrectRequest()
        {
            var logger = GetMockLogger();
            var mediator = GetMockMediator(true);

            mediator
                .Setup(_ => _.Send(It.IsAny<ListDocuments.Request>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ListDocuments.Response())
                .Verifiable("Correct request was not sent.");

            var sut = new PortalDocumentController(mediator.Object, logger.Object);

            var actual = await sut.ListDocumentsAsync(string.Empty);

            mediator.Verify();
        }


        [Fact]
        public async Task UploadDocumentsShouldCreateCorrectRequest()
        {
            var logger = GetMockLogger();
            var mediator = GetMockMediator(true);

            mediator
                .Setup(_ => _.Send(It.IsAny<UploadDocuments.Request>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new UploadDocuments.Response())
                .Verifiable("Correct request was not sent.");

            var sut = new PortalDocumentController(mediator.Object, logger.Object);

            var actual = await sut.UploadDocumentsAsync(new PortalDocument()); ;

            mediator.Verify();
        }

        [Fact]
        public async Task DownloadDocumentShouldCreateCorrectRequest()
        {
            var logger = GetMockLogger();
            var mediator = GetMockMediator(true);

            mediator
                .Setup(_ => _.Send(It.IsAny<DownloadDocument.Request>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new DownloadDocument.Response())
                .Verifiable("Correct request was not sent.");

            var sut = new PortalDocumentController(mediator.Object, logger.Object);

            var actual = await sut.DownloadDocumentAsync(string.Empty);

            mediator.Verify();
        }
    }
}