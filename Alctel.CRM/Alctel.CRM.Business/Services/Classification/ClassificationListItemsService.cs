using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alctel.CRM.API.Entities;
using Alctel.CRM.API.Interfaces;
using Alctel.CRM.API.Repositories;
using Alctel.CRM.Business.Interfaces.Classification;
using Newtonsoft.Json;

namespace Alctel.CRM.Business.Services.Classification;

public class ClassificationListItemsService : IClassificationListItemsService
{
    private readonly IClassificationListItemsAPIRepository _classificationListItemsAPIRepository;

    public ClassificationListItemsService(IClassificationListItemsAPIRepository classificationListItemsAPIRepository)
    {
        _classificationListItemsAPIRepository = classificationListItemsAPIRepository;
    }

    public async Task<ClassificationListItemsAPI> GetClassificationListItemAPIAsync(long classificationListId)
    {
        try
        {
            var apiResponse = await _classificationListItemsAPIRepository.GetClassificationListItemAPIAsync(classificationListId);

            if (apiResponse.IsSuccessStatusCode)
            {
                if (apiResponse.Response != null)
                {
                    apiResponse.Response.Active = apiResponse.Response.Active2;
                    return apiResponse.Response;
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Exception: {ex.Message}. Trace: {ex.StackTrace}");
        }

        return new ClassificationListItemsAPI();
    }

    public async Task<List<ClassificationListItemsAPI>> GetClassificationListItemsListAPIAsync(Int64 classificationListId)
    {
        try
        {
            //var apiResponse = await _ClassificationListAPIRepository.GetAllDataAsync();
            var apiResponse = await _classificationListItemsAPIRepository.GetClassificationListItemsAPIAsync(classificationListId);

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

        return new List<ClassificationListItemsAPI>();
    }

    public async Task<List<ClassificationListItemsAPI>> GetClassificationListItemsActiveAPIAsync(Int64 classificationListId)
    {
        try
        {
            //var apiResponse = await _ClassificationListAPIRepository.GetAllDataAsync();
            var apiResponse = await _classificationListItemsAPIRepository.GetClassificationListItemsActiveAPIAsync(classificationListId);

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

        return new List<ClassificationListItemsAPI>();
    }

    public async Task<int> InsertClassificationListItemsAPIAsync(ClassificationListItemsAPI classificationListItemsAPI)
    {
        try
        {
            if (string.IsNullOrEmpty(classificationListItemsAPI.Name) == false)
            {
                dynamic obj = new ExpandoObject();
                obj.idLista = classificationListItemsAPI.ClassificationListId;
                obj.nomeLista = classificationListItemsAPI.Name;

                var json = JsonConvert.SerializeObject(obj);

                var apiResponse = await _classificationListItemsAPIRepository.InsertClassificationItemsAPIAsync(json);


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

        return 0;
    }

    public async Task<bool> UpdateClassificationListItemsAPIAsync(ClassificationListItemsAPI classificationListItemsAPI)
    {
        try
        {
            var apiResponse = await _classificationListItemsAPIRepository.UpdateClassificationItemsAPIAsync(classificationListItemsAPI.Id, classificationListItemsAPI.Active);

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
}
