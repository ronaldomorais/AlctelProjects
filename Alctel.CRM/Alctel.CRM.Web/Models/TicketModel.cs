using System.ComponentModel.DataAnnotations;
using Alctel.CRM.API.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;

namespace Alctel.CRM.Web.Models;

public class TicketModel
{
    public bool TicketSaved { get; set; }
    public bool TicketInTransfering { get; set; }
    public Int64 Id { get; set; }
    public string? ConversationId { get; set; }

    [Display(Name = "Protocolo")]
    public string? Protocol { get; set; }

    [Display(Name = "Protocolo Pai")]
    public string? ParentProtocol { get; set; }

    [Display(Name = "Data do Protocolo")]
    public DateTime ProtocolDate { get; set; }

    [Display(Name = "Tipo de Protocolo")]
    public string? ProtocolType { get; set; }

    [Display(Name = "Data Abertura Chamado")]
    public DateTime? TicketDate { get; set; }

    //[Required(ErrorMessage = "Tipo de Demanda é obrigatório")]
    //[Display(Name = "Tipo de Demanda")]
    public string? DemandType { get; set; }

    //[Required(ErrorMessage = "Tipo de Demanda é obrigatório")]
    [Display(Name = "Tipo de Demanda")]
    public Int64? DemandTypeId { get; set; }
    public List<SelectListItem> DemandTypeOptions { get; set; } = new List<SelectListItem>();

    //[Required(ErrorMessage = "Criticidade é obrigatório")]
    //[Display(Name = "Criticidade")]
    public string? TicketCriticality { get; set; }

    [Required(ErrorMessage = "Criticidade é obrigatório")]
    [Display(Name = "Criticidade")]
    public Int64? TicketCriticalityId { get; set; }
    public List<SelectListItem> TicketCriticalityOptions { get; set; } = new List<SelectListItem>();

    //[Required(ErrorMessage = "Status é obrigatório")]
    //[Display(Name = "Estado")]
    public string? TicketStatus { get; set; }

    [Required(ErrorMessage = "Estado é obrigatório")]
    [Display(Name = "Estado")]
    public Int64? TicketStatusId { get; set; }
    public List<SelectListItem> TicketStatusOptions { get; set; } = new List<SelectListItem>();

    [Display(Name = "SLA Inicial")]
    public string? SlaSystemRole { get; set; }

    [Display(Name = "SLA Contador")]
    public string? Sla { get; set; }

    [Display(Name = "Nível de Atendimento")]
    public string? QueueGT { get; set; }

    [Display(Name = "Nível de Atendimento")]
    public Int64 QueueGTId { get; set; }
    public List<SelectListItem> QueueGTOptions { get; set; } = new List<SelectListItem>();


    [Display(Name = "Fila Genesys")]
    public string? QueueGenesys { get; set; }

    [Display(Name = "Sentimento do Cliente")]
    public string? Thermometer { get; set; }

    [Display(Name = "Usuário")]
    public string? User { get; set; }
    public Int64? UserId { get; set; }

    [Display(Name = "Navegação")]
    public string? CustomerNavigation { get; set; }

    [Display(Name = "Fila")]
    public string? Queue { get; set; }

    [Display(Name = "Classificação")]
    public string? Classification { get; set; }

    [Display(Name = "Conteúdo do Email")]
    public string? DemandInformation { get; set; }

    [Required(ErrorMessage = "Informações da Demanda é obrigatório")]
    [Display(Name = "Informações da Demanda")]
    public string? DemandObservation { get; set; }

    [Required(ErrorMessage = "Foi dada a Solução para o Cliente é obrigatório")]
    [Display(Name = "Foi dada a Solução para o Cliente?")]
    public string? AnySolution { get; set; }
    public List<SelectListItem> AnySolutionOptions { get; set; } = new List<SelectListItem>();

    public string? ParentTicket { get; set; }

    public string? CustomerFullName { get; set; }
    public CustomerModel Customer { get; set; } = new CustomerModel();
    public bool HasAttachment { get; set; }

    public ClassificationTreeModel ClassificationTree { get; set; } = new ClassificationTreeModel();

    public List<TicketClassification> TicketClassification { get; set; } = new List<TicketClassification>();

    public List<TicketModel> TicketCustomer { get; set; } = new List<TicketModel>();

    public List<LogDataReceivedModel>? Logs { get; set; }
    public TicketDataToCompareIfChangedLog TicketDataToCompareIfChanged { get; set; } = new TicketDataToCompareIfChangedLog();

    public List<IFormFile> Files { get; set; } = new List<IFormFile>();

    public List<TicketAttachmentModel>? Attachments { get; set; }
}

public class TicketDataToCompareIfChangedLog
{
    public Int64 Id { get; set; }
    public Int64 QueueGTId { get; set; }
    public string? QueueGT { get; set; }
    public Int64? DemandTypeId { get; set; }
    public string? DemandType { get; set; }
    public Int64? TicketCriticalityId { get; set; }
    public string? TicketCriticality { get; set; }
    public Int64? TicketStatusId { get; set; }
    public string? TicketStatus { get; set; }
    public string? AnySolution { get; set; }
    public string? DemandObservation { get; set; }
}



