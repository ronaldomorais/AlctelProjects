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

public class ClassificationReasonService : IClassificationReasonService
{
    private readonly IClassificationReasonAPIRepository _classificationReasonAPIRepository;
    public ClassificationReasonService(IClassificationReasonAPIRepository classificationReasonAPIRepository)
    {
        _classificationReasonAPIRepository = classificationReasonAPIRepository;
    }

    public async Task<List<ClassificationReasonAPI>> GetClassificationReasonListAPIAsync()
    {
        try
        {
            //var apiResponse = await _ClassificationReasonAPIRepository.GetAllDataAsync();
            var apiResponse = await _classificationReasonAPIRepository.GetClassificationReasonListAPIAsync();

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

        return new List<ClassificationReasonAPI>();
    }

    public async Task<List<ClassificationReasonAPI>> GetClassificationReasonActivatedListAPIAsync()
    {
        try
        {
            var apiResponse = await _classificationReasonAPIRepository.GetClassificationReasonActivatedListAPIAsync();

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

        return new List<ClassificationReasonAPI>();
    }

    public async Task<List<ClassificationReasonAPI>> GetClassificationReasonAPIAsync(Int64 id)
    {
        try
        {
            var apiResponse = await _classificationReasonAPIRepository.GetClassificationReasonAPIAsync(id);

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

        return new List<ClassificationReasonAPI>();
    }

    public async Task<bool> InsertClassificationReasonAPIAsync(ClassificationReasonAPI ClassificationReasonAPI)
    {
        try
        {
            if (string.IsNullOrEmpty(ClassificationReasonAPI.Name) == false)
            {
                var apiResponse = await _classificationReasonAPIRepository.InsertClassificationReasonAPIAsync(ClassificationReasonAPI.Name);

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

    public async Task<bool> UpdateClassificationReasonAPIAsync(ClassificationReasonAPI ClassificationReasonAPI)
    {
        try
        {
            var apiResponse = await _classificationReasonAPIRepository.UpdateClassificationReasonAPIAsync(ClassificationReasonAPI.Id, ClassificationReasonAPI.Active);

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

    public async Task<List<ClassificationReasonAPI>> SearchClassificationReasonAPIAsync(string filter, string value)
    {
        try
        {
            var json = JsonConvert.SerializeObject(value);
            var apiResponse = await _classificationReasonAPIRepository.SearchDataAsync(json);

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

        return new List<ClassificationReasonAPI>();
    }

    public async Task<List<ClassificationReasonChildrenAPI>> GetClassificationReasonListChildrenAPIAsync(Int64 reasonid)
    {
        try
        {
            //var apiResponse = await _ClassificationReasonAPIRepository.GetAllDataAsync();
            var apiResponse = await _classificationReasonAPIRepository.GetClassificationReasonListChildrenAPIAsync(reasonid);

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

        return new List<ClassificationReasonChildrenAPI>();
    }
}
