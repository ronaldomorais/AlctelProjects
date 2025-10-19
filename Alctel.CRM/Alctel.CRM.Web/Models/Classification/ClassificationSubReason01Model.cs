using Microsoft.AspNetCore.Mvc.Rendering;

namespace Alctel.CRM.Web.Models;

public class ClassificationSubReason01Model
{
    public Int64 ClassificationSubReason01Id { get; set; }
    public string? ClassificationSubReason01Name { get; set; }
    public List<SelectListItem> ClassificationSubReason01NameOptions { get; set; } = new List<SelectListItem>();
}
