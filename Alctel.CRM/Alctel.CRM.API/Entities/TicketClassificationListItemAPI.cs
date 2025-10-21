using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Alctel.CRM.API.Entities;

public class TicketClassificationListItemAPI
{
    [JsonProperty("idLista")]
    public Int64 Id { get; set; }

    [JsonProperty("idItemLista")]
    public Int64 ListItemId { get; set; }

    [JsonProperty("nome")]
    public string? Name { get; set; }

    [JsonProperty("statusItemLista")]
    public bool Active { get; set; }

    [JsonProperty("logs")]
    public List<LogDataReceived>? Logs { get; set; }
}
