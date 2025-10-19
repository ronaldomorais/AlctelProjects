using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Alctel.CRM.API.Entities;

public class ServiceUnitAPI
{
    //[JsonPropertyName("idUnidade")]
    [JsonProperty("idUnidade")]
    public Int64 Id { get; set; }

    //[JsonPropertyName("nomeUnidade")]
    [JsonProperty("nomeUnidade")]
    public string? Name { get; set; }

    //[JsonPropertyName("statusUnidade")]
    [JsonProperty("statusUnidade")]
    public bool Active { get; set; }
    [JsonProperty("logs")]
    public List<LogDataReceived>? Logs { get; set; }
}
