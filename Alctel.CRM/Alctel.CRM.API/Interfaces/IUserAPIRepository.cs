using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alctel.CRM.API.Entities;

namespace Alctel.CRM.API.Interfaces;

public interface IUserAPIRepository
{
    Task<APIResponse<List<UserAPI>>> GetAllDataAsync();
    Task<APIResponse<UserAPI>> GetDataAsync(Int64 id);
    Task<APIResponse<bool>> UpdateDataAsync(string data);
    Task<APIResponse<List<UserAPI>>> SearchDataAsync(string data);
    Task<APIResponse<List<AgentsAssistantsDataAPI>>> GetAgentsAssistantListAPIAsync();
}
