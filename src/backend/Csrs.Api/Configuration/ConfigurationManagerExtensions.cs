using System.Diagnostics.CodeAnalysis;

namespace Csrs.Api.Configuration
{
    public static class ConfigurationManagerExtensions
    {
        /// <summary>
        /// Adds the CSRS environment variables as a configuration source.
        /// </summary>
        [ExcludeFromCodeCoverage]
        public static void AddCsrsEnvironmentVariables(this ConfigurationManager configurationManager)
        {
            ((IConfigurationBuilder)configurationManager).Add(new CsrsEnvironmentVariablesConfigurationSource());
        }
    }
}
