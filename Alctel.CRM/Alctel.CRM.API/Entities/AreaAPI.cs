using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Alctel.CRM.API.Entities;

public class AreaAPI
{
    //[JsonPropertyName("idArea")]
    [JsonProperty("idArea")]
    public Int64 Id { get; set; }

    //[JsonPropertyName("nomeArea")]
    [JsonProperty("nomeArea")]
    public string? Name { get; set; }

    //[JsonPropertyName("statusArea")]
    [JsonProperty("statusArea")]
    public bool Active { get; set; }
    [JsonProperty("logs")]
    public List<LogDataReceived>? Logs { get; set; }
}
