using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alctel.CRM.API.Entities;

namespace Alctel.CRM.API.Interfaces;

public interface IServiceUnitAPIRepository
{
    Task<APIResponse<List<ServiceUnitAPI>>> GetServiceUnitListAPIAsync();
    Task<APIResponse<List<ServiceUnitAPI>>> GetServiceUnitActivatedListAPIAsync();
    Task<APIResponse<ServiceUnitAPI>> GetServiceUnitAPIAsync(Int64 id);
    Task<APIResponse<bool>> InsertServiceUnitAPIAsync(string data);
    Task<APIResponse<bool>> UpdateServiceUnitAPIAsync(Int64 id, bool status);
    Task<APIResponse<List<ServiceUnitAPI>>> SearchDataAsync(string data);
}
