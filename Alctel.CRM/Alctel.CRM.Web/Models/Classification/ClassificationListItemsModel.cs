using Newtonsoft.Json;

namespace Alctel.CRM.Web.Models;

public class ClassificationListItemsModel
{
    public Int64 ClassificationListId { get; set; }
    public Int64 Id { get; set; }
    public string? ClassificationListName { get; set; }
    public string? Name { get; set; }
    public bool Active { get; set; }
    public List<LogDataReceivedModel>? Logs { get; set; }
    public ClassificationListItemsDataToCompareIfChangedLog ClassificationListItemsDataToCompareIfChanged { get; set; } = new ClassificationListItemsDataToCompareIfChangedLog();
}

public class ClassificationListItemsDataToCompareIfChangedLog
{
    public Int64 Id { get; set; }
    public bool Active { get; set; }
}
