using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Alctel.CRM.API.Entities;

public class CustomerAPI
{
    [JsonProperty("idCliente")]

    public Int64 Id { get; set; }

    [JsonProperty("nomeCompleto")]
    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    [JsonProperty("cpf")]
    public string? Cpf { get; set; }

    [JsonProperty("nomeSocial")]
    public string? SocialAffectionateName { get; set; }

    [JsonProperty("emailPrincipal")]
    public string? Email1 { get; set; }

    [JsonProperty("emailSecundario")]
    public string? Email2 { get; set; }

    [JsonProperty("cnpj")]
    public string? Cnpj { get; set; }
    [JsonProperty("nomeEmpresa")]
    public string? CompanyName { get; set; }

    [JsonProperty("telefonePrincipal")]
    public string? PhoneNumber1 { get; set; }

    [JsonProperty("telefoneSecundario")]
    public string? PhoneNumber2 { get; set; }

    [JsonProperty("telefoneEmpresa")]
    public string? PhoneNumberCompany { get; set; }
    //public string? MobilePhone { get; set; }

    [JsonProperty("categoria")]
    public string? Category { get; set; }

    [JsonProperty("subCategoria")]
    public string? SubCategory { get; set; }

    [JsonProperty("matricula")]
    public string? Registration { get; set; }
    public DateTime CreatedOn { get; set; }
    //public string? LastControlLog { get; set; }

    [JsonProperty("campo1")]
    public string? ExtraField01 { get; set; }

    [JsonProperty("campo2")]
    public string? ExtraField02 { get; set; }

    [JsonProperty("campo3")]
    public string? ExtraField03 { get; set; }

    [JsonProperty("logs")]
    public List<LogDataReceived>? Logs { get; set; }    
}
