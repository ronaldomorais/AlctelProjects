using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alctel.CRM.Business.Interfaces;

public interface IConfigService
{
    string GetGenesysClientId();
    string GetBaseUrl(string physicalPath);
}
