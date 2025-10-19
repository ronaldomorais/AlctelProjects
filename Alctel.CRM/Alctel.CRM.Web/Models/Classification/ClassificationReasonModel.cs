using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;

namespace Alctel.CRM.Web.Models;

public class ClassificationReasonModel
{
    public Int64 Id { get; set; }
    public Int64 ClassificationDemandTypeId { get; set; }
    public string? Name { get; set; }
    public Int64 ClassificationListReasonId { get; set; }
    public Int64 ParentReasonId { get; set; }
    public bool Active { get; set; }
}
