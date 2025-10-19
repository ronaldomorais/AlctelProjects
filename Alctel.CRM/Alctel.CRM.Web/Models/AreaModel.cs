namespace Alctel.CRM.Web.Models;

public class AreaModel
{
    public Int64 Id { get; set; }
    public string? Name { get; set; }
    public bool Active { get; set; }
    public List<LogDataReceivedModel>? Logs { get; set; }
    public AreaDataToCompareIfChangedLog AreaDataToCompareIfChanged { get; set; } = new AreaDataToCompareIfChangedLog();
}


public class AreaDataToCompareIfChangedLog
{
    public Int64 Id { get; set; }
    public bool Active { get; set; }
}
