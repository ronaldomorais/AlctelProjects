using System.ComponentModel.DataAnnotations;

namespace Alctel.CRM.Web.Models;

public class CustomerModel
{
    public Int64 Id { get; set; }
    //[Required(ErrorMessage ="Nome é obrigatório")]
    public string? FirstName { get; set; }

    //[Required(ErrorMessage ="Sobre nome é obrigatório")]
    public string? LastName { get; set; }

    //[Required(ErrorMessage ="CPF é obrigatório")]
    public string? Cpf { get; set; }
    public string? SocialAffectionateName { get; set; }
    //[Required(ErrorMessage = "Email 1 é obrigatório")]
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
    public DateTime CreatedOn { get; set; }
    public string? ExtraField01 { get; set; }
    public string? ExtraField02 { get; set; }
    public string? ExtraField03 { get; set; }

    public List<TicketModel>? TicketCustomer { get; set; }
    public List<LogDataReceivedModel>? Logs { get; set; }
    public CustomerDataToCompareIfChangedLog CustomerDataToCompareIfChanged { get; set; } = new CustomerDataToCompareIfChangedLog();
}

public class CustomerDataToCompareIfChangedLog
{
    public Int64 Id { get; set; }
    public string? SocialAffectionateName { get; set; }
    public string? PhoneNumber2 { get; set; }
    public string? Email2 { get; set; }
    public string? Cnpj { get; set; }
    public string? CompanyName { get; set; }
    public string? PhoneNumberCompany { get; set; }
    public string? ExtraField01 { get; set; }
    public string? ExtraField02 { get; set; }
    public string? ExtraField03 { get; set; }

}