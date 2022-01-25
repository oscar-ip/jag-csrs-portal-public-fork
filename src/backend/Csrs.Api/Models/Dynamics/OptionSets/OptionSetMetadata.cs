namespace Csrs.Api.Models.Dynamics.OptionSets
{
    public class OptionSetMetadata
    {
        public OptionSet? OptionSet { get; set; }
    }

    public class PicklistOptionSetMetadata
    {
        public List<OptionSetMetadata>? Value { get; set; }
    }


    public static class OptionSetMetadataExtensions
    {
        public static IEnumerable<LookupValue> GetOptionValues(this OptionSetMetadata? optionSetMetadata)
        {
            if (optionSetMetadata?.OptionSet?.Options == null)
            {
                return Array.Empty<LookupValue>();
            }

            var values = optionSetMetadata.OptionSet.Options
                .Select(_ => new LookupValue(_.Value, _.Label?.UserLocalizedLabel?.Label ?? string.Empty));

            return values;
        }

        public static IEnumerable<LookupValue> GetOptionValues(this PicklistOptionSetMetadata? picklistOptionSetMetadata)
        {
            if (picklistOptionSetMetadata == null || picklistOptionSetMetadata.Value == null)
            {
                yield break;
            }

            foreach (var optionSetMetadata in picklistOptionSetMetadata.Value)
            {
                foreach (var optionValue in optionSetMetadata.GetOptionValues())
                {
                    yield return optionValue;
                }
            }
        }

    }
}
