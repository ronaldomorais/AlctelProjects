using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Alctel.CRM.API.Entities;

public class TicketClassificationAPI
{
    [JsonProperty("idChamado")]
    public Int64 TicketId { get; set; }

    [JsonProperty("idClassificacaoServico")]
    public Int64 ServiceId { get; set; }

    [JsonProperty("idListaItemUnidade")]
    public Int64 ServiceUnitId { get; set; }

    [JsonProperty("idUsuario")]
    public Int64 UserId { get; set; }

    [JsonProperty("ordem")]
    public Int64 Order { get; set; }

    [JsonProperty("motivos")]
    public List<TicketReasonAPI> TicketReason { get; set; } = new List<TicketReasonAPI>();
}

public class TicketReasonAPI
{
    [JsonProperty("idClassificacaoMotivo")]
    public Int64 ReasonId { get; set; }

    [JsonProperty("idItemListaMotivo")]
    public Int64? ListItemId { get; set; }

}
