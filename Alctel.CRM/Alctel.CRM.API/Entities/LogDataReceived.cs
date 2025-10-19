using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Alctel.CRM.API.Entities;

public class LogDataReceived
{
    [JsonProperty("dataHora")]
    public DateTime LogEvent { get; set; }

    [JsonProperty("nomeCompleto")]
    public string? User { get; set; }

    [JsonProperty("modulo")]
    public string? Module { get; set; }

    [JsonProperty("secao")]
    public string? Section { get; set; }

    [JsonProperty("campo")]
    public string? Field { get; set; }

    [JsonProperty("valor")]
    public string? Value { get; set; }

    [JsonProperty("acao")]
    public string? Action { get; set; }

    [JsonProperty("descricao")]
    public string? Description { get; set; }
}
