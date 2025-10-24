using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Alctel.CRM.API.Entities;

public class TicketClassificationServiceAPI
{
    [JsonProperty("idClassificacaoServico")]
    public Int64 Id { get; set; }

    [JsonProperty("nome")]
    public string? Name { get; set; }

    [JsonProperty("status")]
    public bool Active { get; set; }

    [JsonProperty("idClassificacaoPrograma")]
    public Int64 ProgramId { get; set; }

    [JsonProperty("idClassificacaoManifestacao")]
    public Int64? ManifestationTypeId { get; set; }
}
