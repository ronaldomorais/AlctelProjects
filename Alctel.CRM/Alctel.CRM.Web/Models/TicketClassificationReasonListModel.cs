namespace Alctel.CRM.Web.Models;

public class TicketClassificationReasonListModel
{
    public Int64 Id { get; set; }

    public Int64 ManifestationTypeId { get; set; }

    public string? Name { get; set; }

    public Int64 ListId { get; set; }

    public Int64? ParentId { get; set; }

    public bool Active { get; set; }
}
