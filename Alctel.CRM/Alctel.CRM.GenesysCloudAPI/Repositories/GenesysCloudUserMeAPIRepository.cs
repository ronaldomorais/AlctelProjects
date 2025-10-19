using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alctel.CRM.GenesysCloudAPI.Entities;
using Alctel.CRM.GenesysCloudAPI.Interfaces;
using Microsoft.Extensions.Configuration;

namespace Alctel.CRM.GenesysCloudAPI.Repositories;

public class GenesysCloudUserMeAPIRepository : APIExplorerConsumer, IGenesysCloudUserMeAPIRepository
{
    public GenesysCloudUserMeAPIRepository(IConfiguration configuration) : base(configuration)
    {
    }

    public async Task<ApiResult> Request()
    {
        ApiResult apiResponse = new ApiResult();
        apiResponse.StatusCode = System.Net.HttpStatusCode.InternalServerError;

        var urlRegion = _configuration.GetSection("GenesysCloud:UrlRegion").Value;
        var endpoint = "api/v2/users/me";

        var url = $"https://api.{urlRegion}/{endpoint}";

        string token = await GetTokenAsync();

        if (token != string.Empty)
        {
            Dictionary<string, string> headers = new Dictionary<string, string>();
            headers.Add("Authorization", $"Bearer {token}");
            apiResponse = await GetApiAsync(url, "application/json", headers);
        }

        return apiResponse;
    }

}
