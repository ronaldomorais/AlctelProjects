using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Alctel.CRM.API.Entities;

public class TicketAssignmentQueueUserCreateAPI
{
    [JsonProperty("idChamado")]
    public Int64 TicketId { get; set; }

    [JsonProperty("idUserDestino")]
    public Int64 UserDestId { get; set; }
}
