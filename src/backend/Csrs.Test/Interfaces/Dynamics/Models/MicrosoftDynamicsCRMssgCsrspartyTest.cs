using AutoFixture;
using Csrs.Interfaces.Dynamics.Models;
using Newtonsoft.Json;
using System;
using Xunit;

namespace Csrs.Test.Interfaces.Dynamics.Models
{
    public class MicrosoftDynamicsCRMssgCsrspartyTest : ModelTestBase
    {
        [Fact]
        public void party_date_of_birth_should_be_serialized_as_yyyy_mm_dd()
        {
            Fixture fixture = new Fixture();
 
            var dob = fixture.Create<DateTimeOffset>();

            MicrosoftDynamicsCRMssgCsrsparty sut = new MicrosoftDynamicsCRMssgCsrsparty();
            sut.SsgDateofbirth = dob;
            var expected = $"{{\"ssg_dateofbirth\":\"{dob.Year:d4}-{dob.Month:d2}-{dob.Day:d2}\"}}";

            var actual = JsonConvert.SerializeObject(sut, JsonSerializerSettings);
            Assert.Equal(expected, actual);
        }
    }
}
