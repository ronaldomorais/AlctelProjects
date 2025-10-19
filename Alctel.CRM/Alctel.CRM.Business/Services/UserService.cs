using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Alctel.CRM.API.Entities;
using Alctel.CRM.API.Interfaces;
using Alctel.CRM.Business.Interfaces;
using Alctel.CRM.Context.InMemory.Entities;
using Alctel.CRM.Context.InMemory.Interfaces;

namespace Alctel.CRM.Business.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IUserAPIRepository _userAPIRepository;

    public UserService(IUserRepository userRepository, IUserAPIRepository userAPIRepository)
    {
        _userRepository = userRepository;
        _userAPIRepository = userAPIRepository;
    }

    public async Task<List<User>?> GetAllUserAsync()
    {
        try
        {
            //return await _userRepository.GetAllAsync();
            return await _userRepository.GetAllUsersAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Exception: {ex.Message}. Trace: {ex.StackTrace}");
        }

        return null;
    }

    public async Task<User?> GetUserAsync(Int64 id)
    {
        try
        {
            var customers = await _userRepository.FindAsync(_ => _.Id == id);

            if (customers != null && customers.Any())
            {
                return customers.FirstOrDefault();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Exception: {ex.Message}. Trace: {ex.StackTrace}");
        }

        return null;
    }

    public async Task<bool> CreateUserAsync(User user)
    {
        try
        {
            if (user != null)
            {
                return await _userRepository.InsertAsync(user);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Exception: {ex.Message}. Trace: {ex.StackTrace}");
        }

        return false;
    }

    public async Task<bool> UpdateUserAsync(User user)
    {
        try
        {
            return await _userRepository.UpdateAsync(user);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Exception: {ex.Message}. Trace: {ex.StackTrace}");
        }

        return false;
    }

    public async Task<bool> DeleteUserAsync(User user)
    {
        try
        {
            return await _userRepository.DeleteAsync(user);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Exception: {ex.Message}. Trace: {ex.StackTrace}");
        }

        return false;
    }

    public async Task<List<UserAPI>> GetAllUsersAPIAsync()
    {
        try
        {
            var apiResponse = await _userAPIRepository.GetAllDataAsync();

            if (apiResponse.IsSuccessStatusCode)
            {
                if (apiResponse.Response != null)
                    return apiResponse.Response;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Exception: {ex.Message}. Trace: {ex.StackTrace}");
        }

        return new List<UserAPI>();
    }


    public async Task<UserAPI> GetUserAPIAsync(Int64 id)
    {
        try
        {
            var apiResponse = await _userAPIRepository.GetDataAsync(id);

            if (apiResponse.IsSuccessStatusCode)
            {
                if (apiResponse.Response != null)
                    return apiResponse.Response;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Exception: {ex.Message}. Trace: {ex.StackTrace}");
        }

        return new UserAPI();
    }

    public async Task<bool> UpdateUserAPIAsync(UserAPI user)
    {
        try
        {
            dynamic userObj = new ExpandoObject();
            userObj.idUsuario = user.Id;
            userObj.statusUsuario = user.Active;
            userObj.nomeUnidade = user.Unit;
            userObj.areaUsuario = user.Area;
            userObj.perfilUsuario = user.AccessProfile;
            userObj.nomeCompleto = user.Fullname;
            userObj.email = user.Email;
            userObj.idGenesys = user.GenesysId == null ? string.Empty : user.GenesysId;
            userObj.nomeFilaGT = user.QueueGT;

            var userjson = JsonSerializer.Serialize(userObj);

            var ret = await _userAPIRepository.UpdateDataAsync(userjson);

            if (ret != null)
            {
                return ret.Response;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Exception: {ex.Message}. Trace: {ex.StackTrace}");
        }

        return false;
    }

    public async Task<List<UserAPI>> SearchUserAPIAsync(string filter, string value)
    {
        try
        {
            dynamic obj = new ExpandoObject();
            obj.filtro = filter;
            obj.valor = value;

            var json = JsonSerializer.Serialize(obj);

            var apiResponse = await _userAPIRepository.SearchDataAsync(json);

            if (apiResponse.IsSuccessStatusCode)
            {
                if (apiResponse.Response != null)
                    return apiResponse.Response;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Exception: {ex.Message}. Trace: {ex.StackTrace}");
        }

        return new List<UserAPI>();
    }

    public async Task<List<AgentsAssistantsDataAPI>> GetAgentsAssistantListAsync()
    {
        try
        {
            var apiResponse = await _userAPIRepository.GetAgentsAssistantListAPIAsync();

            if (apiResponse.IsSuccessStatusCode)
            {
                if (apiResponse.Response != null)
                    return apiResponse.Response;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Exception: {ex.Message}. Trace: {ex.StackTrace}");
        }

        return new List<AgentsAssistantsDataAPI>();
    }
}
