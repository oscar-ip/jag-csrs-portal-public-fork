using Csrs.Api.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Xunit;

namespace Csrs.Test.Controllers
{
    /// <summary>
    /// Tests that validate the properties of all controllers in the project.
    /// </summary>
    public class ControllersTest
    {
        [Theory]
        [MemberData(nameof(GetControllerTypes))]
        public void ControllerShouldInheritFromCsrsControllerBase(Type controllerType)
        {
            var expectedBaseClass = typeof(CsrsControllerBase<>).MakeGenericType(controllerType);
            Assert.True(controllerType.IsSubclassOf(expectedBaseClass));
        }

        [Theory(Skip = "Skip until authentication is complete")]
        [MemberData(nameof(GetControllerTypes))]
        public void ControllerShouldHaveAuthorizeAttribute(Type controllerType)
        {
            Assert.NotNull(controllerType.GetCustomAttribute<AuthorizeAttribute>());
        }

        [Theory]
        [MemberData(nameof(GetControllerTypes))]
        public void ControllerShouldHaveApiControllerAttribute(Type controllerType)
        {
            Assert.NotNull(controllerType.GetCustomAttribute<ApiControllerAttribute>());
        }

        public static IEnumerable<object[]> GetControllerTypes
        {
            get
            {
                Assembly assembly = typeof(CsrsControllerBase<>).Assembly;

                var controllerTypes = assembly.GetTypes().Where(type => !type.IsAbstract && typeof(ControllerBase).IsAssignableFrom(type));
                foreach (var controllerType in controllerTypes)
                {
                    yield return new object[] { controllerType };
                }
            }
        }
    }
}
