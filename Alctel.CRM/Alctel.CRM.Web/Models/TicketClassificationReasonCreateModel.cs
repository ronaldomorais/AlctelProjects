using Alctel.CRM.API.Entities;
using Newtonsoft.Json;

namespace Alctel.CRM.Web.Models;

public class TicketClassificationReasonCreateModel
{
    public Int64 ManifestationTypeId { get; set; }

    public Int64 ProgramId { get; set; }

    public string? ServiceName { get; set; }

    public List<TicketReasonCreateModel> ticketReason { get; set; } = new List<TicketReasonCreateModel>();
}

public class TicketReasonCreateModel
{
    public Int64? ListId { get; set; }

    public Int64? ParentId { get; set; }

    public string? ReasonName { get; set; }

    public bool Status { get; set; }
}

