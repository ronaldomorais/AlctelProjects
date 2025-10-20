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

public class TicketClassificationAPIRepository : ITicketClassificationAPIRepository
{
    private readonly IConfiguration _configuration;

    public TicketClassificationAPIRepository(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task<APIResponse<List<TicketClassificationManifestationType>>> GetTicketClassificationManifestationTypeAPIAsync()
    {
        APIResponse<List<TicketClassificationManifestationType>> apiResponse = new APIResponse<List<TicketClassificationManifestationType>>();
        try
        {
            var url = _configuration.GetSection("MiddlewareTicketControl:url").Value;
            var username = _configuration.GetSection("MiddlewareTicketControl:username").Value;
            var password = _configuration.GetSection("MiddlewareTicketControl:password").Value;
            var path = _configuration.GetSection("MiddlewareTicketControl:paths:ticketclassificationmanifestationtype").Value;

            ApiContext<List<TicketClassificationManifestationType>> apiContext = new ApiContext<List<TicketClassificationManifestationType>>();

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

    public async Task<APIResponse<List<TicketClassificationListAPI>>> GetTicketClassificationListAPIAsync()
    {
        APIResponse<List<TicketClassificationListAPI>> apiResponse = new APIResponse<List<TicketClassificationListAPI>>();
        try
        {
            var url = _configuration.GetSection("MiddlewareTicketControl:url").Value;
            var username = _configuration.GetSection("MiddlewareTicketControl:username").Value;
            var password = _configuration.GetSection("MiddlewareTicketControl:password").Value;
            var path = _configuration.GetSection("MiddlewareTicketControl:paths:ticketclassificationlist").Value;

            ApiContext<List<TicketClassificationListAPI>> apiContext = new ApiContext<List<TicketClassificationListAPI>>();

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

    public async Task<APIResponse<List<TicketClassificationListItemsAPI>>> GetTicketClassificationListItemsAPIAsync(Int64 id)
    {
        APIResponse<List<TicketClassificationListItemsAPI>> apiResponse = new APIResponse<List<TicketClassificationListItemsAPI>>();
        try
        {
            var url = _configuration.GetSection("MiddlewareTicketControl:url").Value;
            var username = _configuration.GetSection("MiddlewareTicketControl:username").Value;
            var password = _configuration.GetSection("MiddlewareTicketControl:password").Value;
            var path = _configuration.GetSection("MiddlewareTicketControl:paths:ticketclassificationlistitem").Value;

            ApiContext<List<TicketClassificationListItemsAPI>> apiContext = new ApiContext<List<TicketClassificationListItemsAPI>>();

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
}
