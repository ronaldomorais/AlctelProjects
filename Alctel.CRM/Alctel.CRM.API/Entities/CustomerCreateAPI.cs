using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Alctel.CRM.API.Entities;

public class CustomerCreateAPI
{
    [JsonProperty("nomeCliente")]
    public string? FirstName { get; set; }

    [JsonIgnore]
    public string? LastName { get; set; }

    [JsonProperty("cpfCliente")]
    public string? Cpf { get; set; }

    [JsonProperty("nomeSocialCliente")]
    public string? SocialAffectionateName { get; set; }

    [JsonProperty("emailPrincipalCliente")]
    public string? Email1 { get; set; }

    [JsonProperty("emailSecundarioCliente")]
    public string? Email2 { get; set; }

    [JsonProperty("cnpjCliente")]
    public string? Cnpj { get; set; }

    [JsonProperty("nomeEmpresaCliente")]
    public string? CompanyName { get; set; }

    [JsonProperty("telefonePrimarioCliente")]
    public string? PhoneNumber1 { get; set; }

    [JsonProperty("telefoneSecundarioCliente")]
    public string? PhoneNumber2 { get; set; }

    [JsonProperty("telefoneEmpresaCliente")]
    public string? PhoneNumberCompany { get; set; }
    //public string? MobilePhone { get; set; }

    [JsonProperty("categoriaCliente")]
    public string? Category { get; set; }

    [JsonProperty("subCategoriaCliente")]
    public string? SubCategory { get; set; }

    [JsonProperty("matriculaCliente")]
    public string? Registration { get; set; }
    //public DateTime CreatedOn { get; set; }
    //public string? LastControlLog { get; set; }

    [JsonProperty("campo1")]
    public string? ExtraField01 { get; set; }

    [JsonProperty("campo2")]
    public string? ExtraField02 { get; set; }

    [JsonProperty("campo3")]
    public string? ExtraField03 { get; set; }
}
