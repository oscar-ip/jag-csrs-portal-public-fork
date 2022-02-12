using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace Csrs.Interfaces.Dynamics.Models
{
    class MicrosoftDynamicsCRMactivitypointerMetadata
    {
    }

    [MetadataType(typeof(MicrosoftDynamicsCRMactivitypointerMetadata))]
    public partial class MicrosoftDynamicsCRMactivitypointer
    {

        [JsonProperty(PropertyName = "owninguser@odata.bind")]
        public string OwninguserODataBind { get; set; }

        [JsonProperty(PropertyName = "ownerid@odata.bind")]
        public string OwnerIdODataBind { get; set; }

        [JsonProperty(PropertyName = "regardingobjectid_ssg_csrsfile@odata.bind")]
        public string RegardingobjectidSsgCsrsfileODataBind { get; set; }

    }

}
