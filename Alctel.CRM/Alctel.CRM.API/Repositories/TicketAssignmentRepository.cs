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

namespace Alctel.CRM.API.Repositories;

public class TicketAssignmentRepository : ITicketAssignmentRepository
{
    private readonly IConfiguration _configuration;

    public TicketAssignmentRepository(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task<APIResponse<int>> InsertTicketAssignmentUserAPIAsync(string data)
    {
        APIResponse<int> apiResponse = new APIResponse<int>();
        try
        {
            var url = _configuration.GetSection("MiddlewareTicketControl:url").Value;
            var username = _configuration.GetSection("MiddlewareTicketControl:username").Value;
            var password = _configuration.GetSection("MiddlewareTicketControl:password").Value;
            var path = _configuration.GetSection("MiddlewareTicketControl:paths:ticketassignmentuser").Value;

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

    public async Task<APIResponse<int>> InsertTicketAssignmentQueueUserAPIAsync(string data)
    {
        APIResponse<int> apiResponse = new APIResponse<int>();
        try
        {
            var url = _configuration.GetSection("MiddlewareTicketControl:url").Value;
            var username = _configuration.GetSection("MiddlewareTicketControl:username").Value;
            var password = _configuration.GetSection("MiddlewareTicketControl:password").Value;
            var path = _configuration.GetSection("MiddlewareTicketControl:paths:ticketassignmentqueueuser").Value;

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
}
