using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Alctel.CRM.API.Entities;

public class TicketCreateAPI
{
    [JsonProperty("idCliente")]
    public Int64 CustomerId { get; set; }

    [JsonProperty("protocolo")]
    public string? Protocol { get; set; }

    [JsonProperty("conversationId")]
    public string? ConversationId { get; set; }

    [JsonProperty("cpf")]
    public string? Cpf { get; set; }

    [JsonProperty("nomeCompleto")]
    public string? FirstName { get; set; }

    //[JsonProperty("idMotivoDemanda")]
    [JsonIgnore]
    public Int64 DemandTypeId { get; set; }

    [JsonProperty("idStatusChamado")]
    public Int64 TicketStatusId { get; set; }

    [JsonProperty("idCriticidadeChamado")]
    public Int64 TicketCriticalityId { get; set; }

    [JsonProperty("idFilaGT")]
    public Int64 QueueGT { get; set; }

    [JsonProperty("observacaoDemanda")]
    public string? DemandObservation { get; set; }

    [JsonProperty("slaInicial")]
    public Int64 SlaSystemRole { get; set; }

    [JsonProperty("navegacao")]
    public string? CustomerNavigation { get; set; }

    [JsonProperty("fila")]
    public string? QueueGenesys { get; set; }

    [JsonProperty("protocoloPai")]
    public string? ParentTicket { get; set; }

    [JsonProperty("idUsuario")]
    public Int64 UserId { get; set; }

    [JsonProperty("informacaoDemanda")]
    public string? DemandInformation { get; set; }

    [JsonProperty("solucionado")]
    public bool AnySolution { get; set; }
}
