namespace Alctel.CRM.Web.Models;

public class SlaTicketConfigModel
{
    public Int64 SlaTicketId { get; set; }

    public string? ManifestationTypeName { get; set; }

    public string? ServiceName { get; set; }

    public string? Criticality { get; set; }

    public string? Reasons { get; set; }

    public string? Sla { get; set; }

    public string? Alarm { get; set; }
}
