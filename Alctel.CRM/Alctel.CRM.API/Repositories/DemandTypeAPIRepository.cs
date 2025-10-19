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

public class DemandTypeAPIRepository : IDemandTypeAPIRepository
{
    private readonly IConfiguration _configuration;
    public DemandTypeAPIRepository(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task<APIResponse<List<DemandTypeAPI>>> GetDemandTypeListAPIAsync()
    {
        APIResponse<List<DemandTypeAPI>> apiResponse = new APIResponse<List<DemandTypeAPI>>();
        try
        {
            var url = _configuration.GetSection("MiddlewareTicketControl:url").Value;
            var username = _configuration.GetSection("MiddlewareTicketControl:username").Value;
            var password = _configuration.GetSection("MiddlewareTicketControl:password").Value;
            var path = _configuration.GetSection("MiddlewareTicketControl:paths:demandtypelist").Value;

            ApiContext<List<DemandTypeAPI>> apiContext = new ApiContext<List<DemandTypeAPI>>();

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

    public async Task<APIResponse<List<DemandTypeAPI>>> GetDemandTypeActivatedListAPIAsync()
    {
        APIResponse<List<DemandTypeAPI>> apiResponse = new APIResponse<List<DemandTypeAPI>>();
        try
        {
            var url = _configuration.GetSection("MiddlewareTicketControl:url").Value;
            var username = _configuration.GetSection("MiddlewareTicketControl:username").Value;
            var password = _configuration.GetSection("MiddlewareTicketControl:password").Value;
            var path = _configuration.GetSection("MiddlewareTicketControl:paths:demandtypeactivatedlist").Value;

            ApiContext<List<DemandTypeAPI>> apiContext = new ApiContext<List<DemandTypeAPI>>();

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

    public async Task<APIResponse<DemandTypeAPI>> GetDemandTypeAPIAsync(Int64 id)
    {
        APIResponse<DemandTypeAPI> apiResponse = new APIResponse<DemandTypeAPI>();
        try
        {
            var url = _configuration.GetSection("MiddlewareTicketControl:url").Value;
            var username = _configuration.GetSection("MiddlewareTicketControl:username").Value;
            var password = _configuration.GetSection("MiddlewareTicketControl:password").Value;
            var path = _configuration.GetSection("MiddlewareTicketControl:paths:demandtypeid").Value;

            ApiContext<DemandTypeAPI> apiContext = new ApiContext<DemandTypeAPI>();

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


    public async Task<APIResponse<bool>> InsertDemandTypeAPIAsync(string data)
    {
        APIResponse<bool> apiResponse = new APIResponse<bool>();
        try
        {
            var url = _configuration.GetSection("MiddlewareTicketControl:url").Value;
            var username = _configuration.GetSection("MiddlewareTicketControl:username").Value;
            var password = _configuration.GetSection("MiddlewareTicketControl:password").Value;
            var path = _configuration.GetSection("MiddlewareTicketControl:paths:demandtypeinsert").Value;

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

    public async Task<APIResponse<bool>> UpdateDemandTypeAPIAsync(Int64 id, bool status)
    {
        APIResponse<bool> apiResponse = new APIResponse<bool>();
        try
        {
            var url = _configuration.GetSection("MiddlewareTicketControl:url").Value;
            var username = _configuration.GetSection("MiddlewareTicketControl:username").Value;
            var password = _configuration.GetSection("MiddlewareTicketControl:password").Value;
            var path = _configuration.GetSection("MiddlewareTicketControl:paths:demandtypeupdate").Value;

            ApiContext<bool> apiContext = new ApiContext<bool>();

            if (url != null && username != null && password != null)
            {
                dynamic obj = new ExpandoObject();
                obj.idMotivoDemanda = id;
                obj.statusMotivoDemanda = status;

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

    public async Task<APIResponse<List<DemandTypeAPI>>> SearchDataAsync(string data)
    {
        APIResponse<List<DemandTypeAPI>> apiResponse = new APIResponse<List<DemandTypeAPI>>();
        try
        {
            var url = _configuration.GetSection("MiddlewareTicketControl:url").Value;
            var username = _configuration.GetSection("MiddlewareTicketControl:username").Value;
            var password = _configuration.GetSection("MiddlewareTicketControl:password").Value;
            var path = _configuration.GetSection("MiddlewareTicketControl:paths:demandtypesearch").Value;

            url = url != null ? url.EndsWith("/") ? url.Substring(0, url.Length - 1) : url : url;
            path = path != null ? path.StartsWith("/") ? path : $"/{path}" : path;
            var fullurl = $"{url}{path}";

            var basicAuth = Convert.ToBase64String(ASCIIEncoding.ASCII.GetBytes($"{username}:{password}"));

            HeaderRequest headers = new HeaderRequest();
            headers.Add("Authorization", $"Basic {basicAuth}");
            headers.Add("User-Agent", "Alctel.CRM");

            ApiContext<List<DemandTypeAPI>> apiContext = new ApiContext<List<DemandTypeAPI>>();
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
