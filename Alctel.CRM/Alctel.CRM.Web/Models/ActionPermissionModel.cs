namespace Alctel.CRM.Web.Models;

public class ActionPermissionModel
{
    public Int64 Id { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public bool ActionChecked { get; set; }
    public List<ModuleModel> Modules { get; set; } = new List<ModuleModel>();
}
