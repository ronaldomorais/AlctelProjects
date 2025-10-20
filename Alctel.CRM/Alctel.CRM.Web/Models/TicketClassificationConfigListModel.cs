using System.ComponentModel;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Alctel.CRM.Web.Models;

public class TicketClassificationConfigListModel
{
    public Int64 ListId { get; set; }

    [DisplayName("Listas")]
    public string? Listname { get; set; }
    public List<SelectListItem> ListOptions { get; set; } = new List<SelectListItem>();

    public Int64 ListItemId { get; set; }
    public string? ListItemName { get; set; }
    public List<SelectListItem> ListItemOptions { get; set; } = new List<SelectListItem>();
}
