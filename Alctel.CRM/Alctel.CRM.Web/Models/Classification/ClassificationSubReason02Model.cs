using Microsoft.AspNetCore.Mvc.Rendering;

namespace Alctel.CRM.Web.Models;

public class ClassificationSubReason02Model
{
    public Int64 ClassificationSubReason02Id { get; set; }
    public string? ClassificationSubReason02Name { get; set; }
    public List<SelectListItem> ClassificationSubReason02NameOptions { get; set; } = new List<SelectListItem>();
}
