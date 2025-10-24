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

public class TicketClassificationAPIRepository : ITicketClassificationAPIRepository
{
    private readonly IConfiguration _configuration;

    public TicketClassificationAPIRepository(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task<APIResponse<List<TicketClassificationManifestationTypeAPI>>> GetTicketClassificationManifestationTypeAPIAsync()
    {
        APIResponse<List<TicketClassificationManifestationTypeAPI>> apiResponse = new APIResponse<List<TicketClassificationManifestationTypeAPI>>();
        try
        {
            var url = _configuration.GetSection("MiddlewareTicketControl:url").Value;
            var username = _configuration.GetSection("MiddlewareTicketControl:username").Value;
            var password = _configuration.GetSection("MiddlewareTicketControl:password").Value;
            var path = _configuration.GetSection("MiddlewareTicketControl:paths:ticketclassificationmanifestationtype").Value;

            ApiContext<List<TicketClassificationManifestationTypeAPI>> apiContext = new ApiContext<List<TicketClassificationManifestationTypeAPI>>();

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

    public async Task<APIResponse<int>> InsertTicketClassificationListAPIAsync(string data)
    {
        APIResponse<int> apiResponse = new APIResponse<int>();
        try
        {
            var url = _configuration.GetSection("MiddlewareTicketControl:url").Value;
            var username = _configuration.GetSection("MiddlewareTicketControl:username").Value;
            var password = _configuration.GetSection("MiddlewareTicketControl:password").Value;
            var path = _configuration.GetSection("MiddlewareTicketControl:paths:ticketclassificationinsertlist").Value;

            ApiContext<int> apiContext = new ApiContext<int>();

            if (url != null && username != null && password != null)
            {
                apiResponse = await apiContext.PostBasicAuthAPIAsync(url, username, password, path, JsonConvert.SerializeObject(data));
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

    public async Task<APIResponse<TicketClassificationListItemAPI>> GetTicketClassificationListItemAPIAsync(Int64 id)
    {
        APIResponse<TicketClassificationListItemAPI> apiResponse = new APIResponse<TicketClassificationListItemAPI>();
        try
        {
            var url = _configuration.GetSection("MiddlewareTicketControl:url").Value;
            var username = _configuration.GetSection("MiddlewareTicketControl:username").Value;
            var password = _configuration.GetSection("MiddlewareTicketControl:password").Value;
            var path = _configuration.GetSection("MiddlewareTicketControl:paths:ticketclassificationlistitemid").Value;

            ApiContext<TicketClassificationListItemAPI> apiContext = new ApiContext<TicketClassificationListItemAPI>();

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

    public async Task<APIResponse<int>> UpdateTicketClassificationListItemAPIAsync(Int64 id, bool status)
    {
        APIResponse<int> apiResponse = new APIResponse<int>();
        try
        {
            var url = _configuration.GetSection("MiddlewareTicketControl:url").Value;
            var username = _configuration.GetSection("MiddlewareTicketControl:username").Value;
            var password = _configuration.GetSection("MiddlewareTicketControl:password").Value;
            var path = _configuration.GetSection("MiddlewareTicketControl:paths:ticketclassificationlistitemupdate").Value;

            ApiContext<int> apiContext = new ApiContext<int>();

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

    public async Task<APIResponse<int>> InsertTicketClassificationListItemAPIAsync(Int64 listId, string data)
    {
        APIResponse<int> apiResponse = new APIResponse<int>();
        try
        {
            var url = _configuration.GetSection("MiddlewareTicketControl:url").Value;
            var username = _configuration.GetSection("MiddlewareTicketControl:username").Value;
            var password = _configuration.GetSection("MiddlewareTicketControl:password").Value;
            var path = _configuration.GetSection("MiddlewareTicketControl:paths:ticketclassificationinsertlistitem").Value;

            ApiContext<int> apiContext = new ApiContext<int>();

            dynamic obj = new ExpandoObject();
            obj.idLista = listId;
            obj.nomeLista = data;

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

    public async Task<APIResponse<int>> InsertTicketClassificationManifestationTypeAPIAsync(string data)
    {
        APIResponse<int> apiResponse = new APIResponse<int>();
        try
        {
            var url = _configuration.GetSection("MiddlewareTicketControl:url").Value;
            var username = _configuration.GetSection("MiddlewareTicketControl:username").Value;
            var password = _configuration.GetSection("MiddlewareTicketControl:password").Value;
            var path = _configuration.GetSection("MiddlewareTicketControl:paths:ticketclassificationmanifestationtypeinsert").Value;

            ApiContext<int> apiContext = new ApiContext<int>();

            if (url != null && username != null && password != null)
            {
                apiResponse = await apiContext.PostBasicAuthAPIAsync(url, username, password, path, JsonConvert.SerializeObject(data));
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

    public async Task<APIResponse<List<TicketClassificationProgramAPI>>> GetTicketClassificationProgramAPIAsync()
    {
        APIResponse<List<TicketClassificationProgramAPI>> apiResponse = new APIResponse<List<TicketClassificationProgramAPI>>();
        try
        {
            var url = _configuration.GetSection("MiddlewareTicketControl:url").Value;
            var username = _configuration.GetSection("MiddlewareTicketControl:username").Value;
            var password = _configuration.GetSection("MiddlewareTicketControl:password").Value;
            var path = _configuration.GetSection("MiddlewareTicketControl:paths:ticketclassificationprogramlist").Value;

            ApiContext<List<TicketClassificationProgramAPI>> apiContext = new ApiContext<List<TicketClassificationProgramAPI>>();

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

    public async Task<APIResponse<List<TicketClassificationReasonListAPI>>> GetTicketClassificationReasonListAPIAsync()
    {
        APIResponse<List<TicketClassificationReasonListAPI>> apiResponse = new APIResponse<List<TicketClassificationReasonListAPI>>();
        try
        {
            var url = _configuration.GetSection("MiddlewareTicketControl:url").Value;
            var username = _configuration.GetSection("MiddlewareTicketControl:username").Value;
            var password = _configuration.GetSection("MiddlewareTicketControl:password").Value;
            var path = _configuration.GetSection("MiddlewareTicketControl:paths:ticketclassificationreasonlist").Value;

            ApiContext<List<TicketClassificationReasonListAPI>> apiContext = new ApiContext<List<TicketClassificationReasonListAPI>>();

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

    public async Task<APIResponse<List<TicketClassificationReasonListAPI>>> GetTicketClassificationReasonSonListAPIAsync(Int64 id)
    {
        APIResponse<List<TicketClassificationReasonListAPI>> apiResponse = new APIResponse<List<TicketClassificationReasonListAPI>>();
        try
        {
            var url = _configuration.GetSection("MiddlewareTicketControl:url").Value;
            var username = _configuration.GetSection("MiddlewareTicketControl:username").Value;
            var password = _configuration.GetSection("MiddlewareTicketControl:password").Value;
            var path = _configuration.GetSection("MiddlewareTicketControl:paths:ticketclassificationreasonsonlist").Value;

            ApiContext<List<TicketClassificationReasonListAPI>> apiContext = new ApiContext<List<TicketClassificationReasonListAPI>>();

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

    public async Task<APIResponse<List<TicketClassificationServiceAPI>>> GetTicketClassificationServiceAPIAsync()
    {
        APIResponse<List<TicketClassificationServiceAPI>> apiResponse = new APIResponse<List<TicketClassificationServiceAPI>>();
        try
        {
            var url = _configuration.GetSection("MiddlewareTicketControl:url").Value;
            var username = _configuration.GetSection("MiddlewareTicketControl:username").Value;
            var password = _configuration.GetSection("MiddlewareTicketControl:password").Value;
            var path = _configuration.GetSection("MiddlewareTicketControl:paths:ticketclassficationservicelist").Value;

            ApiContext<List<TicketClassificationServiceAPI>> apiContext = new ApiContext<List<TicketClassificationServiceAPI>>();

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

    public async Task<APIResponse<List<TicketClassificationServiceAPI>>> GetTicketClassificationServiceByManifestationAPIAsync(Int64 id)
    {
        APIResponse<List<TicketClassificationServiceAPI>> apiResponse = new APIResponse<List<TicketClassificationServiceAPI>>();
        try
        {
            var url = _configuration.GetSection("MiddlewareTicketControl:url").Value;
            var username = _configuration.GetSection("MiddlewareTicketControl:username").Value;
            var password = _configuration.GetSection("MiddlewareTicketControl:password").Value;
            var path = _configuration.GetSection("MiddlewareTicketControl:paths:ticketclassificationservicebymanifestation").Value;

            ApiContext<List<TicketClassificationServiceAPI>> apiContext = new ApiContext<List<TicketClassificationServiceAPI>>();

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
    
    public async Task<APIResponse<List<TicketClassificationProgramAPI>>> GetTicketClassificationProgramByServiceAPIAsync(Int64 id)
    {
        APIResponse<List<TicketClassificationProgramAPI>> apiResponse = new APIResponse<List<TicketClassificationProgramAPI>>();
        try
        {
            var url = _configuration.GetSection("MiddlewareTicketControl:url").Value;
            var username = _configuration.GetSection("MiddlewareTicketControl:username").Value;
            var password = _configuration.GetSection("MiddlewareTicketControl:password").Value;
            var path = _configuration.GetSection("MiddlewareTicketControl:paths:ticketclassificationprogrambyservice").Value;

            ApiContext<List<TicketClassificationProgramAPI>> apiContext = new ApiContext<List<TicketClassificationProgramAPI>>();

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

    public async Task<APIResponse<List<TicketClassificationReasonAPI>>> GetTicketClassificationReasonByManifestationServiceAPIAsync(Int64 manifestationid, Int64 serviceId)
    {
        APIResponse<List<TicketClassificationReasonAPI>> apiResponse = new APIResponse<List<TicketClassificationReasonAPI>>();
        try
        {
            var url = _configuration.GetSection("MiddlewareTicketControl:url").Value;
            var username = _configuration.GetSection("MiddlewareTicketControl:username").Value;
            var password = _configuration.GetSection("MiddlewareTicketControl:password").Value;
            var path = _configuration.GetSection("MiddlewareTicketControl:paths:ticketclassificationreasonbymanifestationservice").Value;

            ApiContext<List<TicketClassificationReasonAPI>> apiContext = new ApiContext<List<TicketClassificationReasonAPI>>();

            dynamic obj = new ExpandoObject();
            obj.idClassificacaoTipoManifestacao = manifestationid;
            obj.idClassificacaoServico = serviceId;

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

    public async Task<APIResponse<List<TicketClassificationReasonListItemAPI>>> GetTicketClassificationReasonListItemsAPIAsync(Int64 manifestationid, Int64 serviceId, Int64? parentId)
    {
        APIResponse<List<TicketClassificationReasonListItemAPI>> apiResponse = new APIResponse<List<TicketClassificationReasonListItemAPI>>();
        try
        {
            var url = _configuration.GetSection("MiddlewareTicketControl:url").Value;
            var username = _configuration.GetSection("MiddlewareTicketControl:username").Value;
            var password = _configuration.GetSection("MiddlewareTicketControl:password").Value;
            var path = _configuration.GetSection("MiddlewareTicketControl:paths:ticketclassificationreasonlistitems").Value;

            ApiContext<List<TicketClassificationReasonListItemAPI>> apiContext = new ApiContext<List<TicketClassificationReasonListItemAPI>>();

            dynamic obj = new ExpandoObject();
            obj.idClassificacaoTipoManifestacao = manifestationid;
            obj.idClassificacaoServico = serviceId;
            obj.idMotivoPai = parentId;

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

    public async Task<APIResponse<string>> InsertTicketClassificationReasonAPIAsync(TicketClassificationReasonCreateAPI data)
    {
        APIResponse<string> apiResponse = new APIResponse<string>();
        try
        {
            var url = _configuration.GetSection("MiddlewareTicketControl:url").Value;
            var username = _configuration.GetSection("MiddlewareTicketControl:username").Value;
            var password = _configuration.GetSection("MiddlewareTicketControl:password").Value;
            var path = _configuration.GetSection("MiddlewareTicketControl:paths:ticketclassificationreasoninsert").Value;

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

    public async Task<APIResponse<int>> InsertTicketClassificationAPIAsync(TicketClassificationAPI data)
    {
        APIResponse<int> apiResponse = new APIResponse<int>();
        try
        {
            var url = _configuration.GetSection("MiddlewareTicketControl:url").Value;
            var username = _configuration.GetSection("MiddlewareTicketControl:username").Value;
            var password = _configuration.GetSection("MiddlewareTicketControl:password").Value;
            var path = _configuration.GetSection("MiddlewareTicketControl:paths:ticketclassificationinsert").Value;

            ApiContext<int> apiContext = new ApiContext<int>();

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
}
