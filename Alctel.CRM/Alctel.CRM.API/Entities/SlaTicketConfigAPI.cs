using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Alctel.CRM.API.Entities;

public class SlaTicketConfigAPI
{
    [JsonProperty("idChamadoSla")]
    public Int64 SlaTicketId { get; set; }

    [JsonProperty("tipoManifestacao")]
    public string? ManifestationTypeName { get; set; }

    [JsonProperty("servico")]
    public string? ServiceName { get; set; }

    [JsonProperty("criticidade")]
    public string? Criticality { get; set; }

    [JsonProperty("motivos")]
    public string? Reasons { get; set; }

    [JsonProperty("sla")]
    public string? Sla { get; set; }

    [JsonProperty("alarme")]
    public string? Alarm{ get; set; }

    [JsonProperty("idCriticidade")]
    public string? CriticalityId { get; set; }
}
