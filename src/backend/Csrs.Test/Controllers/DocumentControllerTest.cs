using Csrs.Api.Controllers;
using Csrs.Api.Features.Documents;
using Csrs.Api.Models;
using Moq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Csrs.Test.Controllers
{
    public class DocumentControllerTest : ControllerTest<DocumentController>
    {
        [Fact]
        public void CanCreateController()
        {
            var logger = GetMockLogger();
            var mediator = GetMockMediator();
            new DocumentController(mediator.Object, logger.Object);
        }
    }
}
