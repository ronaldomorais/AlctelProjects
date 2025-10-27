namespace Alctel.CRM.Web.Models;

public class TicketClassficationListModel
{
    public string? ManifestationTypeName { get; set; }

    public string? ProgramName { get; set; }

    public string? ServiceName { get; set; }

    public Int64 ServiceId { get; set; }

    public string? Reasons { get; set; }

    public bool Active { get; set; }
}
