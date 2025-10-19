namespace Alctel.CRM.Context.InMemory.Entities;

public class AccessProfile
{
    public Int64 Id { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public List<Module> Modules { get; set; } = new List<Module>();
    public List<User> Users { get; set; } = new List<User>();
}