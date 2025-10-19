using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alctel.CRM.API.Entities;

namespace Alctel.CRM.API.Interfaces;

public interface IServiceLevelAPIRepository
{
    Task<APIResponse<List<ServiceLevelAPI>>> GetServiceLevelListAPIAsync();
    Task<APIResponse<List<ServiceLevelAPI>>> GetServiceLevelActivatedListAPIAsync();
    Task<APIResponse<ServiceLevelAPI>> GetServiceLevelAPIAsync(Int64 id);
    Task<APIResponse<bool>> InsertServiceLevelAPIAsync(string data);
    Task<APIResponse<bool>> UpdateServiceLevelAPIAsync(Int64 id, bool status);
    Task<APIResponse<List<ServiceLevelAPI>>> SearchDataAsync(string data);
}
