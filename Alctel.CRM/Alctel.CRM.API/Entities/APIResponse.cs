using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Alctel.CRM.API.Entities;

public class APIResponse<ResponseType>
{
    public bool IsSuccessStatusCode { get; set; }
    public HttpStatusCode StatusCode { get; set; }
    public string AdditionalMessage { get; set; } = string.Empty;
    public ResponseType? Response { get; set; }
}
