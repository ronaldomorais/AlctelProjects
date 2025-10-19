using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Alctel.CRM.API.Entities;

public class DemandTypeAPI
{
    //[JsonPropertyName("idDemanda")]
    [JsonProperty("idMotivoDemanda")]
    public Int64 Id { get; set; }

    //[JsonPropertyName("nomeDemanda")]
    [JsonProperty("nomeMotivoDemanda")]
    public string? Name { get; set; }

    //[JsonPropertyName("statusDemanda")]
    [JsonProperty("statusMotivoDemanda")]
    public bool Active { get; set; }
    [JsonProperty("logs")]
    public List<LogDataReceived>? Logs { get; set; }
}
