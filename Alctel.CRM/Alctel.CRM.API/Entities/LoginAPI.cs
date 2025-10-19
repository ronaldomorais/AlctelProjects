using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Alctel.CRM.API.Entities;

public class LoginAPI
{
    [JsonProperty("idPerfil")]
    public Int64 PerfilId { get; set; }

    [JsonProperty("idUser")]
    public Int64 UserId { get; set; }

    [JsonProperty("perfil")]
    public string? Profile { get; set; }

    [JsonProperty("nomeCompleto")]
    public string? UserName { get; set; }

    [JsonProperty("statusUser")]
    public bool UserStatus { get; set; } = false;

    [JsonProperty("statusPerfil")]
    public bool ProfileStatus { get; set; } = false;
}
