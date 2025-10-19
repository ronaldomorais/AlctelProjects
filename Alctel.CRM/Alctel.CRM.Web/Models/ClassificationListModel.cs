namespace Alctel.CRM.Web.Models;

public class ClassificationListModel
{
    public Int64 Id { get; set; }
    public string? Name { get; set; }
    public bool Active { get; set; }
    public List<LogDataReceivedModel>? Logs { get; set; }
    public ClassificationListDataToCompareIfChangedLog ClassificationListDataToCompareIfChanged { get; set; } = new ClassificationListDataToCompareIfChangedLog();
}

public class ClassificationListDataToCompareIfChangedLog
{
    public Int64 Id { get; set; }
    public bool Active { get; set; }
}
