using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Alctel.CRM.API.Entities;

public class TicketAttachmentAPI
{
    [JsonProperty("nome")]
    public string? Filename { get; set; }

    [JsonProperty("caminho")]
    public string? FilePath { get; set; }
}
