using Newtonsoft.Json;

namespace Alctel.CRM.Web.Models;

public class LogDataReceivedModel
{
    public DateTime LogEvent { get; set; }
    public string? User { get; set; }
    public string? Module { get; set; }
    public string? Section { get; set; }
    public string? Field { get; set; }
    public string? Value { get; set; }
    public string? Action { get; set; }
    public string? Description { get; set; }
}
