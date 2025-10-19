using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alctel.CRM.Context.InMemory.Entities;

public class DemandType
{
    public Int64 Id { get; set; }
    public string? Name { get; set; }
    public bool Active { get; set; }
}
