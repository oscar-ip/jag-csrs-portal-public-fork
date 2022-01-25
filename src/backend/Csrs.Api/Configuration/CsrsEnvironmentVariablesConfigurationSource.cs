namespace Csrs.Api.Configuration
{
    public class CsrsEnvironmentVariablesConfigurationSource : IConfigurationSource
    {
        public IConfigurationProvider Build(IConfigurationBuilder builder)
        {
            return new CsrsEnvironmentVariablesConfigurationProvider();
        }
    }
}
