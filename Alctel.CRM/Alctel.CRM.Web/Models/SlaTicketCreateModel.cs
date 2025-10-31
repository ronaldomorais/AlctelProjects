namespace Alctel.CRM.Web.Models;

public class SlaTicketCreateModel
{
    public Int64 ManifestationTypeId { get; set; }

    public Int64 ServiceId { get; set; }

    public Int64 CriticalityId { get; set; }

    public Int64 Sla { get; set; }

    public Int64 Alarm { get; set; }

    public List<SlaReasonModel> SlaReasons { get; set; } = new List<SlaReasonModel>();
}

public class SlaReasonModel
{
    public Int64 ReasonId { get; set; }
    public Int64 ListId { get; set; }
    public Int64 ListItemId { get; set; }
}
