using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alctel.CRM.Business.Model;

public class ResponseServiceModel
{
    public bool IsValid { get; set; }
    public string Value { get; set; } = string.Empty;
}
