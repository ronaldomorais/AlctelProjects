namespace Alctel.CRM.Web.Models;

public class ModuleModel
{
    public Int64 Id { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public bool ModuleChecked { get; set; }
    //public Guid? ActionPermissionId { get; set; }
    public List<ActionPermissionModel>? ActionPermissions { get; set; }
}
