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

public class SlaAPIRepository : ISlaAPIRepository
{
    private readonly IConfiguration _configuration;

    public SlaAPIRepository(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task<APIResponse<int>> InsertSlaCalendarAPIAsync(SlaAlertAgendaAPI data)
    {
        APIResponse<int> apiResponse = new APIResponse<int>();
        try
        {
            var url = _configuration.GetSection("MiddlewareTicketControl:url").Value;
            var username = _configuration.GetSection("MiddlewareTicketControl:username").Value;
            var password = _configuration.GetSection("MiddlewareTicketControl:password").Value;
            var path = _configuration.GetSection("MiddlewareTicketControl:paths:slacalendarinsert").Value;

            ApiContext<int> apiContext = new ApiContext<int>();

            dynamic obj = new ExpandoObject();
            obj.idUsuario = data.UserId;
            obj.descricao = data.HolidayName;
            obj.data = data.HolidayDate.ToString("yyyy-MM-dd");

            var json = JsonConvert.SerializeObject(obj);

            if (url != null && username != null && password != null)
            {
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

    public async Task<APIResponse<List<SlaAlertAgendaAPI>>> GetSlaCalendarAPIAsync()
    {
        APIResponse<List<SlaAlertAgendaAPI>> apiResponse = new APIResponse<List<SlaAlertAgendaAPI>>();
        try
        {
            var url = _configuration.GetSection("MiddlewareTicketControl:url").Value;
            var username = _configuration.GetSection("MiddlewareTicketControl:username").Value;
            var password = _configuration.GetSection("MiddlewareTicketControl:password").Value;
            var path = _configuration.GetSection("MiddlewareTicketControl:paths:slacalendarlist").Value;

            ApiContext<List<SlaAlertAgendaAPI>> apiContext = new ApiContext<List<SlaAlertAgendaAPI>>();

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

    public async Task<APIResponse<List<SlaTicketConfigAPI>>> GetSlaTicketConfigAPIAsync()
    {
        APIResponse<List<SlaTicketConfigAPI>> apiResponse = new APIResponse<List<SlaTicketConfigAPI>>();
        try
        {
            var url = _configuration.GetSection("MiddlewareTicketControl:url").Value;
            var username = _configuration.GetSection("MiddlewareTicketControl:username").Value;
            var password = _configuration.GetSection("MiddlewareTicketControl:password").Value;
            var path = _configuration.GetSection("MiddlewareTicketControl:paths:slaticketconfiglist").Value;

            ApiContext<List<SlaTicketConfigAPI>> apiContext = new ApiContext<List<SlaTicketConfigAPI>>();

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

    public async Task<APIResponse<string>> InsertSlaTicketConfigAPIAsync(SlaTicketCreateAPI data)
    {
        APIResponse<string> apiResponse = new APIResponse<string>();
        try
        {
            var url = _configuration.GetSection("MiddlewareTicketControl:url").Value;
            var username = _configuration.GetSection("MiddlewareTicketControl:username").Value;
            var password = _configuration.GetSection("MiddlewareTicketControl:password").Value;
            var path = _configuration.GetSection("MiddlewareTicketControl:paths:slaticketconfiginsert").Value;

            ApiContext<string> apiContext = new ApiContext<string>();

            var json = JsonConvert.SerializeObject(data);

            if (url != null && username != null && password != null)
            {
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

    public async Task<APIResponse<int>> GetBusinessDaysAPI(DateTime startTime, DateTime endTime)
    {
        APIResponse<int> apiResponse = new APIResponse<int>();
        try
        {
            var url = _configuration.GetSection("MiddlewareTicketControl:url").Value;
            var username = _configuration.GetSection("MiddlewareTicketControl:username").Value;
            var password = _configuration.GetSection("MiddlewareTicketControl:password").Value;
            var path = _configuration.GetSection("MiddlewareTicketControl:paths:slaticketbusinessdays").Value;

            ApiContext<int> apiContext = new ApiContext<int>();

            dynamic obj = new ExpandoObject();
            obj.dataInicial = startTime.ToString("yyyy-MM-dd");
            obj.dataFinal = endTime.ToString("yyyy-MM-dd");

            var json = JsonConvert.SerializeObject(obj);

            if (url != null && username != null && password != null)
            {
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

    public async Task<APIResponse<List<SlaTicketConfigAPI>>> GetSlaTicketConfigAPIAsync(Int64 id)
    {
        APIResponse<List<SlaTicketConfigAPI>> apiResponse = new APIResponse<List<SlaTicketConfigAPI>>();
        try
        {
            var url = _configuration.GetSection("MiddlewareTicketControl:url").Value;
            var username = _configuration.GetSection("MiddlewareTicketControl:username").Value;
            var password = _configuration.GetSection("MiddlewareTicketControl:password").Value;
            var path = _configuration.GetSection("MiddlewareTicketControl:paths:slaticketconfigid").Value;

            ApiContext<List<SlaTicketConfigAPI>> apiContext = new ApiContext<List<SlaTicketConfigAPI>>();

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

    public async Task<APIResponse<int>> UpdateSlaTicketConfigAPIAsync(SlaTicketConfigAPI data)
    {
        APIResponse<int> apiResponse = new APIResponse<int>();
        try
        {
            var url = _configuration.GetSection("MiddlewareTicketControl:url").Value;
            var username = _configuration.GetSection("MiddlewareTicketControl:username").Value;
            var password = _configuration.GetSection("MiddlewareTicketControl:password").Value;
            var path = _configuration.GetSection("MiddlewareTicketControl:paths:stlaticketconfigupdate").Value;

            ApiContext<int> apiContext = new ApiContext<int>();

            dynamic obj = new ExpandoObject();
            obj.idChamadoSla = data.SlaTicketId;
            obj.idCriticidade = data.CriticalityId;
            obj.sla = data.Sla;
            obj.alarme = data.Alarm;

            var json = JsonConvert.SerializeObject(obj);

            if (url != null && username != null && password != null)
            {
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
