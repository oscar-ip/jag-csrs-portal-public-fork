using AutoFixture;
using Csrs.Interfaces.Dynamics.Models;
using Microsoft.Rest.Serialization;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using Xunit;

namespace Csrs.Test.Interfaces.Dynamics.Models
{
    public class MicrosoftDynamicsCRMssgCsrspartyTest
    {
        [Fact]
        public void party_date_of_birth_should_be_serialized_as_yyyy_mm_dd()
        {
            Fixture fixture = new Fixture();

            var serializationSettings = new JsonSerializerSettings
            {
                Formatting = Formatting.None,
                DateFormatHandling = DateFormatHandling.IsoDateFormat,
                DateTimeZoneHandling = DateTimeZoneHandling.Utc,
                NullValueHandling = NullValueHandling.Ignore,
                ReferenceLoopHandling = ReferenceLoopHandling.Serialize,
                ContractResolver = new ReadOnlyJsonContractResolver(),
                Converters = new List<JsonConverter>
                    {
                        new Iso8601TimeSpanConverter()
                    }
            };

            var dob = fixture.Create<DateTimeOffset>();

            MicrosoftDynamicsCRMssgCsrsparty sut = new MicrosoftDynamicsCRMssgCsrsparty();
            sut.SsgDateofbirth = dob;
            var expected = $"{{\"ssg_dateofbirth\":\"{dob.Year:d4}-{dob.Month:d2}-{dob.Day:d2}\"}}";

            var actual = JsonConvert.SerializeObject(sut, serializationSettings);
            Assert.Equal(expected, actual);
        }
    }
}
