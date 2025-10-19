using System;
using System.Collections.Generic;
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

public class ClassificationDemandService : IClassificationDemandService
{
    private readonly IClassificationDemandAPIRepository _classificationDemandAPIRepository;
    public ClassificationDemandService(IClassificationDemandAPIRepository classificationDemandAPIRepository)
    {
        _classificationDemandAPIRepository = classificationDemandAPIRepository;
    }

    public async Task<List<ClassificationDemandAPI>> GetClassficationDemandListAPIAsync()
    {
        try
        {
            //var apiResponse = await _ClassficationDemandAPIRepository.GetAllDataAsync();
            var apiResponse = await _classificationDemandAPIRepository.GetClassficationDemandListAPIAsync();

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

        return new List<ClassificationDemandAPI>();
    }

    public async Task<List<ClassificationDemandAPI>> GetClassficationDemandActivatedListAPIAsync()
    {
        try
        {
            var apiResponse = await _classificationDemandAPIRepository.GetClassficationDemandActivatedListAPIAsync();

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

        return new List<ClassificationDemandAPI>();
    }

    public async Task<ClassificationDemandAPI> GetClassficationDemandAPIAsync(Int64 id)
    {
        try
        {
            var apiResponse = await _classificationDemandAPIRepository.GetClassficationDemandAPIAsync(id);

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

        return new ClassificationDemandAPI();
    }

    public async Task<bool> InsertClassficationDemandAPIAsync(ClassificationDemandAPI classificationDemandAPI)
    {
        try
        {
            if (string.IsNullOrEmpty(classificationDemandAPI.Name) == false)
            {
                var apiResponse = await _classificationDemandAPIRepository.InsertClassficationDemandAPIAsync(classificationDemandAPI.Name);

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

    public async Task<bool> UpdateClassficationDemandAPIAsync(ClassificationDemandAPI cassificationDemandAPI)
    {
        try
        {
            var apiResponse = await _classificationDemandAPIRepository.UpdateClassficationDemandAPIAsync(cassificationDemandAPI.Id, cassificationDemandAPI.Active);

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

    public async Task<List<ClassificationDemandAPI>> SearchClassficationDemandAPIAsync(string filter, string value)
    {
        try
        {
            var json = JsonConvert.SerializeObject(value);
            var apiResponse = await _classificationDemandAPIRepository.SearchDataAsync(json);

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

        return new List<ClassificationDemandAPI>();
    }
}
