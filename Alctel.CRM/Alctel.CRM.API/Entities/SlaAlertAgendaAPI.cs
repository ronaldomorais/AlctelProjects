using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Alctel.CRM.API.Entities;

public class SlaAlertAgendaAPI
{
    [JsonProperty("idUsuario")]
    public Int64 UserId { get; set; }

    [JsonProperty("descricao")]
    public string? HolidayName { get; set; }

    [JsonProperty("data")]
    public DateTime HolidayDate { get; set; }
}
