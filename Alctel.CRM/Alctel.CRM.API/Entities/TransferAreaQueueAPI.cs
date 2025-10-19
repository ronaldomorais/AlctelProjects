using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Alctel.CRM.API.Entities;

public class TransferAreaQueueAPI
{
    [JsonProperty("idArea")]
    public Int64 AreaId { get; set; }

    [JsonProperty("nome")]
    public string? AreaName { get; set; }

    [JsonProperty("idLista")]
    public Int64? ListId { get; set; }

    [JsonProperty("idFilaGT")]
    public Int64 QueueGTId { get; set; }

    [JsonProperty("status")]
    public bool AreaStatus { get; set; }

    [JsonProperty("itensLista")]
    public List<ListItemAPI>? ListItemAPICollection { get; set; }
}

public class ListItemAPI
{
    [JsonProperty("idLista")]
    public Int64? ListId { get; set; }

    [JsonProperty("idItemLista")]
    public Int64? ListItemId { get; set; }

    [JsonProperty("nomeLista")]
    public string? ListName { get; set; }

    [JsonProperty("nome")]
    public string? ListItemName { get; set; }

    [JsonProperty("statusLista")]
    public bool ListStatus { get; set; }
}
