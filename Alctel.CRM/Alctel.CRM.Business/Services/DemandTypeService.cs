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

public class DemandTypeService : IDemandTypeService
{
    private readonly IDemandTypeRepository _demandTypeRepository;
    private readonly IDemandTypeAPIRepository _demandTypeAPIRepository;

    public DemandTypeService(IDemandTypeRepository DemandTypeRepository, IDemandTypeAPIRepository demandTypeAPIRepository)
    {
        _demandTypeRepository = DemandTypeRepository;
        _demandTypeAPIRepository = demandTypeAPIRepository;
    }

    public async Task<List<DemandType>?> GetAllDemandTypeAsync()
    {
        try
        {
            return await _demandTypeRepository.GetAllAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Exception: {ex.Message}. Trace: {ex.StackTrace}");
        }

        return null;
    }

    public async Task<DemandType?> GetDemandTypeAsync(Int64 id)
    {
        try
        {
            var itemList = await _demandTypeRepository.FindAsync(_ => _.Id == id);

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

    public async Task<bool> CreateDemandTypeAsync(DemandType data)
    {
        try
        {
            if (data != null)
            {
                data.Active = true;
                return await _demandTypeRepository.InsertAsync(data);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Exception: {ex.Message}. Trace: {ex.StackTrace}");
        }

        return false;
    }

    public async Task<bool> UpdateDemandTypeAsync(DemandType data)
    {
        try
        {
            return await _demandTypeRepository.UpdateAsync(data);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Exception: {ex.Message}. Trace: {ex.StackTrace}");
        }

        return false;
    }

    public async Task<bool> DeleteDemandTypeAsync(DemandType data)
    {
        try
        {
            return await _demandTypeRepository.DeleteAsync(data);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Exception: {ex.Message}. Trace: {ex.StackTrace}");
        }

        return false;
    }

    public async Task<List<DemandTypeAPI>> GetDemandTypeListAPIAsync()
    {
        try
        {
            //var apiResponse = await _DemandTypeAPIRepository.GetAllDataAsync();
            var apiResponse = await _demandTypeAPIRepository.GetDemandTypeListAPIAsync();

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

        return new List<DemandTypeAPI>();
    }

    public async Task<List<DemandTypeAPI>> GetDemandTypeActivatedListAPIAsync()
    {
        try
        {
            var apiResponse = await _demandTypeAPIRepository.GetDemandTypeActivatedListAPIAsync();

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

        return new List<DemandTypeAPI>();
    }

    public async Task<DemandTypeAPI> GetDemandTypeAPIAsync(Int64 id)
    {
        try
        {
            var apiResponse = await _demandTypeAPIRepository.GetDemandTypeAPIAsync(id);

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

        return new DemandTypeAPI();
    }

    public async Task<bool> InsertDemandTypeAPIAsync(DemandTypeAPI DemandTypeAPI)
    {
        try
        {
            if (string.IsNullOrEmpty(DemandTypeAPI.Name) == false)
            {
                var apiResponse = await _demandTypeAPIRepository.InsertDemandTypeAPIAsync(DemandTypeAPI.Name);

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

    public async Task<bool> UpdateDemandTypeAPIAsync(DemandTypeAPI DemandTypeAPI)
    {
        try
        {
            var apiResponse = await _demandTypeAPIRepository.UpdateDemandTypeAPIAsync(DemandTypeAPI.Id, DemandTypeAPI.Active);

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

    public async Task<List<DemandTypeAPI>> SearchDemandTypeAPIAsync(string filter, string value)
    {
        try
        {
            var json = JsonConvert.SerializeObject(value);
            var apiResponse = await _demandTypeAPIRepository.SearchDataAsync(json);

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

        return new List<DemandTypeAPI>();
    }
}
