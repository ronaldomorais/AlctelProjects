using System.ComponentModel;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Alctel.CRM.Web.Models;

public class TicketClassificationModel
{
    [DisplayName("Manifestação")]
    public Int64 ManifestationTypeId { get; set; }
    public string? ManifestationTypeName { get; set; }
    public List<SelectListItem> ManifestationTypeOptions { get; set; } = new List<SelectListItem>();

    [DisplayName("Unidade")]
    public Int64 ServiceUnitId { get; set; }
    public string? ServiceUnitName { get; set; }
    public List<SelectListItem> ServiceUnitOptions { get; set; } = new List<SelectListItem>();

    [DisplayName("Serviço")]
    public Int64 ServiceId { get; set; }

    [DisplayName("Serviço")]
    public string? ServiceName { get; set; }
    public List<SelectListItem> ServiceOptions { get; set; } = new List<SelectListItem>();

    [DisplayName("Programa")]
    public Int64 ProgramId { get; set; }
    public string? ProgramName { get; set; }
    public List<SelectListItem> ProgramOptions { get; set; } = new List<SelectListItem>();

    [DisplayName("Motivo 1")]
    public Int64 Reason01Id { get; set; }
    public string? Reason01Name { get; set; }

    public Int64? Reason01ListId { get; set; }
    public string? Reason01ListName { get; set; }
    public List<SelectListItem> Reason01Options { get; set; } = new List<SelectListItem>();

    [DisplayName("Motivo 1")]
    public Int64? Reason01ListItemId { get; set; }
    public string? Reason01ListItemName { get; set; }

    [DisplayName("Motivo 2")]
    public Int64 Reason02Id { get; set; }
    public string? Reason02Name { get; set; }

    public Int64? Reason02ListId { get; set; }
    public string? Reason02ListName { get; set; }
    public List<SelectListItem> Reason02Options { get; set; } = new List<SelectListItem>();

    [DisplayName("Motivo 2")]
    public Int64? Reason02ListItemId { get; set; }
    public string? Reason02ListItemName { get; set; }

    [DisplayName("Criticidade")]
    public Int64 CriticalityId { get; set; }
    public string? CriticalityIdName { get; set; }
    public List<SelectListItem> CriticalityOptions { get; set; } = new List<SelectListItem>();

    [DisplayName("SLA (em dias)")]
    public int SlaInDays { get; set; }

    [DisplayName("Alarmar (em dias)")]
    public int Alarm { get; set; }
}
