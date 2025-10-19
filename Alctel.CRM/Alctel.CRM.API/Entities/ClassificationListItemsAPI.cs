using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Alctel.CRM.API.Entities;

public class ClassificationListItemsAPI
{
    [JsonProperty("idLista")]
    public Int64 ClassificationListId { get; set; }

    [JsonProperty("idItemLista")]
    public Int64 Id { get; set; }

    [JsonProperty("nomeLista")]
    public string? ClassificationListName { get; set; }

    [JsonProperty("nome")]
    public string? Name { get; set; }

    [JsonProperty("statusLista")]
    public bool Active { get; set; }

    [JsonProperty("statusItemLista")]
    public bool Active2 { get; set; }

    [JsonProperty("logs")]
    public List<LogDataReceived>? Logs { get; set; }
}
