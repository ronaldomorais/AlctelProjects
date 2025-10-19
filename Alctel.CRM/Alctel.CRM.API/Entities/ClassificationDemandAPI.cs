using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Alctel.CRM.API.Entities;

public class ClassificationDemandAPI
{
    [JsonProperty("idClassDemanda")]
    public Int64 Id { get; set; }

    [JsonProperty("nomeClassDemanda")]
    public string? Name { get; set; }

    [JsonProperty("statusClassDemanda")]
    public bool Active { get; set; }
}
