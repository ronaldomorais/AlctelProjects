namespace Alctel.CRM.Web.Models;

public class ServiceLevelModel
{
    public Int64 Id { get; set; }
    public string? Name { get; set; }
    public bool Active { get; set; }
    public List<LogDataReceivedModel>? Logs { get; set; }
    public ServiceLevelDataToCompareIfChangedLog ServiceLevelDataToCompareIfChanged { get; set; } = new ServiceLevelDataToCompareIfChangedLog();
}


public class ServiceLevelDataToCompareIfChangedLog
{
    public Int64 Id { get; set; }
    public bool Active { get; set; }
}
