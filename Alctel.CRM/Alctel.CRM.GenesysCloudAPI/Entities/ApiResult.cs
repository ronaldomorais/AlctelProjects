using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Alctel.CRM.GenesysCloudAPI.Entities;

public class ApiResult
{
    public HttpStatusCode? StatusCode { get; set; }
    public string? JsonString { get; set; }
    public dynamic? JsonObject { get; set; }
}
