using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Alctel.CRM.API.Entities;

public class TicketClassificationUpdateAPI
{
    [JsonProperty("idClassificacaoServico")]
    public Int64 ServiceId { get; set; }

    [JsonProperty("status")]
    public bool Active { get; set; }
}
