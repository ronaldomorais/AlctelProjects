using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alctel.CRM.API.Entities;
using Alctel.CRM.API.Interfaces;
using Alctel.CRM.Business.Interfaces.Reason;
using Alctel.CRM.Context.InMemory.Entities.Classification;
using Alctel.CRM.Context.InMemory.Interfaces;
using Alctel.CRM.Context.InMemory.Interfaces.Classification;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace Alctel.CRM.Business.Services.Reason;

public class ReasonListService : IReasonListService
{
    private readonly IReasonListRepository _ReasonListRepository;
    private readonly IReasonListAPIRepository _reasonListAPIRepository;

    public ReasonListService(IReasonListRepository ReasonListRepository, IReasonListAPIRepository reasonListAPIRepository)
    {
        _ReasonListRepository = ReasonListRepository;
        _reasonListAPIRepository = reasonListAPIRepository;
    }

    public async Task<List<ReasonList>?> GetAllReasonListAsync()
    {
        try
        {
            return await _ReasonListRepository.GetAllAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Exception: {ex.Message}. Trace: {ex.StackTrace}");
        }

        return null;
    }

    public async Task<ReasonList?> GetReasonListAsync(Int64 id)
    {
        try
        {
            var itemList = await _ReasonListRepository.FindAsync(_ => _.Id == id, _ => _.Include(_ => _.Reasons));

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

    public async Task<bool> CreateReasonListAsync(ReasonList data)
    {
        try
        {
            if (data != null)
            {
                data.Active = true;
                return await _ReasonListRepository.InsertAsync(data);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Exception: {ex.Message}. Trace: {ex.StackTrace}");
        }

        return false;
    }

    public async Task<bool> UpdateReasonListAsync(ReasonList data)
    {
        try
        {
            return await _ReasonListRepository.UpdateAsync(data);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Exception: {ex.Message}. Trace: {ex.StackTrace}");
        }

        return false;
    }

    public async Task<bool> DeleteReasonListAsync(ReasonList data)
    {
        try
        {
            return await _ReasonListRepository.DeleteAsync(data);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Exception: {ex.Message}. Trace: {ex.StackTrace}");
        }

        return false;
    }

    public async Task<List<ReasonListAPI>> GetReasonListListAPIAsync()
    {
        try
        {
            //var apiResponse = await _ReasonListAPIRepository.GetAllDataAsync();
            var apiResponse = await _reasonListAPIRepository.GetReasonListListAPIAsync();

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

        return new List<ReasonListAPI>();
    }

    public async Task<List<ReasonListAPI>> GetReasonListActivatedListAPIAsync()
    {
        try
        {
            var apiResponse = await _reasonListAPIRepository.GetReasonListActivatedListAPIAsync();

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

        return new List<ReasonListAPI>();
    }

    public async Task<ReasonListAPI> GetReasonListAPIAsync(Int64 id)
    {
        try
        {
            var apiResponse = await _reasonListAPIRepository.GetReasonListAPIAsync(id);

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

        return new ReasonListAPI();
    }

    public async Task<bool> InsertReasonListAPIAsync(ReasonListAPI ReasonListAPI)
    {
        try
        {
            if (string.IsNullOrEmpty(ReasonListAPI.Name) == false)
            {
                var apiResponse = await _reasonListAPIRepository.InsertReasonListAPIAsync(ReasonListAPI.Name);

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

    public async Task<bool> UpdateReasonListAPIAsync(ReasonListAPI ReasonListAPI)
    {
        try
        {
            var apiResponse = await _reasonListAPIRepository.UpdateReasonListAPIAsync(ReasonListAPI.Id, ReasonListAPI.Active);

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

    public async Task<List<ReasonListAPI>> SearchReasonListAPIAsync(string filter, string value)
    {
        try
        {
            dynamic obj = new ExpandoObject();
            obj.chave = filter;
            obj.valor = value;

            var json = JsonConvert.SerializeObject(obj);

            var apiResponse = await _reasonListAPIRepository.SearchDataAsync(json);

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

        return new List<ReasonListAPI>();
    }
}
