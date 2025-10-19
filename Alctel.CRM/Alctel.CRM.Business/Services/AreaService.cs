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

public class AreaService : IAreaService
{
    private readonly IAreaRepository _areaRepository;
    private readonly IAreaAPIRepository _areaAPIRepository;
    public AreaService(IAreaRepository AreaRepository, IAreaAPIRepository areaAPIRepository)
    {
        _areaRepository = AreaRepository;
        _areaAPIRepository = areaAPIRepository;
    }

    public async Task<List<Area>?> GetAllAreaAsync()
    {
        try
        {
            return await _areaRepository.GetAllAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Exception: {ex.Message}. Trace: {ex.StackTrace}");
        }

        return null;
    }

    public async Task<Area?> GetAreaAsync(Int64 id)
    {
        try
        {
            var itemList = await _areaRepository.FindAsync(_ => _.Id == id);

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

    public async Task<bool> CreateAreaAsync(Area data)
    {
        try
        {
            if (data != null)
            {
                data.Active = true;
                return await _areaRepository.InsertAsync(data);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Exception: {ex.Message}. Trace: {ex.StackTrace}");
        }

        return false;
    }

    public async Task<bool> UpdateAreaAsync(Area data)
    {
        try
        {
            return await _areaRepository.UpdateAsync(data);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Exception: {ex.Message}. Trace: {ex.StackTrace}");
        }

        return false;
    }

    public async Task<bool> DeleteAreaAsync(Area data)
    {
        try
        {
            return await _areaRepository.DeleteAsync(data);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Exception: {ex.Message}. Trace: {ex.StackTrace}");
        }

        return false;
    }

    public async Task<List<AreaAPI>> GetAreaListAPIAsync()
    {
        try
        {
            //var apiResponse = await _AreaAPIRepository.GetAllDataAsync();
            var apiResponse = await _areaAPIRepository.GetAreaListAPIAsync();

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

        return new List<AreaAPI>();
    }

    public async Task<List<AreaAPI>> GetAreaActivatedListAPIAsync()
    {
        try
        {
            var apiResponse = await _areaAPIRepository.GetAreaActivatedListAPIAsync();

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

        return new List<AreaAPI>();
    }

    public async Task<AreaAPI> GetAreaAPIAsync(Int64 id)
    {
        try
        {
            var apiResponse = await _areaAPIRepository.GetAreaAPIAsync(id);

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

        return new AreaAPI();
    }

    public async Task<bool> InsertAreaAPIAsync(AreaAPI AreaAPI)
    {
        try
        {
            if (string.IsNullOrEmpty(AreaAPI.Name) == false)
            {
                var apiResponse = await _areaAPIRepository.InsertAreaAPIAsync(AreaAPI.Name);

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

    public async Task<bool> UpdateAreaAPIAsync(AreaAPI AreaAPI)
    {
        try
        {
            var apiResponse = await _areaAPIRepository.UpdateAreaAPIAsync(AreaAPI.Id, AreaAPI.Active);

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

    public async Task<List<AreaAPI>> SearchAreaAPIAsync(string filter, string value)
    {
        try
        {
            var json = JsonConvert.SerializeObject(value);
            var apiResponse = await _areaAPIRepository.SearchDataAsync(json);

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

        return new List<AreaAPI>();
    }

}
