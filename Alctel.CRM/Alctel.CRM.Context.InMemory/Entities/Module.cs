using System.ComponentModel.DataAnnotations.Schema;

namespace Alctel.CRM.Context.InMemory.Entities;

public class Module
{
    public Int64 Id { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public bool ModuleChecked { get; set; } = true;
    public List<ActionPermission> ActionPermissions { get; set; } = new List<ActionPermission>();
    public List<AccessProfile> AccessProfiles { get; set; } = new List<AccessProfile>();
}