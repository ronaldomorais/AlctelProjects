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
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Alctel.CRM.API.Repositories;

public class ClassificationListAPIRepository : IClassificationListAPIRepository
{
    private readonly IConfiguration _configuration;
    //private List<ClassificationListAPI> _classificationListAPICollection;

    public ClassificationListAPIRepository(IConfiguration configuration)
    {
        _configuration = configuration;

        //_classificationListAPICollection = new List<ClassificationListAPI>();
        //_classificationListAPICollection.Add(new ClassificationListAPI
        //{
        //    Id = 1,
        //    Name = "Unidade de Atendimento",
        //    Active = true,
        //});

        //_classificationListAPICollection.Add(new ClassificationListAPI
        //{
        //    Id = 2,
        //    Name = "Unidade de Educação",
        //    Active = true,
        //});

        //_classificationListAPICollection.Add(new ClassificationListAPI
        //{
        //    Id = 3,
        //    Name = "Unidades de Hospedagem",
        //    Active = true,
        //});
    }

    public async Task<APIResponse<List<ClassificationListAPI>>> GetClassificationListListAPIAsync()
    {
        APIResponse<List<ClassificationListAPI>> apiResponse = new APIResponse<List<ClassificationListAPI>>();
        try
        {
            var url = _configuration.GetSection("MiddlewareTicketControl:url").Value;
            var username = _configuration.GetSection("MiddlewareTicketControl:username").Value;
            var password = _configuration.GetSection("MiddlewareTicketControl:password").Value;
            var path = _configuration.GetSection("MiddlewareTicketControl:paths:classificationlistlist").Value;

            ApiContext<List<ClassificationListAPI>> apiContext = new ApiContext<List<ClassificationListAPI>>();

            if (url != null && username != null && password != null)
            {
                apiResponse = await apiContext.PostBasicAuthAPIAsync(url, username, password, path);
            }

            //apiResponse.IsSuccessStatusCode = true;
            //apiResponse.StatusCode = System.Net.HttpStatusCode.OK;
            //apiResponse.Response = new List<ClassificationListAPI>();

            //apiResponse.Response.AddRange(_classificationListAPICollection);
        }
        catch (Exception ex)
        {
            apiResponse.IsSuccessStatusCode = false;
            apiResponse.AdditionalMessage = ex.Message;
            apiResponse.StatusCode = System.Net.HttpStatusCode.InternalServerError;
        }

        return apiResponse;
    }

