namespace Alctel.CRM.Web.Models;

public class TicketClassificationUnitModel
{
    public Int64 ListId { get; set; }

    public Int64 ListItemId { get; set; }

    public string? ListName { get; set; }

    public string? ListItemName { get; set; }

    public bool Active { get; set; }
}
