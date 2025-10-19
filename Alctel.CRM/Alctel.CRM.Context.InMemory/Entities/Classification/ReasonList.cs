using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alctel.CRM.Context.InMemory.Entities.Classification;

public class ReasonList
{
    public Int64 Id { get; set; }
    public string? Name { get; set; }
    public bool Active { get; set; }
    public List<Reason> Reasons { get; set; } = new List<Reason>();
}
