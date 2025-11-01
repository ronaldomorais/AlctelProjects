using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Alctel.CRM.Web.Models;

public class TicketTransferModel
{
    public Int64 TicketId { get; set; }

    public string? Protocol { get; set; }

    public Int64 UserId { get; set; }

    [Required(ErrorMessage = "Fila GT é obrigatório")]
    [DisplayName("Nível de Atendimento")]
    public Int64 TransferQueueGTId { get; set; }
    public List<SelectListItem> TransferQueueGTOptions { get; set; } = new List<SelectListItem>();

    public Int64 ListId { get; set; }

    [Required(ErrorMessage = "Unidade é obrigatório")]
    [DisplayName("Unidade")]
    public Int64 TransferServiceUnitId { get; set; }

    [Required(ErrorMessage = "Opções é obrigatório")]
    [DisplayName("Opções")]
    public Int64 TransferAreaId { get; set; }
    //public List<SelectListItem> TransferAreaOptions { get; set; } = new List<SelectListItem>();

    [Required(ErrorMessage = "Tipo de Demanda é obrigatório")]
    [DisplayName("Tipo de Demanda")]
    public string? TransferDemandTypeId { get; set; }
    //public List<SelectListItem> TransferAreaOptions { get; set; } = new List<SelectListItem>();

    [DisplayName("Formulários")]
    public string? TransferDemandObservation { get; set; }

    [DisplayName("Motivo Transferência")]
    public string? TransferReason { get; set; }

    public string? QueueGT { get; set; }

    public string? ScreenOrigin { get; set; }
}
