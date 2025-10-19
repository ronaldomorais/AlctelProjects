using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Newtonsoft.Json;
using JsonIgnoreAttribute = Newtonsoft.Json.JsonIgnoreAttribute;

namespace Alctel.CRM.API.Entities;

public class AccessProfileAPI
{
    //[JsonPropertyName("idPerfil")]
    [JsonProperty("idPerfil")]
    public Int64 Id { get; set; }

    //[JsonPropertyName("nomePerfil")]
    [JsonProperty("nomePerfil")]
    public string? Name { get; set; }

    [JsonIgnore]
    public string? Description { get; set; }
}
