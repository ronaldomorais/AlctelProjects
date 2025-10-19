using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Alctel.CRM.API.Entities;

public class UserAPI
{
    //[JsonPropertyName("idUsuario")]
    [JsonProperty("idUsuario")]
    public Int64 Id { get; set; }

    //[JsonPropertyName("nomeUsuario")]
    [JsonProperty("nomeUsuario")]
    public string? Fullname { get; set; }

    //[JsonPropertyName("emailUsuario")]
    [JsonProperty("emailUsuario")]
    public string? Email { get; set; }

    //[JsonPropertyName("unidadeUsuario")]
    [JsonProperty("unidadeUsuario")]
    public string? Unit { get; set; }

    //[JsonPropertyName("areaUsuario")]
    [JsonProperty("areaUsuario")]
    public string? Area { get; set; }

    //[JsonPropertyName("perfilUsuario")]
    [JsonProperty("perfilUsuario")]
    public string? AccessProfile { get; set; }

    //[JsonPropertyName("criacaoUsuario")]
    [JsonProperty("criacaoUsuario")]
    public DateTime CreatedOn { get; set; }

    //[JsonPropertyName("usuarioDesde")]
    [JsonProperty("usuarioDesde")]
    public DateTime StatusSince { get; set; }

    //[JsonPropertyName("statusUsuario")]
    [JsonProperty("statusUsuario")]
    public bool Active { get; set; }

    //[JsonPropertyName("statusUsuario")]
    [JsonProperty("idGenesys")]
    public string? GenesysId { get; set; }

    [JsonProperty("idFilaGT")]
    public string? QueueGTId { get; set; }

    public string? QueueGT { get; set; }

    [JsonProperty("logs")]
    public List<LogDataReceived>? Logs { get; set; }
}
