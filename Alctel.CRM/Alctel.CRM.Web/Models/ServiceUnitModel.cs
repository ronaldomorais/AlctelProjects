namespace Alctel.CRM.Web.Models;

public class ServiceUnitModel
{
    public Int64 Id { get; set; }
    public string? Name { get; set; }
    public bool Active { get; set; }
    public List<LogDataReceivedModel>? Logs { get; set; }
    public ServiceUnitDataToCompareIfChangedLog ServiceUnitDataToCompareIfChanged { get; set; } = new ServiceUnitDataToCompareIfChangedLog();
}


public class ServiceUnitDataToCompareIfChangedLog
{
    public Int64 Id { get; set; }
    public bool Active { get; set; }
}
