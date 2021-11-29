using Csrs.Api.Configuration;
using Serilog;
using Serilog.Core;
using Serilog.Events;
using System.Runtime.CompilerServices;

namespace Microsoft.Extensions.DependencyInjection;

public static class SerilogExtensions
{
    public static void UseSerilog(this WebApplicationBuilder builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        builder.Host.UseSerilog((hostingContext, loggerConfiguration) =>
        {
            loggerConfiguration.ReadFrom.Configuration(builder.Configuration);

#if false
            loggerConfiguration.Enrich.With<GitVersionEnricher>();
#endif

            // get the configuration type
            CsrsConfiguration configuration = builder.Configuration.Get<CsrsConfiguration>();

            var splunk = configuration.Splunk;
            if (splunk is null || string.IsNullOrEmpty(splunk.Url) || string.IsNullOrEmpty(splunk.Token))
            {
                return;
            }

            HttpClientHandler? handler = null;

            if (!splunk.ValidatServerCertificate)
            {
                handler = new HttpClientHandler
                {
                    ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
                };
            }

            // When writing to splunk, limit data to information level unless set
            LogEventLevel minimumLevel = LogEventLevel.Information;
            if (splunk.MinimumLevel.HasValue)
            {
                minimumLevel = splunk.MinimumLevel.Value;
            }

            loggerConfiguration.WriteTo.EventCollector(
                splunkHost: splunk.Url,
                eventCollectorToken: splunk.Token,
                sourceType: typeof(SerilogExtensions).Assembly?.GetName()?.Name,
                restrictedToMinimumLevel: minimumLevel,
                messageHandler: handler);
        });
    }


#if false // GitVersion not working in docker build, remove for now

    public class GitVersionEnricher : ILogEventEnricher
    {
        private LogEventProperty? _cachedProperty;

        /// <summary>
        /// The property name added to enriched log events.
        /// </summary>
        public const string GitVersionNamePropertyName = "GitVersion";

        public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
        {
            logEvent.AddPropertyIfAbsent(GetLogEventProperty(propertyFactory));
        }

        private LogEventProperty GetLogEventProperty(ILogEventPropertyFactory propertyFactory)
        {
            // Don't care about thread-safety, in the worst case the field gets overwritten and one
            // property will be GCed
            if (_cachedProperty is null)
            {
                _cachedProperty = CreateProperty(propertyFactory);
            }

            return _cachedProperty;
        }

        // Qualify as uncommon-path
        [MethodImpl(MethodImplOptions.NoInlining)]
        private static LogEventProperty CreateProperty(ILogEventPropertyFactory propertyFactory)
        {
            /* https://github.com/GitTools/GitVersion
            ** 
            ** Possible Properties
            ** 
            ** Major = "0";
            ** Minor = "1";
            ** Patch = "0";
            ** PreReleaseTag = "";
            ** PreReleaseTagWithDash = "";
            ** PreReleaseLabel = "";
            ** PreReleaseLabelWithDash = "";
            ** PreReleaseNumber = "";
            ** WeightedPreReleaseNumber = "60000";
            ** BuildMetaData = "35";
            ** BuildMetaDataPadded = "0035";
            ** FullBuildMetaData = "35.Branch.main.Sha.a1246c0e9e595775383f4bcb604e77db60f21392";
            ** MajorMinorPatch = "0.1.0";
            ** SemVer = "0.1.0";
            ** LegacySemVer = "0.1.0";
            ** LegacySemVerPadded = "0.1.0";
            ** AssemblySemVer = "0.1.0.0";
            ** AssemblySemFileVer = "0.1.0.0";
            ** FullSemVer = "0.1.0+35";
            ** InformationalVersion = "0.1.0+35.Branch.main.Sha.a1246c0e9e595775383f4bcb604e77db60f21392";
            ** BranchName = "main";
            ** EscapedBranchName = "main";
            ** Sha = "a1246c0e9e595775383f4bcb604e77db60f21392";
            ** ShortSha = "a1246c0";
            ** NuGetVersionV2 = "0.1.0";
            ** NuGetVersion = "0.1.0";
            ** NuGetPreReleaseTagV2 = "";
            ** NuGetPreReleaseTag = "";
            ** VersionSourceSha = "c7be21a18230ed8e2e89fae96009d41451d2a756";
            ** CommitsSinceVersionSource = "35";
            ** CommitsSinceVersionSourcePadded = "0035";
            ** UncommittedChanges = "8";
            ** CommitDate = "2021-11-23";
            */

            return propertyFactory.CreateProperty(GitVersionNamePropertyName, new
            {
                GitVersionInformation.Sha,
                GitVersionInformation.BranchName
            });
        }
    }
#endif
}
