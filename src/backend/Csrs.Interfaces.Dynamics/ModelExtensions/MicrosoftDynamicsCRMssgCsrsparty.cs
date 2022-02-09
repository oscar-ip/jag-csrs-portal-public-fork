using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace Csrs.Interfaces.Dynamics.Models
{
    
    class MicrosoftDynamicsCRMssgCsrspartyMetadata
    {
        [JsonProperty(PropertyName = "ssg_dateofbirth")]
        [JsonConverter(typeof(DateFormatConverter), "yyyy-MM-dd")]
        public DateTimeOffset? SsgDateofbirth { get; set; }
    }
    
    
    [MetadataType(typeof(MicrosoftDynamicsCRMssgCsrspartyMetadata))]
    public partial class MicrosoftDynamicsCRMssgCsrsparty
    {
    }
}
