using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alctel.CRM.GenesysCloudAPI.Entities;

namespace Alctel.CRM.GenesysCloudAPI.Interfaces;

public abstract class APIExplorerBase
{
    public abstract Task<ApiResult> GetApiAsync(string url, string contentType, Dictionary<string, string>? headers = null, string? postdata = null);
    public abstract Task<ApiResult> PostApiAsync(string url, string contentType, Dictionary<string, string>? headers = null, string? postdata = null);
    public abstract Task<string> GetTokenAsync();
}
