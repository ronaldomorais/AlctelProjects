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

public class CustomerAPIRepository : ICustomerAPIRepository
{
    private readonly IConfiguration _configuration;
    public CustomerAPIRepository(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task<APIResponse<List<CustomerAPI>>> GetAllDataAsync()
    {
        APIResponse<List<CustomerAPI>> apiResponse = new APIResponse<List<CustomerAPI>>();
        try
        {
            var url = _configuration.GetSection("MiddlewareTicketControl:url").Value;
            var username = _configuration.GetSection("MiddlewareTicketControl:username").Value;
            var password = _configuration.GetSection("MiddlewareTicketControl:password").Value;
            var path = _configuration.GetSection("MiddlewareTicketControl:paths:customerlist").Value;

            url = url != null ? url.EndsWith("/") ? url.Substring(0, url.Length - 1) : url : url;
            path = path != null ? path.StartsWith("/") ? path : $"/{path}" : path;
            var fullurl = $"{url}{path}";

            var basicAuth = Convert.ToBase64String(ASCIIEncoding.ASCII.GetBytes($"{username}:{password}"));

            HeaderRequest headers = new HeaderRequest();
            headers.Add("Authorization", $"Basic {basicAuth}");
            headers.Add("User-Agent", "Alctel.CRM");

            ApiContext<List<CustomerAPI>> apiContext = new ApiContext<List<CustomerAPI>>();
            apiResponse = await apiContext.PostRequest(fullurl, "application/json", headers);
        }
        catch (Exception ex)
        {
            apiResponse.IsSuccessStatusCode = false;
            apiResponse.AdditionalMessage = ex.Message;
            apiResponse.StatusCode = System.Net.HttpStatusCode.InternalServerError;
        }

        return apiResponse;
    }

    public async Task<APIResponse<List<CustomerAPI>>> GetPagenatedDataAsync(int page = 1, int sizepage = 50)
    {
        APIResponse<List<CustomerAPI>> apiResponse = new APIResponse<List<CustomerAPI>>();
        try
        {
            var url = _configuration.GetSection("MiddlewareTicketControl:url").Value;
            var username = _configuration.GetSection("MiddlewareTicketControl:username").Value;
            var password = _configuration.GetSection("MiddlewareTicketControl:password").Value;
            var path = _configuration.GetSection("MiddlewareTicketControl:paths:customerlistpaginated").Value;

            url = url != null ? url.EndsWith("/") ? url.Substring(0, url.Length - 1) : url : url;
            path = path != null ? path.StartsWith("/") ? path : $"/{path}" : path;
            var fullurl = $"{url}{path}";

            var basicAuth = Convert.ToBase64String(ASCIIEncoding.ASCII.GetBytes($"{username}:{password}"));

            HeaderRequest headers = new HeaderRequest();
            headers.Add("Authorization", $"Basic {basicAuth}");
            headers.Add("User-Agent", "Alctel.CRM");

            dynamic obj = new ExpandoObject();
            obj.pagina = page;
            obj.quantidade = sizepage;

            var json = JsonConvert.SerializeObject(obj);

            ApiContext<List<CustomerAPI>> apiContext = new ApiContext<List<CustomerAPI>>();
            apiResponse = await apiContext.PostRequest(fullurl, "application/json", headers, json);
        }
        catch (Exception ex)
        {
            apiResponse.IsSuccessStatusCode = false;
            apiResponse.AdditionalMessage = ex.Message;
            apiResponse.StatusCode = System.Net.HttpStatusCode.InternalServerError;
        }

        return apiResponse;
    }

    public async Task<APIResponse<int>> GetDataAsync()
    {
        APIResponse<int> apiResponse = new APIResponse<int>();
        try
        {
            var url = _configuration.GetSection("MiddlewareTicketControl:url").Value;
            var username = _configuration.GetSection("MiddlewareTicketControl:username").Value;
            var password = _configuration.GetSection("MiddlewareTicketControl:password").Value;
            var path = _configuration.GetSection("MiddlewareTicketControl:paths:customeritemscount").Value;

            url = url != null ? url.EndsWith("/") ? url.Substring(0, url.Length - 1) : url : url;
            path = path != null ? path.StartsWith("/") ? path : $"/{path}" : path;
            var fullurl = $"{url}{path}";

            var basicAuth = Convert.ToBase64String(ASCIIEncoding.ASCII.GetBytes($"{username}:{password}"));

            HeaderRequest headers = new HeaderRequest();
            headers.Add("Authorization", $"Basic {basicAuth}");
            headers.Add("User-Agent", "Alctel.CRM");

            ApiContext<int> apiContext = new ApiContext<int>();
            apiResponse = await apiContext.PostRequest(fullurl, "application/json", headers);
        }
        catch (Exception ex)
        {
            apiResponse.IsSuccessStatusCode = false;
            apiResponse.AdditionalMessage = ex.Message;
            apiResponse.StatusCode = System.Net.HttpStatusCode.InternalServerError;
        }

        return apiResponse;
    }

    public async Task<APIResponse<CustomerAPI>> GetDataAsync(Int64 id)
    {
        APIResponse<CustomerAPI> apiResponse = new APIResponse<CustomerAPI>();
        try
        {
            var url = _configuration.GetSection("MiddlewareTicketControl:url").Value;
            var username = _configuration.GetSection("MiddlewareTicketControl:username").Value;
            var password = _configuration.GetSection("MiddlewareTicketControl:password").Value;
            var path = _configuration.GetSection("MiddlewareTicketControl:paths:customerid").Value;

            url = url != null ? url.EndsWith("/") ? url.Substring(0, url.Length - 1) : url : url;
            path = path != null ? path.StartsWith("/") ? path : $"/{path}" : path;
            var fullurl = $"{url}{path}";

            var basicAuth = Convert.ToBase64String(ASCIIEncoding.ASCII.GetBytes($"{username}:{password}"));

            HeaderRequest headers = new HeaderRequest();
            headers.Add("Authorization", $"Basic {basicAuth}");
            headers.Add("User-Agent", "Alctel.CRM");

            ApiContext<CustomerAPI> apiContext = new ApiContext<CustomerAPI>();
            apiResponse = await apiContext.PostRequest(fullurl, "application/json", headers, id.ToString());
        }
        catch (Exception ex)
        {
            apiResponse.IsSuccessStatusCode = false;
            apiResponse.AdditionalMessage = ex.Message;
            apiResponse.StatusCode = System.Net.HttpStatusCode.InternalServerError;
        }

        return apiResponse;
    }

    public async Task<APIResponse<CustomerAPI>> GetDataAsync(string data, string endpoint)
    {
        APIResponse<CustomerAPI> apiResponse = new APIResponse<CustomerAPI>();
        try
        {
            var url = _configuration.GetSection("MiddlewareTicketControl:url").Value;
            var username = _configuration.GetSection("MiddlewareTicketControl:username").Value;
            var password = _configuration.GetSection("MiddlewareTicketControl:password").Value;
            var path = _configuration.GetSection($"MiddlewareTicketControl:paths:{endpoint}").Value;

            url = url != null ? url.EndsWith("/") ? url.Substring(0, url.Length - 1) : url : url;
            path = path != null ? path.StartsWith("/") ? path : $"/{path}" : path;
            var fullurl = $"{url}{path}";

            var basicAuth = Convert.ToBase64String(ASCIIEncoding.ASCII.GetBytes($"{username}:{password}"));

            HeaderRequest headers = new HeaderRequest();
            headers.Add("Authorization", $"Basic {basicAuth}");
            headers.Add("User-Agent", "Alctel.CRM");

            ApiContext<CustomerAPI> apiContext = new ApiContext<CustomerAPI>();
            apiResponse = await apiContext.PostRequest(fullurl, "application/json", headers, JsonConvert.SerializeObject(data));
        }
        catch (Exception ex)
        {
            apiResponse.IsSuccessStatusCode = false;
            apiResponse.AdditionalMessage = ex.Message;
            apiResponse.StatusCode = System.Net.HttpStatusCode.InternalServerError;
        }

        return apiResponse;
    }

    public async Task<APIResponse<bool>> UpdateDataAsync(string data)
    {
        APIResponse<bool> apiResponse = new APIResponse<bool>();
        try
        {
            var url = _configuration.GetSection("MiddlewareTicketControl:url").Value;
            var username = _configuration.GetSection("MiddlewareTicketControl:username").Value;
            var password = _configuration.GetSection("MiddlewareTicketControl:password").Value;
            var path = _configuration.GetSection("MiddlewareTicketControl:paths:customerupdate").Value;

            url = url != null ? url.EndsWith("/") ? url.Substring(0, url.Length - 1) : url : url;
            path = path != null ? path.StartsWith("/") ? path : $"/{path}" : path;
            var fullurl = $"{url}{path}";

            var basicAuth = Convert.ToBase64String(ASCIIEncoding.ASCII.GetBytes($"{username}:{password}"));

            HeaderRequest headers = new HeaderRequest();
            headers.Add("Authorization", $"Basic {basicAuth}");
            headers.Add("User-Agent", "Alctel.CRM");

            ApiContext<bool> apiContext = new ApiContext<bool>();
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

    public async Task<APIResponse<string>> InsertCustomerAPIAsync(string data)
    {
        APIResponse<string> apiResponse = new APIResponse<string>();
        try
        {
            var url = _configuration.GetSection("MiddlewareTicketControl:url").Value;
            var username = _configuration.GetSection("MiddlewareTicketControl:username").Value;
            var password = _configuration.GetSection("MiddlewareTicketControl:password").Value;
            var path = _configuration.GetSection("MiddlewareTicketControl:paths:customerinsert").Value;

            ApiContext<string> apiContext = new ApiContext<string>();

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

    public async Task<APIResponse<List<CustomerAPI>>> SearchDataAsync(string data)
    {
        APIResponse<List<CustomerAPI>> apiResponse = new APIResponse<List<CustomerAPI>>();
        try
        {
            var url = _configuration.GetSection("MiddlewareTicketControl:url").Value;
            var username = _configuration.GetSection("MiddlewareTicketControl:username").Value;
            var password = _configuration.GetSection("MiddlewareTicketControl:password").Value;
            var path = _configuration.GetSection("MiddlewareTicketControl:paths:customersearch").Value;

            url = url != null ? url.EndsWith("/") ? url.Substring(0, url.Length - 1) : url : url;
            path = path != null ? path.StartsWith("/") ? path : $"/{path}" : path;
            var fullurl = $"{url}{path}";

            var basicAuth = Convert.ToBase64String(ASCIIEncoding.ASCII.GetBytes($"{username}:{password}"));

            HeaderRequest headers = new HeaderRequest();
            headers.Add("Authorization", $"Basic {basicAuth}");
            headers.Add("User-Agent", "Alctel.CRM");

            ApiContext<List<CustomerAPI>> apiContext = new ApiContext<List<CustomerAPI>>();
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
