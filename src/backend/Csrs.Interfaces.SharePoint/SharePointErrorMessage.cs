using System.Text.Json.Serialization;

namespace Csrs.Interfaces
{
    public class SharePointErrorMessage
    {
        [JsonPropertyName("lang")]
        public string Language { get; set; }

        [JsonPropertyName("value")]
        public string Value { get; set; }

    }
}