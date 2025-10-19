using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alctel.CRM.Context.InMemory.Entities;

public class User
{
    public Int64 Id { get; set; }
    public string? Fullname { get; set; }
    public string? Email { get; set; }
    public string? Unit { get; set; }
    public string? Area { get; set; }
    public DateTime CreatedOn { get; set; }
    public bool Active { get; set; }
    public DateTime StatusSince { get; set; }
    public List<AccessProfile> AccessProfiles { get; set; } = new List<AccessProfile>();
}
