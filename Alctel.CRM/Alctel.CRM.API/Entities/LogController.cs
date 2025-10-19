using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alctel.CRM.API.Entities;

public class LogController
{
    public Int64 Id { get; set; }
    public string? Module { get; set; }
    public string? Section { get; set; }
    public string? Field { get; set; }
    public string? Value { get; set; }
    public string? Action { get; set; }
    public Int64 UserId { get; set; }
    public string? Description { get; set; }
}
