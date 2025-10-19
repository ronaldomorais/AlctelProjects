using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Alctel.CRM.API.Entities;

public class TransferFormAPI
{
    [JsonProperty("idFormulario")]
    public Int64 FormId { get; set; }

    [JsonProperty("titulo")]
    public string? FormTitle { get; set; }

    [JsonProperty("corpo")]
    public string? FormBody { get; set; }
}
