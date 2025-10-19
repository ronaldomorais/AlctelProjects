using System.ComponentModel.DataAnnotations;

namespace Alctel.CRM.Web.Models;

public class LoginModel
{
    [Required(ErrorMessage ="Necessário digitar um usuário")]
    public string? Username { get; set; }
    [Required(ErrorMessage = "Senha não pode estar vazia")]
    public string? Password { get; set; }
}
