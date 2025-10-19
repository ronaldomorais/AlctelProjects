using Newtonsoft.Json;

namespace Alctel.CRM.Web.Models.Classification;

public class ClassificationDemandTypeModel
{
    public Int64 Id { get; set; }
    public Int64 DemandId { get; set; }
    public string? Name { get; set; }
    public bool Active { get; set; }
}
