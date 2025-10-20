using Newtonsoft.Json;

namespace Alctel.CRM.Web.Models;

public class TicketClassificationListItemsModel
{
    public Int64 Id { get; set; }

    public Int64 ListItemId { get; set; }

    public string? ListName { get; set; }

    public string? Name { get; set; }

    public bool Status { get; set; }
}
