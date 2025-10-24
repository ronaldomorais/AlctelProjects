using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Alctel.CRM.API.Entities;

public class TicketClassificationReasonCreateAPI
{
    [JsonProperty("idClassificacaoManifestacao")]
    public Int64 ManifestationTypeId { get; set; }

    [JsonProperty("idClassificacaoPrograma")]
    public Int64 ProgramId { get; set; }

    [JsonProperty("nomeServico")]
    public string? ServiceName { get; set; }

    [JsonProperty("motivos")]
    public List<TicketReasonCreateAPI> ticketReason { get; set; } = new List<TicketReasonCreateAPI>();
}

public class TicketReasonCreateAPI
{
    [JsonProperty("idLista")]
    public Int64 ListId { get; set; }

    [JsonProperty("idMotivoPai")]
    public Int64? ParentId { get; set; }

    [JsonProperty("nomeMotivo")]
    public string? ReasonName { get; set; }

    [JsonProperty("status")]
    public bool Status { get; set; }
}
