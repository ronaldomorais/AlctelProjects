using System;
using System.Collections.Generic;
using System.Dynamic;
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

public class ClassificationListService : IClassificationListService
{
    private readonly IClassificationListAPIRepository _classificationListAPIRepository;
    public ClassificationListService(IClassificationListAPIRepository ClassificationListAPIRepository)
    {
        _classificationListAPIRepository = ClassificationListAPIRepository;
    }

    public async Task<List<ClassificationListAPI>> GetClassificationListListAPIAsync()
    {
        try
        {
            //var apiResponse = await _ClassificationListAPIRepository.GetAllDataAsync();
            var apiResponse = await _classificationListAPIRepository.GetClassificationListListAPIAsync();

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

        return new List<ClassificationListAPI>();
    }



    //public async Task<List<ClassificationListAPI>> GetClassificationListActivatedListAPIAsync()
    //{
    //    try
    //    {
    //        var apiResponse = await _classificationListAPIRepository.GetClassificationListActivatedListAPIAsync();

    //        if (apiResponse.IsSuccessStatusCode)
    //        {
    //            if (apiResponse.Response != null)
    //                return apiResponse.Response;
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        Console.WriteLine($"Exception: {ex.Message}. Trace: {ex.StackTrace}");
    //    }

    //    return new List<ClassificationListAPI>();
    //}

    //public async Task<ClassificationListAPI> GetClassificationListAPIAsync(Int64 id)
    //{
    //    try
    //    {
    //        var apiResponse = await _classificationListAPIRepository.GetClassificationListAPIAsync(id);

    //        if (apiResponse.IsSuccessStatusCode)
    //        {
    //            if (apiResponse.Response != null)
    //                return apiResponse.Response;
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        Console.WriteLine($"Exception: {ex.Message}. Trace: {ex.StackTrace}");
    //    }

    //    return new ClassificationListAPI();
    //}

    //public async Task<bool> InsertClassificationListAPIAsync(ClassificationListAPI ClassificationListAPI)
    //{
    //    try
    //    {
    //        if (string.IsNullOrEmpty(ClassificationListAPI.Name) == false)
    //        {
    //            var apiResponse = await _classificationListAPIRepository.InsertClassificationListAPIAsync(ClassificationListAPI.Name);

    //            if (apiResponse.IsSuccessStatusCode)
    //            {
    //                return apiResponse.Response;
    //            }

    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        Console.WriteLine($"Exception: {ex.Message}. Trace: {ex.StackTrace}");
    //    }

    //    return false;
    //}

    //public async Task<bool> UpdateClassificationListAPIAsync(ClassificationListAPI ClassificationListAPI)
    //{
    //    try
    //    {
    //        var apiResponse = await _classificationListAPIRepository.UpdateClassificationListAPIAsync(ClassificationListAPI.Id, ClassificationListAPI.Active);

    //        if (apiResponse.IsSuccessStatusCode)
    //        {
    //            return apiResponse.Response;
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        Console.WriteLine($"Exception: {ex.Message}. Trace: {ex.StackTrace}");
    //    }

    //    return false;
    //}

    //public async Task<List<ClassificationListAPI>> SearchClassificationListAPIAsync(string filter, string value)
    //{
    //    try
    //    {
    //        dynamic obj = new ExpandoObject();
    //        obj.chave = filter;
    //        obj.valor = value;

    //        var json = JsonConvert.SerializeObject(obj);

    //        var apiResponse = await _classificationListAPIRepository.SearchDataAsync(json);

    //        if (apiResponse.IsSuccessStatusCode)
    //        {
    //            if (apiResponse.Response != null)
    //                return apiResponse.Response;
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        Console.WriteLine($"Exception: {ex.Message}. Trace: {ex.StackTrace}");
    //    }

    //    return new List<ClassificationListAPI>();
    //}
}
