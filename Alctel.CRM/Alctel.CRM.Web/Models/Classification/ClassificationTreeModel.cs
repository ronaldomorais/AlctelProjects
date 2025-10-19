using Microsoft.AspNetCore.Mvc.Rendering;

namespace Alctel.CRM.Web.Models;

public class ClassificationTreeModel
{
    public string? ClassificationDemandId { get; set; }
    public List<SelectListItem> ClassificationDemandOptions { get; set; } = new List<SelectListItem>();

    public string? ClassificationDemandTypeId { get; set; }
    public List<SelectListItem> ClassificationDemandTypeOptions { get; set; } = new List<SelectListItem>();

    public string? ClassificationReasonId { get; set; }
    public List<SelectListItem> ClassificationReasonOptions { get; set; } = new List<SelectListItem>();

    public string? ClassificationReasonListItemId { get; set; }
    public List<SelectListItem> ClassificationReasonListItemOptions { get; set; } = new List<SelectListItem>();

    public string? ClassificationSubmotive01Id { get; set; }
    public List<SelectListItem> ClassificationSubmotive01Options { get; set; } = new List<SelectListItem>();

    public string? ClassificationSubmotive01ListItemId { get; set; }
    public List<SelectListItem> ClassificationSubmotive01ListItemOptions { get; set; } = new List<SelectListItem>();

    public string? ClassificationSubmotive02Id { get; set; }
    public List<SelectListItem> ClassificationSubmotive02Options { get; set; } = new List<SelectListItem>();

    public string? ClassificationSubmotive02ListItemId { get; set; }
    public List<SelectListItem> ClassificationSubmotive02ListItemOptions { get; set; } = new List<SelectListItem>();
}
