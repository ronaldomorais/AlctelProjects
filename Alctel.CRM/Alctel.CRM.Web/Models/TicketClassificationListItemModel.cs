using Alctel.CRM.API.Entities;
using Newtonsoft.Json;

namespace Alctel.CRM.Web.Models;

public class TicketClassificationListItemModel
{
    public Int64 Id { get; set; }

    public Int64 ListItemId { get; set; }

    public string? Name { get; set; }

    public bool Active { get; set; }

    public List<LogDataReceived>? Logs { get; set; }
    public ListItemDataToCompareIfChangedLog ListItemDataToCompareIfChanged { get; set; } = new ListItemDataToCompareIfChangedLog();

}

public class ListItemDataToCompareIfChangedLog
{
    public Int64 Id { get; set; }
    public bool Active { get; set; }
}
