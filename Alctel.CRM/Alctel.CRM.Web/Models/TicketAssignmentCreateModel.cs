namespace Alctel.CRM.Web.Models;

public class TicketAssignmentCreateModel
{
    public Int64 UserOriginId { get; set; }
    public string? UsernameOrigin { get; set; }
    public Int64 UserDestId { get; set; }
    public Int64 QueueGTId { get; set; }
    public string? QueueGTOrigin { get; set; }
    public Int64 TicketId { get; set; }
    public string? AssignmentTypeId { get; set; }
}
