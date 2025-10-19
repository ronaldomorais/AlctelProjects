using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Alctel.CRM.API.Entities;

public class TransferDemandAreaByAreaAPI
{
    [JsonProperty("idAreaDemanda")]
    public Int64 DemandAreaId { get; set; }

    [JsonProperty("nome")]
    public string? DemandAreaName { get; set; }

    [JsonProperty("idArea")]
    public string? AreaQueueId { get; set; }

    [JsonProperty("idFormulario")]
    public string? FormId { get; set; }

    [JsonProperty("status")]
    public string? DemandAreaStatus { get; set; }
}
