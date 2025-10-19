namespace Alctel.CRM.Context.InMemory.Entities;

public class ActionPermission
{
    public Int64 Id { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public bool ActionChecked { get; set; } = true;
    public List<Module> Modules { get; set; } = new List<Module>();
}