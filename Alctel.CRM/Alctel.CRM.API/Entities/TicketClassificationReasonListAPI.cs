using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Alctel.CRM.API.Entities;

public class TicketClassificationReasonListAPI
{
    [JsonProperty("idClassMotivo")]
    public Int64 Id { get; set; }

    [JsonProperty("idClassTipoManifestacao")]
    public Int64 ManifestationTypeId { get; set; }

    [JsonProperty("motivoClassMotivo")]
    public string? Name { get; set; }

    [JsonProperty("idListaClassMotivo")]
    public Int64 ListId { get; set; }

    [JsonProperty("idMotivoPai")]
    public Int64? ParentId { get; set; }

    [JsonProperty("statusClassMotivo")]
    public bool Active { get; set; }
}
