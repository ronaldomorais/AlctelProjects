using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alctel.CRM.API.Entities;
using Alctel.CRM.Context.InMemory.Entities;

namespace Alctel.CRM.Business.Interfaces;

public interface IUserService
{
    Task<List<User>?> GetAllUserAsync();
    Task<User?> GetUserAsync(Int64 Id);
    Task<bool> CreateUserAsync(User user);
    Task<bool> UpdateUserAsync(User user);
    Task<bool> DeleteUserAsync(User user);

    Task<List<UserAPI>> GetAllUsersAPIAsync();
    Task<UserAPI> GetUserAPIAsync(Int64 id);
    Task<bool> UpdateUserAPIAsync(UserAPI user);
    Task<List<UserAPI>> SearchUserAPIAsync(string filter, string value);
    Task<List<AgentsAssistantsDataAPI>> GetAgentsAssistantListAsync();
}
