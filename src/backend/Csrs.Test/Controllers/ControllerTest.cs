using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Reflection;
using System.Security.Claims;
using Xunit;

namespace Csrs.Test.Controllers
{
    public abstract class ControllerTest<TController> where TController : ControllerBase
    {
        protected Mock<ILogger<TController>> GetMockLogger(bool strict = false) => new(strict ? MockBehavior.Strict : MockBehavior.Default);
        protected Mock<IMediator> GetMockMediator(bool strict = false) => new(strict ? MockBehavior.Strict : MockBehavior.Default);

        /// <summary>
        /// Creates a <see cref="ClaimsPrincipal"/> with the correct claim type of the bceid user id.
        /// </summary>
        /// <param name="id">The &quot;bceid_userid&quot; to set on the user.</param>
        /// <returns></returns>
        protected ClaimsPrincipal CreateUser(Guid id)
        {
            // TODO: create a ClaimTypes class with all our claim types
            return new ClaimsPrincipal(new ClaimsIdentity(new[] { new Claim("bceid_userid", id.ToString()) }));
        }

        [Fact]
        public void ControllerConstructorChecksParameters()
        {
            var logger = GetMockLogger();
            var mediator = GetMockMediator();

            // find the 
            var constructor = typeof(TController).GetConstructor(new[] { typeof(IMediator), typeof(ILogger<TController>) });
            if (constructor != null)
            {
                Exception? exception;

                // invoking via reflection will throw TargetInvocationException with real exception in the InnerException
                exception = Assert.Throws<TargetInvocationException>(() => constructor.Invoke(new object[] { null!, logger.Object })).InnerException;
                Assert.Equal("mediator", Assert.IsType<ArgumentNullException>(exception).ParamName);

                exception = Assert.Throws<TargetInvocationException>(() => constructor.Invoke(new object[] { mediator.Object, null! })).InnerException;
                Assert.Equal("logger", Assert.IsType<ArgumentNullException>(exception).ParamName);
            }
        }
    }
}
