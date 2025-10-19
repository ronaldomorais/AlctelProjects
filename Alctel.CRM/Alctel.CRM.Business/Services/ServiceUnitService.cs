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

public class ServiceUnitService : IServiceUnitService
{
    private readonly IServiceUnitRepository _serviceUnitRepository;
    private readonly IServiceUnitAPIRepository _serviceUnitAPIRepository;
    public ServiceUnitService(IServiceUnitRepository serviceUnitRepository, IServiceUnitAPIRepository serviceUnitAPIRepository)
    {
        _serviceUnitRepository = serviceUnitRepository;
        _serviceUnitAPIRepository = serviceUnitAPIRepository;
    }

    public async Task<List<ServiceUnit>?> GetAllServiceUnitAsync()
    {
        try
        {
            return await _serviceUnitRepository.GetAllAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Exception: {ex.Message}. Trace: {ex.StackTrace}");
        }

        return null;
    }

    public async Task<ServiceUnit?> GetServiceUnitAsync(Int64 id)
    {
        try
        {
            var itemList = await _serviceUnitRepository.FindAsync(_ => _.Id == id);

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

    public async Task<bool> CreateServiceUnitAsync(ServiceUnit data)
    {
        try
        {
            if (data != null)
            {
                data.Active = true;
                return await _serviceUnitRepository.InsertAsync(data);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Exception: {ex.Message}. Trace: {ex.StackTrace}");
        }

        return false;
    }

    public async Task<bool> UpdateServiceUnitAsync(ServiceUnit data)
    {
        try
        {
            return await _serviceUnitRepository.UpdateAsync(data);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Exception: {ex.Message}. Trace: {ex.StackTrace}");
        }

        return false;
    }

    public async Task<bool> DeleteServiceUnitAsync(ServiceUnit data)
    {
        try
        {
            return await _serviceUnitRepository.DeleteAsync(data);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Exception: {ex.Message}. Trace: {ex.StackTrace}");
        }

        return false;
    }

    public async Task<List<ServiceUnitAPI>> GetServiceUnitListAPIAsync()
    {
        try
        {
            //var apiResponse = await _serviceUnitAPIRepository.GetAllDataAsync();
            var apiResponse = await _serviceUnitAPIRepository.GetServiceUnitListAPIAsync();

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

        return new List<ServiceUnitAPI>();
    }

    public async Task<List<ServiceUnitAPI>> GetServiceUnitActivatedListAPIAsync()
    {
        try
        {
            var apiResponse = await _serviceUnitAPIRepository.GetServiceUnitActivatedListAPIAsync();

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

        return new List<ServiceUnitAPI>();
    }

    public async Task<ServiceUnitAPI> GetServiceUnitAPIAsync(Int64 id)
    {
        try
        {
            var apiResponse = await _serviceUnitAPIRepository.GetServiceUnitAPIAsync(id);

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

        return new ServiceUnitAPI();
    }

    public async Task<bool> InsertServiceUnitAPIAsync(ServiceUnitAPI serviceUnitAPI)
    {
        try
        {
            if (string.IsNullOrEmpty(serviceUnitAPI.Name) == false)
            {
                var apiResponse = await _serviceUnitAPIRepository.InsertServiceUnitAPIAsync(serviceUnitAPI.Name);

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

    public async Task<bool> UpdateServiceUnitAPIAsync(ServiceUnitAPI serviceUnitAPI)
    {
        try
        {
            var apiResponse = await _serviceUnitAPIRepository.UpdateServiceUnitAPIAsync(serviceUnitAPI.Id, serviceUnitAPI.Active);

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

    public async Task<List<ServiceUnitAPI>> SearchServiceUnitAPIAsync(string filter, string value)
    {
        try
        {
            var json = JsonConvert.SerializeObject(value);
            var apiResponse = await _serviceUnitAPIRepository.SearchDataAsync(json);

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

        return new List<ServiceUnitAPI>();
    }
}
