using Csrs.Api.Configuration;
using System.Diagnostics.CodeAnalysis;

namespace Microsoft.Extensions.DependencyInjection;

public static class ConfigurationManagerExtensions
{
    /// <summary>
    /// Adds the CSRS environment variables as a configuration source.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public static void AddCsrsEnvironmentVariables(this ConfigurationManager configurationManager)
    {
        ((IConfigurationBuilder)configurationManager).Add(new CsrsEnvironmentVariablesConfigurationSource()).AddEnvironmentVariables();
    }
}
