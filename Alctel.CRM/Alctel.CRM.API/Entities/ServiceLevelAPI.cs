using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Alctel.CRM.API.Entities;

public class ServiceLevelAPI
{
    //[JsonPropertyName("idNivel")]
    [JsonProperty("idNivel")]
    public Int64 Id { get; set; }

    //[JsonPropertyName("nomeNivel")]
    [JsonProperty("nomeNivel")]
    public string? Name { get; set; }

    //[JsonPropertyName("statusNivel")]
    [JsonProperty("statusNivel")]
    public bool Active { get; set; }
    [JsonProperty("logs")]
    public List<LogDataReceived>? Logs { get; set; }
}
