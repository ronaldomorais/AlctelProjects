using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alctel.CRM.API.Entities;
using Alctel.CRM.API.Interfaces;
using Alctel.CRM.Business.Interfaces;
using Alctel.CRM.Context.InMemory.Entities;
using Alctel.CRM.Context.InMemory.Interfaces;
using Newtonsoft.Json;

namespace Alctel.CRM.Business.Services;

public class ClassificationDemandTypeService : IClassificationDemandTypeService
{
    private readonly IClassificationDemandTypeAPIRepository _classificationDemandTypeAPIRepository;
    public ClassificationDemandTypeService(IClassificationDemandTypeAPIRepository classificationDemandTypeAPIRepository)
    {
        _classificationDemandTypeAPIRepository = classificationDemandTypeAPIRepository;
    }

    public async Task<List<ClassificationDemandTypeAPI>> GetClassificationDemandTypeListAPIAsync()
    {
        try
        {
            //var apiResponse = await _ClassificationDemandTypeAPIRepository.GetAllDataAsync();
            var apiResponse = await _classificationDemandTypeAPIRepository.GetClassificationDemandTypeListAPIAsync();

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

        return new List<ClassificationDemandTypeAPI>();
    }

    public async Task<List<ClassificationDemandTypeAPI>> GetClassificationDemandTypeActivatedListAPIAsync()
    {
        try
        {
            var apiResponse = await _classificationDemandTypeAPIRepository.GetClassificationDemandTypeActivatedListAPIAsync();

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

        return new List<ClassificationDemandTypeAPI>();
    }

    public async Task<List<ClassificationDemandTypeAPI>> GetClassificationDemandTypeListAPIAsync(Int64 id)
    {
        try
        {
            var apiResponse = await _classificationDemandTypeAPIRepository.GetClassificationDemandTypeListAPIAsync(id);

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

        return new List<ClassificationDemandTypeAPI>();
    }

    public async Task<bool> InsertClassificationDemandTypeAPIAsync(ClassificationDemandTypeAPI ClassificationDemandTypeAPI)
    {
        try
        {
            if (string.IsNullOrEmpty(ClassificationDemandTypeAPI.Name) == false)
            {
                var apiResponse = await _classificationDemandTypeAPIRepository.InsertClassificationDemandTypeAPIAsync(ClassificationDemandTypeAPI.Name);

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

    public async Task<bool> UpdateClassificationDemandTypeAPIAsync(ClassificationDemandTypeAPI ClassificationDemandTypeAPI)
    {
        try
        {
            var apiResponse = await _classificationDemandTypeAPIRepository.UpdateClassificationDemandTypeAPIAsync(ClassificationDemandTypeAPI.Id, ClassificationDemandTypeAPI.Active);

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

    public async Task<List<ClassificationDemandTypeAPI>> SearchClassificationDemandTypeAPIAsync(string filter, string value)
    {
        try
        {
            var json = JsonConvert.SerializeObject(value);
            var apiResponse = await _classificationDemandTypeAPIRepository.SearchDataAsync(json);

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

        return new List<ClassificationDemandTypeAPI>();
    }
}
