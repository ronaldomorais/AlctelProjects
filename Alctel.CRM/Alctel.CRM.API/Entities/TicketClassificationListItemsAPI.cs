using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Alctel.CRM.API.Entities;

public class TicketClassificationListItemsAPI
{
    [JsonProperty("idLista")]
    public Int64 Id { get; set; }

    [JsonProperty("idItemLista")]
    public Int64 ListItemId { get; set; }

    [JsonProperty("nomeLista")]
    public string? ListName { get; set; }

    [JsonProperty("nome")]
    public string? Name { get; set; }

    [JsonProperty("statusLista")]
    public bool Status { get; set; }
}
