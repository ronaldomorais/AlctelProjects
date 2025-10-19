using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Alctel.CRM.Web.Models;

public class UserModel
{
    public Int64 Id { get; set; }
    //[Required(ErrorMessage = "Nome � obrigat�rio")]
    public string? Fullname { get; set; }
    //[Required(ErrorMessage = "Email � obrigat�rio")]
    public string? Email { get; set; }

    [Required(ErrorMessage = "Unidade � obrigat�rio")]
    public string? Unit { get; set; }

    [Required(ErrorMessage = "�rea � obrigat�rio")]
    public string? Area { get; set; }

    [Required(ErrorMessage = "Perfil � obrigat�rio")]
    public string? AccessProfile { get; set; }
    public DateTime CreatedOn { get; set; }
    public bool Active { get; set; }
    public string? GenesysId { get; set; }
    public DateTime StatusSince { get; set; }
    public Int64? QueueGTId { get; set; }
    public string? QueueGT { get; set; }

    public List<SelectListItem>? AccessProfileOptions { get; set; }

    public List<SelectListItem>? UnitServiceOptions { get; set; }

    public List<SelectListItem>? AreaOptions { get; set; }

    public List<SelectListItem>? QueueGTOptions { get; set; }

    public List<LogDataReceivedModel>? Logs { get; set; }

    public UserDataToCompareIfChangedLog UserDataToCompareIfChanged { get; set; } = new UserDataToCompareIfChangedLog();
}

public class UserDataToCompareIfChangedLog
{
    public Int64 Id { get; set; }
    public bool Active { get; set; }
    public string? AccessProfile { get; set; }
    public string? Area { get; set; }
    public string? Unit { get; set; }
    public Int64? QueueGTId { get; set; }
}