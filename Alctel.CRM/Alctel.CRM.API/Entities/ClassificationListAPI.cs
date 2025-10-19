using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Alctel.CRM.API.Entities;

public class ClassificationListAPI
{
    [JsonProperty("idLista")]
    public Int64 Id { get; set; }

    [JsonProperty("nomeLista")]
    public string? Name { get; set; }

    [JsonProperty("statusLista")]
    public bool Active { get; set; }

}
