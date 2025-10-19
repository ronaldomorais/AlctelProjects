using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Alctel.CRM.API.Entities;

public class TicketAssignmentUserCreateAPI
{
    [JsonProperty("idChamado")]
    public Int64 TicketId { get; set; }

    [JsonProperty("idUserOrigem")]
    public Int64 UserOriginId { get; set; }

    [JsonProperty("idUserDestino")]
    public Int64 UserDestId { get; set; }
}
