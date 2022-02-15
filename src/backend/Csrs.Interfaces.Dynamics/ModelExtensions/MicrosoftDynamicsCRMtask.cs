using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace Csrs.Interfaces.Dynamics.Models
{
    
    class MicrosoftDynamicsCRMtaskMetadata
    {
    }

    [MetadataType(typeof(MicrosoftDynamicsCRMtaskMetadata))]
    public partial class MicrosoftDynamicsCRMtask
    {

        [JsonProperty(PropertyName = "owninguser_task@odata.bind")]
        public string OwninguserTaskODataBind { get; set; }

        [JsonProperty(PropertyName = "owningteam_task@odata.bind")]
        public string OwningteamTaskODataBind { get; set; }

    }
}
