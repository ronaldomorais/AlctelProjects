using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alctel.CRM.API.Entities;

namespace Alctel.CRM.API.Interfaces;

public interface IAccessProfileAPIRepository
{
    Task<APIResponse<List<AccessProfileAPI>>> GetAllDataAsync();
    Task<APIResponse<List<AccessProfileAPI>>> GetAccessProfileActivatedListAPIAsync();
}
