using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alctel.CRM.API.Entities;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace Alctel.CRM.API.Context;

public class ApiContext<ResponseType>
{
    public async Task<APIResponse<ResponseType>> PostRequest(string url, string contentType, HeaderRequest? headers = null, string? postdata = null)
    {
        APIResponse<ResponseType> apiResponse = new APIResponse<ResponseType>();
        try
        {
            HttpClient httpClient = new HttpClient();
            StringContent? httpContent = null;

            if (headers != null)
            {
                foreach (var header in headers)
                {
                    httpClient.DefaultRequestHeaders.Add(header.Key, header.Value);
                }
            }

            if (postdata != null)
            {
                httpContent = new StringContent(postdata, System.Text.Encoding.UTF8, contentType);
            }

            var response = await httpClient.PostAsync(url, httpContent);

            if (response != null)
            {
                apiResponse.IsSuccessStatusCode = response.IsSuccessStatusCode;
                apiResponse.StatusCode = response.StatusCode;

                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsStringAsync();
                    apiResponse.Response = JsonConvert.DeserializeObject<ResponseType>(result);
                }
                else
                {
                    var result = await response.Content.ReadAsStringAsync();
                    var generic_result = JsonConvert.DeserializeObject<dynamic>(result);

                    if (generic_result != null)
                    {
                        apiResponse.IsSuccessStatusCode = false;
                        apiResponse.AdditionalMessage = generic_result.message;
                    }
                }
            }
            else
            {
                apiResponse.StatusCode = System.Net.HttpStatusCode.NoContent;
                apiResponse.IsSuccessStatusCode = false;
                apiResponse.AdditionalMessage = "Resposta nula";
            }
        }
        catch (Exception ex)
        {
            apiResponse.StatusCode = System.Net.HttpStatusCode.InternalServerError;
            apiResponse.AdditionalMessage = ex.Message;
            apiResponse.IsSuccessStatusCode = false;
        }

        return apiResponse;
    }

    public async Task<APIResponse<ResponseType>> PostBasicAuthAPIAsync(string url, string username, string password, string? path = null, string? data = null)
    {
        APIResponse<ResponseType> apiResponse = new APIResponse<ResponseType>();
        try
        {
            string fullurl = string.Empty;
            url = url.EndsWith("/") ? url.Substring(0, url.Length - 1) : url;
            path = path != null ? path.StartsWith("/") ? path : $"/{path}" : path;


            if (path != null)
                fullurl = $"{url}{path}";
            else
                fullurl = url;

            var basicAuth = Convert.ToBase64String(ASCIIEncoding.ASCII.GetBytes($"{username}:{password}"));

            HeaderRequest headers = new HeaderRequest();
            headers.Add("Authorization", $"Basic {basicAuth}");
            headers.Add("User-Agent", "Alctel.CRM");

            ApiContext<ResponseType> apiContext = new ApiContext<ResponseType>();
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
