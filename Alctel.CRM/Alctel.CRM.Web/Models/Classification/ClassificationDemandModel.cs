using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;

namespace Alctel.CRM.Web.Models;

public class ClassificationDemandModel
{
    public Int64 Id { get; set; }
    public string? Name { get; set; }
    public bool Active { get; set; }
}
