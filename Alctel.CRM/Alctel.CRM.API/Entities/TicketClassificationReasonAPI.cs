using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Alctel.CRM.API.Entities;

public class TicketClassificationReasonAPI
{
    [JsonProperty("idClassificacaoMotivo")]
    public Int64 Id { get; set; }

    [JsonProperty("idClassificacaoTipoManifestacao")]
    public Int64 ManifestationTypeId { get; set; }

    [JsonProperty("motivo")]
    public string? ReasonName { get; set; }

    [JsonProperty("idLista")]
    public Int64 ListId { get; set; }

    [JsonProperty("idMotivoPai")]
    public Int64? ParentId { get; set; }

    [JsonProperty("status")]
    public bool Active { get; set; }

    [JsonProperty("idClassificacaoServico")]
    public bool ServiceId { get; set; }
}
