namespace Alctel.CRM.Context.InMemory.Entities;

public class Customer
{
    public Int64 Id { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Cpf { get; set; }
    public string? SocialAffectionateName { get; set; }
    public string? Email1 { get; set; }
    public string? Email2 { get; set; }
    public string? Cnpj { get; set; }
    public string? CompanyName { get; set; }
    public string? PhoneNumber { get; set; }
    public string? PhoneNumber1 { get; set; }
    public string? PhoneNumber2 { get; set; }
    public string? MobilePhone { get; set; }
    public string? Category { get; set; }
    public string? SubCategory { get; set; }
    public string? Registration { get; set; }
    public DateTime CreatedOn { get; set; }
    public string? LastControlLog { get; set; }
}