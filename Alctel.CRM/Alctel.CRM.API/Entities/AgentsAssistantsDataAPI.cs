using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Alctel.CRM.API.Entities;

public class AgentsAssistantsDataAPI
{
    [JsonProperty("idUsuario")]
    public Int64 UserId { get; set; }

    [JsonProperty("nomeUsuario")]
    public string? Username { get; set; }
}
