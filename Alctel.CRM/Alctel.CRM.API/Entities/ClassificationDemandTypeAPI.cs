using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Alctel.CRM.API.Entities;

public class ClassificationDemandTypeAPI
{
    [JsonProperty("idClassTipo")]
    public Int64 Id { get; set; }

    [JsonProperty("idClassDemanda")]
    public Int64 DemandId { get; set; }

    [JsonProperty("nomeClassTipo")]
    public string? Name { get; set; }

    [JsonProperty("statusClassTipo")]
    public bool Active { get; set; }
}
