using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Alctel.CRM.API.Entities;

public class SlaTicketCreateAPI
{
    [JsonProperty("idClassificacaoTipoManifestacao")]
    public Int64 ManifestationTypeId { get; set; }

    [JsonProperty("idClassificacaoServico")]
    public Int64 ServiceId { get; set; }

    [JsonProperty("idCriticidade")]

    public Int64 CriticalityId { get; set; }

    [JsonProperty("sla")]
    public Int64 Sla { get; set; }

    [JsonProperty("alarme")]
    public Int64 Alarm { get; set; }

    [JsonProperty("motivos")]
    public List<SlaReasonAPI> SlaReasons { get; set; } = new List<SlaReasonAPI>();
}

public class SlaReasonAPI
{
    [JsonProperty("idClassificacaoMotivo")]
    public Int64 ReasonId { get; set; }

    [JsonProperty("idLista")]
    public Int64 ListId { get; set; }

    [JsonProperty("idListaItemMotivo")]
    public Int64 ListItemId { get; set; }
}