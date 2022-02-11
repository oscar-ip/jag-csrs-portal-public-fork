using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace Csrs.Interfaces.Dynamics.Models
{
    public partial class MicrosoftDynamicsCRMssgCsrsfileMetadata
    {
        [JsonProperty(PropertyName = "ssg_dateoforderorwa")]
        [JsonConverter(typeof(DateFormatConverter), "yyyy-MM-dd")]
        public DateTimeOffset? SsgDateoforderorwa { get; set; }

        [JsonProperty(PropertyName = "ssg_dateoforderorwa")]
        [JsonConverter(typeof(BooleanConverter))]
        public bool SafetyAlertRecipient { get; set; }

        [JsonProperty(PropertyName = "ssg_dateoforderorwa")]
        [JsonConverter(typeof(BooleanConverter))]
        public bool SafetyAlertPayor { get; set; }

        [JsonProperty(PropertyName = "ssg_dateoforderorwa")]
        [JsonConverter(typeof(BooleanConverter))]
        public bool IsFMEPFileActive { get; set; }

        [JsonProperty(PropertyName = "ssg_registrationdate")]
        [JsonConverter(typeof(DateFormatConverter), "yyyy-MM-dd")]
        public System.DateTimeOffset? SsgRegistrationdate { get; set; }
    }

    [MetadataType(typeof(MicrosoftDynamicsCRMssgCsrsfileMetadata))]
    public partial class MicrosoftDynamicsCRMssgCsrsfile
    {
        [JsonProperty(PropertyName = "ssg_BCCourtLocation@odata.bind")]
        public string SsgBCCourtLocationODataBind { get; set; }

        [JsonProperty(PropertyName = "ssg_BCCourtLevel@odata.bind")]
        public string SsgBCCourtLevelODataBind { get; set; }

        [JsonProperty(PropertyName = "ssg_Recipient@odata.bind")]
        public string SsgRecipientODataBind { get; set; }

        [JsonProperty(PropertyName = "ssg_Payor@odata.bind")]
        public string SsgPayorODataBind { get; set; }

    }
}
