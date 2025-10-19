using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alctel.CRM.API.Context;
using Alctel.CRM.API.Entities;
using Alctel.CRM.API.Interfaces;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Alctel.CRM.API.Repositories;

public class LoginAPIRepository : ILoginAPIRepository
{
    private readonly IConfiguration _configuration;
    public LoginAPIRepository(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task<APIResponse<LoginAPI>> GetLoginPIAsync(string login)
    {
        APIResponse<LoginAPI> apiResponse = new APIResponse<LoginAPI>();
        try
        {
            var url = _configuration.GetSection("MiddlewareTicketControl:url").Value;
            var username = _configuration.GetSection("MiddlewareTicketControl:username").Value;
            var password = _configuration.GetSection("MiddlewareTicketControl:password").Value;
            var path = _configuration.GetSection("MiddlewareTicketControl:paths:loginid").Value;

            ApiContext<LoginAPI> apiContext = new ApiContext<LoginAPI>();

            if (url != null && username != null && password != null)
            {
                var json = JsonConvert.SerializeObject(login);
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