    public async Task<APIResponse<List<ClassificationListAPI>>> GetClassificationListActivatedListAPIAsync()
    {
        APIResponse<List<ClassificationListAPI>> apiResponse = new APIResponse<List<ClassificationListAPI>>();
        try
        {
            var url = _configuration.GetSection("MiddlewareTicketControl:url").Value;
            var username = _configuration.GetSection("MiddlewareTicketControl:username").Value;
            var password = _configuration.GetSection("MiddlewareTicketControl:password").Value;
            var path = _configuration.GetSection("MiddlewareTicketControl:paths:classificationlistactivatedlist").Value;

            ApiContext<List<ClassificationListAPI>> apiContext = new ApiContext<List<ClassificationListAPI>>();

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

 

    //public async Task<APIResponse<ClassificationListAPI>> GetClassificationListAPIAsync(Int64 id)
    //{
    //    APIResponse<ClassificationListAPI> apiResponse = new APIResponse<ClassificationListAPI>();
    //    try
    //    {
    //        //var url = _configuration.GetSection("MiddlewareTicketControl:url").Value;
    //        //var username = _configuration.GetSection("MiddlewareTicketControl:username").Value;
    //        //var password = _configuration.GetSection("MiddlewareTicketControl:password").Value;
    //        //var path = _configuration.GetSection("MiddlewareTicketControl:paths:classificationlistid").Value;

    //        //ApiContext<ClassificationListAPI> apiContext = new ApiContext<ClassificationListAPI>();

    //        //if (url != null && username != null && password != null)
    //        //{
    //        //    apiResponse = await apiContext.PostBasicAuthAPIAsync(url, username, password, path, id.ToString());
    //        //}

    //        var classificationList = _classificationListAPICollection.FirstOrDefault(_ => _.Id == id);

    //        if (classificationList != null)
    //        {
    //            apiResponse.StatusCode = System.Net.HttpStatusCode.OK;
    //            apiResponse.IsSuccessStatusCode = true;
    //            apiResponse.Response = classificationList;
    //        }

    //    }
    //    catch (Exception ex)
    //    {
    //        apiResponse.IsSuccessStatusCode = false;
    //        apiResponse.AdditionalMessage = ex.Message;
    //        apiResponse.StatusCode = System.Net.HttpStatusCode.InternalServerError;
    //    }

    //    return apiResponse;
    //}


    //public async Task<APIResponse<bool>> InsertClassificationListAPIAsync(string data)
    //{
    //    APIResponse<bool> apiResponse = new APIResponse<bool>();
    //    try
    //    {
    //        //var url = _configuration.GetSection("MiddlewareTicketControl:url").Value;
    //        //var username = _configuration.GetSection("MiddlewareTicketControl:username").Value;
    //        //var password = _configuration.GetSection("MiddlewareTicketControl:password").Value;
    //        //var path = _configuration.GetSection("MiddlewareTicketControl:paths:classificationlistinsert").Value;

    //        //ApiContext<bool> apiContext = new ApiContext<bool>();

    //        //if (url != null && username != null && password != null)
    //        //{
    //        //    var json = JsonConvert.SerializeObject(data);
    //        //    apiResponse = await apiContext.PostBasicAuthAPIAsync(url, username, password, path, json);
    //        //}

    //        var id =  _classificationListAPICollection.LastOrDefault().Id;

    //        var classificationList = new ClassificationListAPI();
    //        classificationList.Id = ++id;
    //        classificationList.Name = data;
    //        classificationList.Active = true;

    //        _classificationListAPICollection.Add(classificationList);

    //    }
    //    catch (Exception ex)
    //    {
    //        apiResponse.IsSuccessStatusCode = false;
    //        apiResponse.AdditionalMessage = ex.Message;
    //        apiResponse.StatusCode = System.Net.HttpStatusCode.InternalServerError;
    //    }

    //    return apiResponse;
    //}

    //public async Task<APIResponse<bool>> UpdateClassificationListAPIAsync(Int64 id, bool status)
    //{
    //    APIResponse<bool> apiResponse = new APIResponse<bool>();
    //    try
    //    {
    //        //var url = _configuration.GetSection("MiddlewareTicketControl:url").Value;
    //        //var username = _configuration.GetSection("MiddlewareTicketControl:username").Value;
    //        //var password = _configuration.GetSection("MiddlewareTicketControl:password").Value;
    //        //var path = _configuration.GetSection("MiddlewareTicketControl:paths:classificationlistupdate").Value;

    //        //ApiContext<bool> apiContext = new ApiContext<bool>();

    //        //if (url != null && username != null && password != null)
    //        //{
    //        //    dynamic obj = new ExpandoObject();
    //        //    obj.idClassificationList = id;
    //        //    obj.statusClassificationList = status;

    //        //    var json = JsonConvert.SerializeObject(obj);
    //        //    apiResponse = await apiContext.PostBasicAuthAPIAsync(url, username, password, path, json);
    //        //}


    //        var classList = _classificationListAPICollection.FirstOrDefault(_ => _.Id == id);

    //        _classificationListAPICollection.Remove(classList);

    //        _classificationListAPICollection.Add(new ClassificationListAPI
    //        {
    //            Id = classList.Id,
    //            Name = classList.Name,
    //            Active = status
    //        });


    //    }
    //    catch (Exception ex)
    //    {
    //        apiResponse.IsSuccessStatusCode = false;
    //        apiResponse.AdditionalMessage = ex.Message;
    //        apiResponse.StatusCode = System.Net.HttpStatusCode.InternalServerError;
    //    }

    //    return apiResponse;
    //}

    //public async Task<APIResponse<List<ClassificationListAPI>>> SearchDataAsync(string data)
    //{
    //    APIResponse<List<ClassificationListAPI>> apiResponse = new APIResponse<List<ClassificationListAPI>>();
    //    try
    //    {
    //        var url = _configuration.GetSection("MiddlewareTicketControl:url").Value;
    //        var username = _configuration.GetSection("MiddlewareTicketControl:username").Value;
    //        var password = _configuration.GetSection("MiddlewareTicketControl:password").Value;
    //        var path = _configuration.GetSection("MiddlewareTicketControl:paths:classificationlistsearch").Value;

    //        url = url != null ? url.EndsWith("/") ? url.Substring(0, url.Length - 1) : url : url;
    //        path = path != null ? path.StartsWith("/") ? path : $"/{path}" : path;
    //        var fullurl = $"{url}{path}";

    //        var basicAuth = Convert.ToBase64String(ASCIIEncoding.ASCII.GetBytes($"{username}:{password}"));

    //        HeaderRequest headers = new HeaderRequest();
    //        headers.Add("Authorization", $"Basic {basicAuth}");
    //        headers.Add("User-Agent", "Alctel.CRM");

    //        ApiContext<List<ClassificationListAPI>> apiContext = new ApiContext<List<ClassificationListAPI>>();
    //        apiResponse = await apiContext.PostRequest(fullurl, "application/json", headers, data);
    //    }
    //    catch (Exception ex)
    //    {
    //        apiResponse.IsSuccessStatusCode = false;
    //        apiResponse.AdditionalMessage = ex.Message;
    //        apiResponse.StatusCode = System.Net.HttpStatusCode.InternalServerError;
    //    }

    //    return apiResponse;
    //}
}
