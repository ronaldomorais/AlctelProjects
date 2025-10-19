using Microsoft.AspNetCore.Mvc.Rendering;

namespace Alctel.CRM.Web.Models;

public class ClassificationTypeModel
{
    public Int64 ClassificationTypeId { get; set; }
    public string? ClassificationTypeName { get; set; }
    public List<SelectListItem> ClassificationTypeNameOptions { get; set; } = new List<SelectListItem>();
}
