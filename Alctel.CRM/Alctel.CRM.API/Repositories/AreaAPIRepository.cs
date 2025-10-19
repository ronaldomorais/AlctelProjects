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

public class AreaAPIRepository : IAreaAPIRepository
{
    private readonly IConfiguration _configuration;
    public AreaAPIRepository(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task<APIResponse<List<AreaAPI>>> GetAreaListAPIAsync()
    {
        APIResponse<List<AreaAPI>> apiResponse = new APIResponse<List<AreaAPI>>();
        try
        {
            var url = _configuration.GetSection("MiddlewareTicketControl:url").Value;
            var username = _configuration.GetSection("MiddlewareTicketControl:username").Value;
            var password = _configuration.GetSection("MiddlewareTicketControl:password").Value;
            var path = _configuration.GetSection("MiddlewareTicketControl:paths:arealist").Value;

            ApiContext<List<AreaAPI>> apiContext = new ApiContext<List<AreaAPI>>();

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

    public async Task<APIResponse<List<AreaAPI>>> GetAreaActivatedListAPIAsync()
    {
        APIResponse<List<AreaAPI>> apiResponse = new APIResponse<List<AreaAPI>>();
        try
        {
            var url = _configuration.GetSection("MiddlewareTicketControl:url").Value;
            var username = _configuration.GetSection("MiddlewareTicketControl:username").Value;
            var password = _configuration.GetSection("MiddlewareTicketControl:password").Value;
            var path = _configuration.GetSection("MiddlewareTicketControl:paths:areaactivatedlist").Value;

            ApiContext<List<AreaAPI>> apiContext = new ApiContext<List<AreaAPI>>();

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

    public async Task<APIResponse<AreaAPI>> GetAreaAPIAsync(Int64 id)
    {
        APIResponse<AreaAPI> apiResponse = new APIResponse<AreaAPI>();
        try
        {
            var url = _configuration.GetSection("MiddlewareTicketControl:url").Value;
            var username = _configuration.GetSection("MiddlewareTicketControl:username").Value;
            var password = _configuration.GetSection("MiddlewareTicketControl:password").Value;
            var path = _configuration.GetSection("MiddlewareTicketControl:paths:areaid").Value;

            ApiContext<AreaAPI> apiContext = new ApiContext<AreaAPI>();

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


    public async Task<APIResponse<bool>> InsertAreaAPIAsync(string data)
    {
        APIResponse<bool> apiResponse = new APIResponse<bool>();
        try
        {
            var url = _configuration.GetSection("MiddlewareTicketControl:url").Value;
            var username = _configuration.GetSection("MiddlewareTicketControl:username").Value;
            var password = _configuration.GetSection("MiddlewareTicketControl:password").Value;
            var path = _configuration.GetSection("MiddlewareTicketControl:paths:areainsert").Value;

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

    public async Task<APIResponse<bool>> UpdateAreaAPIAsync(Int64 id, bool status)
    {
        APIResponse<bool> apiResponse = new APIResponse<bool>();
        try
        {
            var url = _configuration.GetSection("MiddlewareTicketControl:url").Value;
            var username = _configuration.GetSection("MiddlewareTicketControl:username").Value;
            var password = _configuration.GetSection("MiddlewareTicketControl:password").Value;
            var path = _configuration.GetSection("MiddlewareTicketControl:paths:areaupdate").Value;

            ApiContext<bool> apiContext = new ApiContext<bool>();

            if (url != null && username != null && password != null)
            {
                dynamic obj = new ExpandoObject();
                obj.idArea = id;
                obj.statusArea = status;

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

    public async Task<APIResponse<List<AreaAPI>>> SearchDataAsync(string data)
    {
        APIResponse<List<AreaAPI>> apiResponse = new APIResponse<List<AreaAPI>>();
        try
        {
            var url = _configuration.GetSection("MiddlewareTicketControl:url").Value;
            var username = _configuration.GetSection("MiddlewareTicketControl:username").Value;
            var password = _configuration.GetSection("MiddlewareTicketControl:password").Value;
            var path = _configuration.GetSection("MiddlewareTicketControl:paths:areasearch").Value;

            url = url != null ? url.EndsWith("/") ? url.Substring(0, url.Length - 1) : url : url;
            path = path != null ? path.StartsWith("/") ? path : $"/{path}" : path;
            var fullurl = $"{url}{path}";

            var basicAuth = Convert.ToBase64String(ASCIIEncoding.ASCII.GetBytes($"{username}:{password}"));

            HeaderRequest headers = new HeaderRequest();
            headers.Add("Authorization", $"Basic {basicAuth}");
            headers.Add("User-Agent", "Alctel.CRM");

            ApiContext<List<AreaAPI>> apiContext = new ApiContext<List<AreaAPI>>();
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
}
