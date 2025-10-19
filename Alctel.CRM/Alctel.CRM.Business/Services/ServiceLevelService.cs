using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alctel.CRM.API.Entities;
using Alctel.CRM.API.Interfaces;
using Alctel.CRM.API.Repositories;
using Alctel.CRM.Business.Interfaces;
using Alctel.CRM.Context.InMemory.Entities;
using Alctel.CRM.Context.InMemory.Interfaces;
using Newtonsoft.Json;

namespace Alctel.CRM.Business.Services;

public class ServiceLevelService : IServiceLevelService
{
    private readonly IServiceLevelRepository _serviceLevelRepository;
    private readonly IServiceLevelAPIRepository _serviceLevelAPIRepository;

    public ServiceLevelService(IServiceLevelRepository serviceLevelRepository, IServiceLevelAPIRepository serviceLevelAPIRepository)
    {
        _serviceLevelRepository = serviceLevelRepository;
        _serviceLevelAPIRepository = serviceLevelAPIRepository;
    }

    public async Task<List<ServiceLevel>?> GetAllServiceLevelAsync()
    {
        try
        {
            return await _serviceLevelRepository.GetAllAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Exception: {ex.Message}. Trace: {ex.StackTrace}");
        }

        return null;
    }

    public async Task<ServiceLevel?> GetServiceLevelAsync(Int64 id)
    {
        try
        {
            var itemList = await _serviceLevelRepository.FindAsync(_ => _.Id == id);

            if (itemList != null && itemList.Any())
            {
                return itemList.FirstOrDefault();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Exception: {ex.Message}. Trace: {ex.StackTrace}");
        }

        return null;
    }

    public async Task<bool> CreateServiceLevelAsync(ServiceLevel data)
    {
        try
        {
            if (data != null)
            {
                data.Active = true;
                return await _serviceLevelRepository.InsertAsync(data);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Exception: {ex.Message}. Trace: {ex.StackTrace}");
        }

        return false;
    }

    public async Task<bool> UpdateServiceLevelAsync(ServiceLevel data)
    {
        try
        {
            return await _serviceLevelRepository.UpdateAsync(data);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Exception: {ex.Message}. Trace: {ex.StackTrace}");
        }

        return false;
    }

    public async Task<bool> DeleteServiceLevelAsync(ServiceLevel data)
    {
        try
        {
            return await _serviceLevelRepository.DeleteAsync(data);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Exception: {ex.Message}. Trace: {ex.StackTrace}");
        }

        return false;
    }

    public async Task<List<ServiceLevelAPI>> GetServiceLevelListAPIAsync()
    {
        try
        {
            //var apiResponse = await _ServiceLevelAPIRepository.GetAllDataAsync();
            var apiResponse = await _serviceLevelAPIRepository.GetServiceLevelListAPIAsync();

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

        return new List<ServiceLevelAPI>();
    }

    public async Task<List<ServiceLevelAPI>> GetServiceLevelActivatedListAPIAsync()
    {
        try
        {
            var apiResponse = await _serviceLevelAPIRepository.GetServiceLevelActivatedListAPIAsync();

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

        return new List<ServiceLevelAPI>();
    }
    public async Task<ServiceLevelAPI> GetServiceLevelAPIAsync(Int64 id)
    {
        try
        {
            var apiResponse = await _serviceLevelAPIRepository.GetServiceLevelAPIAsync(id);

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

        return new ServiceLevelAPI();
    }

    public async Task<bool> InsertServiceLevelAPIAsync(ServiceLevelAPI ServiceLevelAPI)
    {
        try
        {
            if (string.IsNullOrEmpty(ServiceLevelAPI.Name) == false)
            {
                var apiResponse = await _serviceLevelAPIRepository.InsertServiceLevelAPIAsync(ServiceLevelAPI.Name);

                if (apiResponse.IsSuccessStatusCode)
                {
                    return apiResponse.Response;
                }

            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Exception: {ex.Message}. Trace: {ex.StackTrace}");
        }

        return false;
    }

    public async Task<bool> UpdateServiceLevelAPIAsync(ServiceLevelAPI ServiceLevelAPI)
    {
        try
        {
            var apiResponse = await _serviceLevelAPIRepository.UpdateServiceLevelAPIAsync(ServiceLevelAPI.Id, ServiceLevelAPI.Active);

            if (apiResponse.IsSuccessStatusCode)
            {
                return apiResponse.Response;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Exception: {ex.Message}. Trace: {ex.StackTrace}");
        }

        return false;
    }

    public async Task<List<ServiceLevelAPI>> SearchServiceLevelAPIAsync(string filter, string value)
    {
        try
        {
            var json = JsonConvert.SerializeObject(value);
            var apiResponse = await _serviceLevelAPIRepository.SearchDataAsync(json);

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

        return new List<ServiceLevelAPI>();
    }
}
