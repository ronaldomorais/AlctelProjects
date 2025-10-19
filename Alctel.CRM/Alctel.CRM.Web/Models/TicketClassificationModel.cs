using Microsoft.AspNetCore.Mvc.Rendering;

namespace Alctel.CRM.Web.Models;

public class TicketClassificationModel
{
    public Int64 ManifestationTypeId { get; set; }
    public string? ManifestationTypeName { get; set; }
    public List<SelectListItem> ManifestationTypeOptions { get; set; } = new List<SelectListItem>();

    public Int64 ServiceUnitId { get; set; }
    public string? ServiceUnitName { get; set; }
    public List<SelectListItem> ServiceUnitOptions { get; set; } = new List<SelectListItem>();

    public Int64 ServiceId { get; set; }
    public string? ServiceName { get; set; }
    public List<SelectListItem> ServiceOptions { get; set; } = new List<SelectListItem>();

    public Int64 Reason01Id { get; set; }
    public string? Reason01Name { get; set; }
    public List<SelectListItem> Reason01Options { get; set; } = new List<SelectListItem>();

    public Int64 Reason02Id { get; set; }
    public string? Reason02Name { get; set; }
    public List<SelectListItem> Reason02Options { get; set; } = new List<SelectListItem>();
}
