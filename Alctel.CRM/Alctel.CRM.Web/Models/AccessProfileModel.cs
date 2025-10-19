namespace Alctel.CRM.Web.Models;

public class AccessProfileModel
{
    public Int64 Id { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public List<ModuleModel> Modules { get; set; } = new List<ModuleModel>();
}
