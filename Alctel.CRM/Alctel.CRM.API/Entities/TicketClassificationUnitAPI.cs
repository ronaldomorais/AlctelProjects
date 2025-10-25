using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Alctel.CRM.API.Entities;

public class TicketClassificationUnitAPI
{
    [JsonProperty("idLista")]
    public Int64 ListId { get; set; }

    [JsonProperty("idItemLista")]
    public Int64 ListItemId { get; set; }

    [JsonProperty("nomeLista")]
    public string? ListName { get; set; }

    [JsonProperty("nome")]
    public string? ListItemName { get; set; }

    [JsonProperty("statusLista")]
    public bool Active { get; set; }
}
