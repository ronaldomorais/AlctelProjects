using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Alctel.CRM.GenesysCloudAPI.Entities;
using Alctel.CRM.GenesysCloudAPI.Interfaces;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace Alctel.CRM.GenesysCloudAPI.Repositories;

public class APIExplorerConsumer(IConfiguration configuration) : APIExplorerBase, IAPIExplorerConsumer
{
    protected readonly IConfiguration _configuration = configuration;
    private HttpClient? _httpClient;

    public sealed override async Task<string> GetTokenAsync()
    {
        string token = string.Empty;

        try
        {
            var urlRegion = _configuration.GetSection("GenesysCloud:UrlRegion").Value;
            var clientId = _configuration.GetSection("GenesysCloud:Token:ClientId").Value;
            var clientSecret = _configuration.GetSection("GenesysCloud:Token:ClientSecret").Value;
            //var urlToken = _configuration.GetSection("GenesysCloud:Token:Url").Value;
            var urlToken = $"https://login.{urlRegion}/oauth/token";

            var encodedData = System.Convert.ToBase64String(System.Text.Encoding.GetEncoding("ISO-8859-1").GetBytes(clientId + ":" + clientSecret));
            var authorizationHeaderString = "Basic " + encodedData;

            Dictionary<string, string> headers = new Dictionary<string, string>();
            headers.Add("Authorization", authorizationHeaderString);

            if (urlToken != null)
            {
                ApiResult apiResponse = await PostApiAsync(urlToken, "application/x-www-form-urlencoded", headers, "grant_type=client_credentials");

                if (apiResponse.StatusCode == HttpStatusCode.OK)
                {
                    token = apiResponse.JsonObject!.access_token;
                }
            }
        }
        catch (Exception ex)
        {
        }

        return token;
    }


    public sealed override async Task<ApiResult> GetApiAsync(string url, string contentType, Dictionary<string, string>? headers = null, string? postdata = null)
    {
        ApiResult apiResponse = new ApiResult();
        apiResponse.StatusCode = HttpStatusCode.InternalServerError;

        string result = string.Empty;

        try
        {
            if (_httpClient == null)
            {
                _httpClient = new HttpClient();
            }

            if (_httpClient.DefaultRequestHeaders.Count() > 0)
                _httpClient.DefaultRequestHeaders.Clear();

            if (headers != null)
            {
                foreach (var header in headers)
                {
                    _httpClient.DefaultRequestHeaders.Add(header.Key, header.Value);
                }
            }

            var response = await _httpClient.GetAsync(url);
            apiResponse.StatusCode = response.StatusCode;

            if (response.StatusCode == HttpStatusCode.OK)
            {
                result = await response.Content.ReadAsStringAsync();

                apiResponse.JsonString = result;
                apiResponse.JsonObject = JsonConvert.DeserializeObject<dynamic>(result);
            }
            else
            {
            }
        }
        catch (Exception ex)
        {
        }

        return apiResponse;
    }

    public sealed override async Task<ApiResult> PostApiAsync(string url, string contentType, Dictionary<string, string>? headers = null, string? postdata = null)
    {
        ApiResult apiResponse = new ApiResult();
        apiResponse.StatusCode = HttpStatusCode.InternalServerError;

        string result = string.Empty;

        try
        {
            if (_httpClient == null)
            {
                _httpClient = new HttpClient();
            }

            if (_httpClient.DefaultRequestHeaders.Count() > 0)
                _httpClient.DefaultRequestHeaders.Clear();

            if (headers != null)
            {
                foreach (var header in headers)
                {
                    _httpClient.DefaultRequestHeaders.Add(header.Key, header.Value);
                }
            }

            if (postdata != null)
            {
                StringContent httpContent = new StringContent(postdata, System.Text.Encoding.UTF8, contentType);
                var response = await _httpClient.PostAsync(url, httpContent);
                apiResponse.StatusCode = response.StatusCode;

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    result = await response.Content.ReadAsStringAsync();

                    apiResponse.JsonString = result;
                    apiResponse.JsonObject = JsonConvert.DeserializeObject<dynamic>(result);
                }
                else
                {
                }
            }
            else
            {
            }
        }
        catch (Exception ex)
        {
        }

        return apiResponse;
    }
}
