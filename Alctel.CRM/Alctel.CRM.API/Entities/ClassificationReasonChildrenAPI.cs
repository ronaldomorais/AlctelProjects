using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Alctel.CRM.API.Entities;

public class ClassificationReasonChildrenAPI
{
    [JsonProperty("idClassMotivo")]
    public Int64 Id { get; set; }

    [JsonProperty("idClassTipo")]
    public Int64 ClassificationDemandTypeId { get; set; }

    [JsonProperty("motivo")]
    public string? Name { get; set; }

    [JsonProperty("idLista")]
    public Int64 ClassificationListReasonId { get; set; }

    [JsonProperty("idMotivoPai")]
    public Int64? ParentReasonId { get; set; }

    [JsonProperty("status")]
    public bool Active { get; set; }
}
