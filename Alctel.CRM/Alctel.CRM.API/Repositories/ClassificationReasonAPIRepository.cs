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

public class ClassificationReasonAPIRepository : IClassificationReasonAPIRepository
{
    private readonly IConfiguration _configuration;
    public ClassificationReasonAPIRepository(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task<APIResponse<List<ClassificationReasonAPI>>> GetClassificationReasonListAPIAsync()
    {
        APIResponse<List<ClassificationReasonAPI>> apiResponse = new APIResponse<List<ClassificationReasonAPI>>();
        try
        {
            var url = _configuration.GetSection("MiddlewareTicketControl:url").Value;
            var username = _configuration.GetSection("MiddlewareTicketControl:username").Value;
            var password = _configuration.GetSection("MiddlewareTicketControl:password").Value;
            var path = _configuration.GetSection("MiddlewareTicketControl:paths:classificationreasonlist").Value;

            ApiContext<List<ClassificationReasonAPI>> apiContext = new ApiContext<List<ClassificationReasonAPI>>();

            if (url != null && username != null && password != null)
            {
                apiResponse = await apiContext.PostBasicAuthAPIAsync(url, username, password, path);
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

    public async Task<APIResponse<List<ClassificationReasonAPI>>> GetClassificationReasonActivatedListAPIAsync()
    {
        APIResponse<List<ClassificationReasonAPI>> apiResponse = new APIResponse<List<ClassificationReasonAPI>>();
        try
        {
            var url = _configuration.GetSection("MiddlewareTicketControl:url").Value;
            var username = _configuration.GetSection("MiddlewareTicketControl:username").Value;
            var password = _configuration.GetSection("MiddlewareTicketControl:password").Value;
            var path = _configuration.GetSection("MiddlewareTicketControl:paths:classificationreasonactivatedlist").Value;

            ApiContext<List<ClassificationReasonAPI>> apiContext = new ApiContext<List<ClassificationReasonAPI>>();

            if (url != null && username != null && password != null)
            {
                apiResponse = await apiContext.PostBasicAuthAPIAsync(url, username, password, path);
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

    public async Task<APIResponse<List<ClassificationReasonAPI>>> GetClassificationReasonAPIAsync(Int64 id)
    {
        APIResponse<List<ClassificationReasonAPI>> apiResponse = new APIResponse<List<ClassificationReasonAPI>>();
        try
        {
            var url = _configuration.GetSection("MiddlewareTicketControl:url").Value;
            var username = _configuration.GetSection("MiddlewareTicketControl:username").Value;
            var password = _configuration.GetSection("MiddlewareTicketControl:password").Value;
            var path = _configuration.GetSection("MiddlewareTicketControl:paths:classificationreasonid").Value;

            ApiContext<List<ClassificationReasonAPI>> apiContext = new ApiContext<List<ClassificationReasonAPI>>();

            if (url != null && username != null && password != null)
            {
                var json = JsonConvert.SerializeObject(id);
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


    public async Task<APIResponse<bool>> InsertClassificationReasonAPIAsync(string data)
    {
        APIResponse<bool> apiResponse = new APIResponse<bool>();
        try
        {
            var url = _configuration.GetSection("MiddlewareTicketControl:url").Value;
            var username = _configuration.GetSection("MiddlewareTicketControl:username").Value;
            var password = _configuration.GetSection("MiddlewareTicketControl:password").Value;
            var path = _configuration.GetSection("MiddlewareTicketControl:paths:classificationreasoninsert").Value;

            ApiContext<bool> apiContext = new ApiContext<bool>();

            if (url != null && username != null && password != null)
            {
                var json = JsonConvert.SerializeObject(data);
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

    public async Task<APIResponse<bool>> UpdateClassificationReasonAPIAsync(Int64 id, bool status)
    {
        APIResponse<bool> apiResponse = new APIResponse<bool>();
        try
        {
            var url = _configuration.GetSection("MiddlewareTicketControl:url").Value;
            var username = _configuration.GetSection("MiddlewareTicketControl:username").Value;
            var password = _configuration.GetSection("MiddlewareTicketControl:password").Value;
            var path = _configuration.GetSection("MiddlewareTicketControl:paths:classificationreasonupdate").Value;

            ApiContext<bool> apiContext = new ApiContext<bool>();

            if (url != null && username != null && password != null)
            {
                dynamic obj = new ExpandoObject();
                obj.idClassificationReason = id;
                obj.statusClassificationReason = status;

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

    public async Task<APIResponse<List<ClassificationReasonAPI>>> SearchDataAsync(string data)
    {
        APIResponse<List<ClassificationReasonAPI>> apiResponse = new APIResponse<List<ClassificationReasonAPI>>();
        try
        {
            var url = _configuration.GetSection("MiddlewareTicketControl:url").Value;
            var username = _configuration.GetSection("MiddlewareTicketControl:username").Value;
            var password = _configuration.GetSection("MiddlewareTicketControl:password").Value;
            var path = _configuration.GetSection("MiddlewareTicketControl:paths:classificationreasonsearch").Value;

            url = url != null ? url.EndsWith("/") ? url.Substring(0, url.Length - 1) : url : url;
            path = path != null ? path.StartsWith("/") ? path : $"/{path}" : path;
            var fullurl = $"{url}{path}";

            var basicAuth = Convert.ToBase64String(ASCIIEncoding.ASCII.GetBytes($"{username}:{password}"));

            HeaderRequest headers = new HeaderRequest();
            headers.Add("Authorization", $"Basic {basicAuth}");
            headers.Add("User-Agent", "Alctel.CRM");

            ApiContext<List<ClassificationReasonAPI>> apiContext = new ApiContext<List<ClassificationReasonAPI>>();
            apiResponse = await apiContext.PostRequest(fullurl, "application/json", headers, data);
        }
        catch (Exception ex)
        {
            apiResponse.IsSuccessStatusCode = false;
            apiResponse.AdditionalMessage = ex.Message;
            apiResponse.StatusCode = System.Net.HttpStatusCode.InternalServerError;
        }

        return apiResponse;
    }

    public async Task<APIResponse<List<ClassificationReasonChildrenAPI>>> GetClassificationReasonListChildrenAPIAsync(Int64 reasondid)
    {
        APIResponse<List<ClassificationReasonChildrenAPI>> apiResponse = new APIResponse<List<ClassificationReasonChildrenAPI>>();
        try
        {
            var url = _configuration.GetSection("MiddlewareTicketControl:url").Value;
            var username = _configuration.GetSection("MiddlewareTicketControl:username").Value;
            var password = _configuration.GetSection("MiddlewareTicketControl:password").Value;
            var path = _configuration.GetSection("MiddlewareTicketControl:paths:classificationreasonchildren").Value;

            ApiContext<List<ClassificationReasonChildrenAPI>> apiContext = new ApiContext<List<ClassificationReasonChildrenAPI>>();

            if (url != null && username != null && password != null)
            {
                apiResponse = await apiContext.PostBasicAuthAPIAsync(url, username, password, path, reasondid.ToString());
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
