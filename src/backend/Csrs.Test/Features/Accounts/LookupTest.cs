using AutoFixture;
using Csrs.Api.Features.Accounts;
using Csrs.Api.Models;
using Csrs.Api.Repositories;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Csrs.Test.Features.Accounts
{
    public class LookupTest
    {
        [Fact]
        public void Response_cannot_be_constructed_with_null()
        {
            Assert.Throws<ArgumentNullException>(() => new Lookups.Response(null!));
        }

        [Fact]
        public void Handler_cannot_be_constructed_with_null_repository()
        {
            var loggerMock = new Mock<ILogger<Lookups.Handler>>();

            var actual = Assert.Throws<ArgumentNullException>(() => new Lookups.Handler(null!, loggerMock.Object));
            Assert.Equal("repository", actual.ParamName);
        }

        [Fact]
        public void Handler_cannot_be_constructed_with_null_logger()
        {
            var repositoryMock = new Mock<ICsrsPartyRepository>();

            var actual = Assert.Throws<ArgumentNullException>(() => new Lookups.Handler(repositoryMock.Object, null!));
            Assert.Equal("logger", actual.ParamName);
        }

        [Fact]
        public async Task Requesting_Genders_calls_correct_repository_function()
        {
            await Picklist_calls_correct_repository_function(Lookups.Request.Gender, _ => _.GetGenderPicklistAsync(It.IsAny<CancellationToken>()));
        }

        [Fact]
        public async Task Requesting_Identity_calls_correct_repository_function()
        {
            await Picklist_calls_correct_repository_function(Lookups.Request.Identity, _ => _.GetIdentityPicklistAsync(It.IsAny<CancellationToken>()));
        }

        [Fact]
        public async Task Requesting_Referral_calls_correct_repository_function()
        {
            await Picklist_calls_correct_repository_function(Lookups.Request.Referral, _ => _.GetReferralPicklistAsync(It.IsAny<CancellationToken>()));
        }

        [Fact]
        public async Task Requesting_Province_calls_correct_repository_function()
        {
            await Picklist_calls_correct_repository_function(Lookups.Request.Province, _ => _.GetProvincePicklistAsync(It.IsAny<CancellationToken>()));
        }

        private async Task Picklist_calls_correct_repository_function(Lookups.Request request, Expression<Func<ICsrsPartyRepository, Task<IList<OptionValue>>>> action)
        {
            var repositoryMock = new Mock<ICsrsPartyRepository>(MockBehavior.Strict);
            var loggerMock = new Mock<ILogger<Lookups.Handler>>();
            Fixture fixture = new Fixture();

            var expected = fixture.CreateMany<OptionValue>().ToList();

            repositoryMock.Setup(action).ReturnsAsync(expected);

            var sut = new Lookups.Handler(repositoryMock.Object, loggerMock.Object);

            var response = await sut.Handle(request, CancellationToken.None);

            Assert.Equal(expected, response.Items);

            repositoryMock.Verify();
            loggerMock.Verify();
        }
    }
}
