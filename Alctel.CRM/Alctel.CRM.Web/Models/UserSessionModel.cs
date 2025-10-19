namespace Alctel.CRM.Web.Models;

public class UserSessionModel
{
    public bool IsAuthenticated { get; set; }
    public string? Profile { get; set; }
    public string? Name { get; set; }
    public string? UserName { get; set; }
    public string? Modules { get; set; }
}

public class UserSessionResponse
{
    public bool IsAuthenticated { get; set; }
    public string? RedirectTo { get; set; }
}
