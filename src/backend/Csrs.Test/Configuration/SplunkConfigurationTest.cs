using Csrs.Api.Configuration;
using Xunit;

namespace Csrs.Test.Configuration
{
    public class SplunkConfigurationTest
    {
        [Fact]
        public void ValidatServerCertificate_should_default_to_true()
        {
            SplunkConfiguration configuration = new();
            Assert.True(configuration.ValidatServerCertificate);
        }
    }
}
