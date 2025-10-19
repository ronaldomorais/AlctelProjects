using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alctel.CRM.API.Entities;

namespace Alctel.CRM.API.Interfaces;

public interface IAreaAPIRepository
{
    Task<APIResponse<List<AreaAPI>>> GetAreaListAPIAsync();
    Task<APIResponse<List<AreaAPI>>> GetAreaActivatedListAPIAsync();
    Task<APIResponse<AreaAPI>> GetAreaAPIAsync(Int64 id);
    Task<APIResponse<bool>> InsertAreaAPIAsync(string data);
    Task<APIResponse<bool>> UpdateAreaAPIAsync(Int64 id, bool status);
    Task<APIResponse<List<AreaAPI>>> SearchDataAsync(string data);
    
}
