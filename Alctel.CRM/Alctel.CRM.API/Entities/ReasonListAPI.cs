using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Alctel.CRM.API.Entities;

public class ReasonListAPI
{
    //[JsonPropertyName("idArea")]
    [JsonProperty("idMotivo")]
    public Int64 Id { get; set; }

    //[JsonPropertyName("nomeArea")]
    [JsonProperty("nomeMotivo")]
    public string? Name { get; set; }

    //[JsonPropertyName("statusArea")]
    [JsonProperty("statusMotivo")]
    public bool Active { get; set; }

    [JsonProperty("idClassificacaoListaId")]
    public Int64 ClassificationListId { get; set; }

    [JsonProperty("logs")]
    public List<LogDataReceived>? Logs { get; set; }
}
