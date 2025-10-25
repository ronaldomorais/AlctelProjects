using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Alctel.CRM.API.Entities;

public class TicketClassficationListAPI
{
    [JsonProperty("tipoManifestacao")]
    public string? ManifestationTypeName { get; set; }

    [JsonProperty("programa")]
    public string? ProgramName { get; set; }

    [JsonProperty("servico")]
    public string? ServiceName { get; set; }

    [JsonProperty("motivoCompleto")]
    public string? Reasons { get; set; }
}
