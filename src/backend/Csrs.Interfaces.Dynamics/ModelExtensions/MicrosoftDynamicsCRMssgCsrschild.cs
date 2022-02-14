using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace Csrs.Interfaces.Dynamics.Models
{
    public partial class MicrosoftDynamicsCRMssgCsrschildMetadata
    {
        [JsonProperty(PropertyName = "ssg_dateofbirth")]
        [JsonConverter(typeof(DateFormatConverter), "yyyy-MM-dd")]
        public DateTimeOffset? SsgDateofbirth { get; set; }
    }

    [MetadataType(typeof(MicrosoftDynamicsCRMssgCsrschildMetadata))]
    public partial class MicrosoftDynamicsCRMssgCsrschild
    {
        [JsonProperty(PropertyName = "ssg_ChildsFather@odata.bind")]
        public string SsgChildsFatherODataBind { get; set; }

        [JsonProperty(PropertyName = "ssg_ChildsMother@odata.bind")]
        public string SsgChildsMotherODataBind { get; set; }

        [JsonProperty(PropertyName = "ssg_FileNumber@odata.bind")]
        public string SsgFileNumberODataBind { get; set; }
       
    }
}
