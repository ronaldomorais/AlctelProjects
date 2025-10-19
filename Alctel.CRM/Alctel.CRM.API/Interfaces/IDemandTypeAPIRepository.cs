using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alctel.CRM.API.Entities;

namespace Alctel.CRM.API.Interfaces;

public interface IDemandTypeAPIRepository
{
    Task<APIResponse<List<DemandTypeAPI>>> GetDemandTypeListAPIAsync();
    Task<APIResponse<List<DemandTypeAPI>>> GetDemandTypeActivatedListAPIAsync();
    Task<APIResponse<DemandTypeAPI>> GetDemandTypeAPIAsync(Int64 id);
    Task<APIResponse<bool>> InsertDemandTypeAPIAsync(string data);
    Task<APIResponse<bool>> UpdateDemandTypeAPIAsync(Int64 id, bool status);
    Task<APIResponse<List<DemandTypeAPI>>> SearchDataAsync(string data);
}
