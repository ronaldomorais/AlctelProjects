using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace Alctel.CRM.Web.Models;

public class CustomerCreateModel
{
    [Required(ErrorMessage ="Nome é obrigatório")]
    public string? FirstName { get; set; }

    [Required(ErrorMessage = "Sobrenome é obrigatório")]
    public string? LastName { get; set; }

    [Required(ErrorMessage = "CPF é obrigatório")]
    public string? Cpf { get; set; }
    public string? SocialAffectionateName { get; set; }
    public string? Email1 { get; set; }
    public string? Email2 { get; set; }
    public string? Cnpj { get; set; }
    public string? CompanyName { get; set; }
    public string? PhoneNumber1 { get; set; }
    public string? PhoneNumber2 { get; set; }
    public string? PhoneNumberCompany { get; set; }
    //public string? MobilePhone { get; set; }
    public string? Category { get; set; }
    public string? SubCategory { get; set; }
    public string? Registration { get; set; }
    //public DateTime CreatedOn { get; set; }
    //public string? LastControlLog { get; set; }
    public string? ExtraField01 { get; set; }
    public string? ExtraField02 { get; set; }
    public string? ExtraField03 { get; set; }
}
