using AutoFixture;
using AutoMapper;
using Csrs.Api.Models;
using Csrs.Api.Models.Dynamics;
using Csrs.Api.Models.Mapping;
using Xunit;

namespace Csrs.Test.Models.Mapping
{
    public class PortalAccountProfileTest : MappingProfileTest<PortalAccountProfile, PortalAccount, SSG_CsrsParty>
    {
        [Fact]
        public void PortalAccountProfile()
        {
            IMapper mapper = CreateMapper();

            var expected = _fixture.Create<PortalAccount>();

            var actual = mapper.Map<SSG_CsrsParty>(expected);

            Assert.Equal(expected.AddressStreet1, actual.AddressStreet1);
        }
    }
}
