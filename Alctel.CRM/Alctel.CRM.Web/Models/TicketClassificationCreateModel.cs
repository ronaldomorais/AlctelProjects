using Alctel.CRM.API.Entities;
using Newtonsoft.Json;

namespace Alctel.CRM.Web.Models;

public class TicketClassificationCreateModel
{
    public Int64 TicketId { get; set; }

    public Int64 ServiceId { get; set; }

    public Int64 ServiceUnitId { get; set; }

    public Int64 UserId { get; set; }
    public string? Username { get; set; }

    public Int64 Order { get; set; }

    public List<TicketReasonModel> TicketReason { get; set; } = new List<TicketReasonModel>();
}

public class TicketReasonModel
{
    [JsonProperty("idClassificacaoMotivo")]
    public Int64 ReasonId { get; set; }

    [JsonProperty("idItemListaMotivo")]
    public Int64? ListItemId { get; set; }

}
