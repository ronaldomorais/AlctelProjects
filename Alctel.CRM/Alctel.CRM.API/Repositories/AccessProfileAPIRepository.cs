using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alctel.CRM.API.Context;
using Alctel.CRM.API.Entities;
using Alctel.CRM.API.Interfaces;
using Microsoft.Extensions.Configuration;

namespace Alctel.CRM.API.Repositories;

public class AccessProfileAPIRepository : IAccessProfileAPIRepository
{
    private readonly IConfiguration _configuration;
    public AccessProfileAPIRepository(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task<APIResponse<List<AccessProfileAPI>>> GetAllDataAsync()
    {
        APIResponse<List<AccessProfileAPI>> apiResponse = new APIResponse<List<AccessProfileAPI>>();
        try
        {
            var url = _configuration.GetSection("MiddlewareTicketControl:url").Value;
            var username = _configuration.GetSection("MiddlewareTicketControl:username").Value;
            var password = _configuration.GetSection("MiddlewareTicketControl:password").Value;
            var path = _configuration.GetSection("MiddlewareTicketControl:paths:accessprofilelist").Value;

            url = url != null ? url.EndsWith("/") ? url.Substring(0, url.Length - 1) : url : url;
            path = path != null ? path.StartsWith("/") ? path : $"/{path}" : path;
            var fullurl = $"{url}{path}";

            var basicAuth = Convert.ToBase64String(ASCIIEncoding.ASCII.GetBytes($"{username}:{password}"));

            HeaderRequest headers = new HeaderRequest();
            headers.Add("Authorization", $"Basic {basicAuth}");
            headers.Add("User-Agent", "Alctel.CRM");

            ApiContext<List<AccessProfileAPI>> apiContext = new ApiContext<List<AccessProfileAPI>>();
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

    public async Task<APIResponse<List<AccessProfileAPI>>> GetAccessProfileActivatedListAPIAsync()
    {
        APIResponse<List<AccessProfileAPI>> apiResponse = new APIResponse<List<AccessProfileAPI>>();
        try
        {
            var url = _configuration.GetSection("MiddlewareTicketControl:url").Value;
            var username = _configuration.GetSection("MiddlewareTicketControl:username").Value;
            var password = _configuration.GetSection("MiddlewareTicketControl:password").Value;
            var path = _configuration.GetSection("MiddlewareTicketControl:paths:accessprofileactivatedlist").Value;

            ApiContext<List<AccessProfileAPI>> apiContext = new ApiContext<List<AccessProfileAPI>>();

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
}
