using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Alctel.CRM.API.Entities;

public class TicketClassificationCreateAPI
{
    [JsonProperty("idChamado")]
    public Int64 TicketId { get; set; }

    [JsonProperty("idClassificacaoMotivo")]
    public Int64 ClassificationReasonId { get; set; }

    [JsonProperty("idItemLista")]
    public Int64 ClassificationReasonListId { get; set; }

    [JsonProperty("idUsuario")]
    public Int64 UserId { get; set; }

    [JsonProperty("ordem")]
    public Int64 Order { get; set; }
}
