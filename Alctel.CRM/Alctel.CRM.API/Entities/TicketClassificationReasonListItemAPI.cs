using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Alctel.CRM.API.Entities;

public class TicketClassificationReasonListItemAPI
{
    [JsonProperty("idClassificacaoMotivo")]
    public Int64 Id { get; set; }

    [JsonProperty("idLista")]
    public Int64 ListId { get; set; }

    [JsonProperty("idListaItem")]
    public Int64 ListItemId { get; set; }

    [JsonProperty("idMotivoPai")]
    public Int64? ParentId { get; set; }

    [JsonProperty("nomeItemLista")]
    public string? ListItemName { get; set; }
}
