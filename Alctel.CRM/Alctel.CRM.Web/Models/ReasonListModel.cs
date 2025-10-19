namespace Alctel.CRM.Web.Models;

public class ReasonListModel
{
    public Int64 Id { get; set; }
    public string? Name { get; set; }
    public bool Active { get; set; }
    public Int64 ClassificationListId { get; set; }
    public List<ReasonModel> Reasons { get; set; } = new List<ReasonModel>();
}
