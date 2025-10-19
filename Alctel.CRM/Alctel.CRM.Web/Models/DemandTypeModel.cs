namespace Alctel.CRM.Web.Models;

public class DemandTypeModel
{
    public Int64 Id { get; set; }
    public string? Name { get; set; }
    public bool Active { get; set; }
    public List<LogDataReceivedModel>? Logs { get; set; }
    public DemandTypeDataToCompareIfChangedLog DemandTypeDataToCompareIfChanged { get; set; } = new DemandTypeDataToCompareIfChangedLog();
}


public class DemandTypeDataToCompareIfChangedLog
{
    public Int64 Id { get; set; }
    public bool Active { get; set; }
}
