namespace Csrs.Api.Configuration
{
    public class CsrsEnvironmentVariablesConfigurationProvider : ConfigurationProvider
    {
        public override void Load()
        {
            Add("SPLUNK_URL", $"{nameof(CsrsConfiguration.Splunk)}:{nameof(SplunkConfiguration.Url)}");
            Add("SPLUNK_TOKEN", $"{nameof(CsrsConfiguration.Splunk)}:{nameof(SplunkConfiguration.Token)}");
            Add("SPLUNK_VALIDATE_SERVER_CERTIFICATE", $"{nameof(CsrsConfiguration.Splunk)}:{nameof(SplunkConfiguration.ValidatServerCertificate)}");
        }

        /// <summary>
        /// Gets the environment variable and maps it to app key if it has a value.
        /// </summary>
        /// <param name="variable"></param>
        /// <param name="appKey"></param>
        private void Add(string variable, string appKey)
        {
            if (variable == null) throw new ArgumentNullException(nameof(variable));
            if (appKey == null) throw new ArgumentNullException(nameof(appKey));

            // note the Load method may be called multiple times if before the
            // application builder Build method is called, the configuration is requested
            // so check if the key already exists before adding it
            if (!Data.ContainsKey(appKey))
            {
                var value = Environment.GetEnvironmentVariable(variable);
                if (value != null)
                {
                    Data.Add(appKey, value);
                }
            }
        }
    }
}
