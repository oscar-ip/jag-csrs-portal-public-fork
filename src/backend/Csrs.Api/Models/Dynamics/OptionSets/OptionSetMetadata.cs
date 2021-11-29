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
        public static IEnumerable<OptionValue> GetOptionValues(this OptionSetMetadata? optionSetMetadata)
        {
            if (optionSetMetadata?.OptionSet?.Options == null)
            {
                return Array.Empty<OptionValue>();
            }

            var values = optionSetMetadata.OptionSet.Options
                .Select(_ => new OptionValue(_.Value, _.Label?.UserLocalizedLabel?.Label ?? string.Empty));

            return values;
        }

        public static IEnumerable<OptionValue> GetOptionValues(this PicklistOptionSetMetadata? picklistOptionSetMetadata)
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
