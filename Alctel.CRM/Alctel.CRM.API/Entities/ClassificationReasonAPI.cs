using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Alctel.CRM.API.Entities;

public class ClassificationReasonAPI
{
    [JsonProperty("idClassMotivo")]
    public Int64 Id { get; set; }

    [JsonProperty("idClassTipo")]
    public Int64 ClassificationDemandTypeId { get; set; }

    [JsonProperty("motivoClassMotivo")]
    public string? Name { get; set; }

    [JsonProperty("idListaClassMotivo")]
    public Int64 ClassificationListReasonId { get; set; }

    [JsonProperty("idMotivoPai")]
    public Int64? ParentReasonId { get; set; }

    [JsonProperty("statusClassMotivo")]
    public bool Active { get; set; }
}
