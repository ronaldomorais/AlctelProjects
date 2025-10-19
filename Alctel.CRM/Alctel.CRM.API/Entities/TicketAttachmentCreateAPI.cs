using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Alctel.CRM.API.Entities;

public class TicketAttachmentCreateAPI
{
    [JsonProperty("idChamado")]
    public Int64 TicketId { get; set; }

    [JsonProperty("nomeComExtensao")]
    public string? Filename { get; set; }

    //public MultipartFormDataContent? Files { get; set; }
    [JsonProperty("arquivo")]
    public string? FileBase64 { get; set; }
}
