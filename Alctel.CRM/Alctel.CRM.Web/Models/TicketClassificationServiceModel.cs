namespace Alctel.CRM.Web.Models;

public class TicketClassificationServiceModel
{
    public Int64 Id { get; set; }

    public string? Name { get; set; }

    public bool Active { get; set; }

    public Int64 ProgramId { get; set; }

    public Int64? ManifestationTypeId { get; set; }
}
