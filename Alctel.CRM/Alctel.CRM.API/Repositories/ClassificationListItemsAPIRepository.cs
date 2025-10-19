using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alctel.CRM.API.Context;
using Alctel.CRM.API.Entities;
using Alctel.CRM.API.Interfaces;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace Alctel.CRM.API.Repositories;

public class ClassificationListItemsAPIRepository : IClassificationListItemsAPIRepository
{
    private readonly IConfiguration _configuration;

    public ClassificationListItemsAPIRepository(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task<APIResponse<List<ClassificationListItemsAPI>>> GetClassificationListItemsAPIAsync(Int64 classificationListId)
    {
        APIResponse<List<ClassificationListItemsAPI>> apiResponse = new APIResponse<List<ClassificationListItemsAPI>>();
        try
        {
            var url = _configuration.GetSection("MiddlewareTicketControl:url").Value;
            var username = _configuration.GetSection("MiddlewareTicketControl:username").Value;
            var password = _configuration.GetSection("MiddlewareTicketControl:password").Value;
            var path = _configuration.GetSection("MiddlewareTicketControl:paths:classificationlistitemlist").Value;

            ApiContext<List<ClassificationListItemsAPI>> apiContext = new ApiContext<List<ClassificationListItemsAPI>>();

            if (url != null && username != null && password != null)
            {
                apiResponse = await apiContext.PostBasicAuthAPIAsync(url, username, password, path, classificationListId.ToString());
            }
        }
        catch (Exception ex)
        {
            apiResponse.IsSuccessStatusCode = false;
            apiResponse.AdditionalMessage = ex.Message;
            apiResponse.StatusCode = System.Net.HttpStatusCode.InternalServerError;
        }

        return apiResponse;
    }

    public async Task<APIResponse<List<ClassificationListItemsAPI>>> GetClassificationListItemsActiveAPIAsync(Int64 classificationListId)
    {
        APIResponse<List<ClassificationListItemsAPI>> apiResponse = new APIResponse<List<ClassificationListItemsAPI>>();
        try
        {
            var url = _configuration.GetSection("MiddlewareTicketControl:url").Value;
            var username = _configuration.GetSection("MiddlewareTicketControl:username").Value;
            var password = _configuration.GetSection("MiddlewareTicketControl:password").Value;
            var path = _configuration.GetSection("MiddlewareTicketControl:paths:classificationlistitemactivelist").Value;

            ApiContext<List<ClassificationListItemsAPI>> apiContext = new ApiContext<List<ClassificationListItemsAPI>>();

            if (url != null && username != null && password != null)
            {
                apiResponse = await apiContext.PostBasicAuthAPIAsync(url, username, password, path, classificationListId.ToString());
            }
        }
        catch (Exception ex)
        {
            apiResponse.IsSuccessStatusCode = false;
            apiResponse.AdditionalMessage = ex.Message;
            apiResponse.StatusCode = System.Net.HttpStatusCode.InternalServerError;
        }

        return apiResponse;
    }

    public async Task<APIResponse<ClassificationListItemsAPI>> GetClassificationListItemAPIAsync(Int64 id)
    {
        APIResponse<ClassificationListItemsAPI> apiResponse = new APIResponse<ClassificationListItemsAPI>();
        try
        {
            var url = _configuration.GetSection("MiddlewareTicketControl:url").Value;
            var username = _configuration.GetSection("MiddlewareTicketControl:username").Value;
            var password = _configuration.GetSection("MiddlewareTicketControl:password").Value;
            var path = _configuration.GetSection("MiddlewareTicketControl:paths:classificationlistitemid").Value;

            ApiContext<ClassificationListItemsAPI> apiContext = new ApiContext<ClassificationListItemsAPI>();

            if (url != null && username != null && password != null)
            {
                apiResponse = await apiContext.PostBasicAuthAPIAsync(url, username, password, path, id.ToString());
            }
        }
        catch (Exception ex)
        {
            apiResponse.IsSuccessStatusCode = false;
            apiResponse.AdditionalMessage = ex.Message;
            apiResponse.StatusCode = System.Net.HttpStatusCode.InternalServerError;
        }

        return apiResponse;
    }

    public async Task<APIResponse<int>> InsertClassificationItemsAPIAsync(string data)
    {
        APIResponse<int> apiResponse = new APIResponse<int>();
        try
        {
            var url = _configuration.GetSection("MiddlewareTicketControl:url").Value;
            var username = _configuration.GetSection("MiddlewareTicketControl:username").Value;
            var password = _configuration.GetSection("MiddlewareTicketControl:password").Value;
            var path = _configuration.GetSection("MiddlewareTicketControl:paths:classificationlistiteminsert").Value;

            ApiContext<int> apiContext = new ApiContext<int>();

            if (url != null && username != null && password != null)
            {
                apiResponse = await apiContext.PostBasicAuthAPIAsync(url, username, password, path, data);
            }
        }
        catch (Exception ex)
        {
            apiResponse.IsSuccessStatusCode = false;
            apiResponse.AdditionalMessage = ex.Message;
            apiResponse.StatusCode = System.Net.HttpStatusCode.InternalServerError;
        }

        return apiResponse;
    }

    public async Task<APIResponse<bool>> UpdateClassificationItemsAPIAsync(long id, bool status)
    {
        APIResponse<bool> apiResponse = new APIResponse<bool>();
        try
        {
            var url = _configuration.GetSection("MiddlewareTicketControl:url").Value;
            var username = _configuration.GetSection("MiddlewareTicketControl:username").Value;
            var password = _configuration.GetSection("MiddlewareTicketControl:password").Value;
            var path = _configuration.GetSection("MiddlewareTicketControl:paths:classificationlistitemupdate").Value;

            ApiContext<bool> apiContext = new ApiContext<bool>();

            if (url != null && username != null && password != null)
            {
                dynamic obj = new ExpandoObject();
                obj.idListaItens = id;
                obj.statusListaItens = status;

                var json = JsonConvert.SerializeObject(obj);
                apiResponse = await apiContext.PostBasicAuthAPIAsync(url, username, password, path, json);
            }
        }
        catch (Exception ex)
        {
            apiResponse.IsSuccessStatusCode = false;
            apiResponse.AdditionalMessage = ex.Message;
            apiResponse.StatusCode = System.Net.HttpStatusCode.InternalServerError;
        }

        return apiResponse;
    }
}
