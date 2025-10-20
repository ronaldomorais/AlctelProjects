using Newtonsoft.Json;

namespace Alctel.CRM.Web.Models;

public class TicketClassificationListModel
{
    public Int64 Id { get; set; }

    public string? Name { get; set; }

    public bool Status { get; set; }
}
