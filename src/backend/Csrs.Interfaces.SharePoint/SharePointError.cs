using System.Text.Json.Serialization;

namespace Csrs.Interfaces
{
    public class SharePointError
    {
        [JsonPropertyName("code")]
        public string Code { get; set; }

        [JsonPropertyName("message")]
        public SharePointErrorMessage Message { get; set; }
    }
}