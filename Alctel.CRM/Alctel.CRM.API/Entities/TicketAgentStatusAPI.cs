using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Alctel.CRM.API.Entities;

public class TicketAgentStatusAPI
{
    [JsonProperty("idStatus")]
    public Int64 Id { get; set; }

    [JsonProperty("nome")]
    public string? Name { get; set; }
}
