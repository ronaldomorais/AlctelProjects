using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Alctel.CRM.API.Entities;

public class TicketAPI
{
    [JsonProperty("idChamado")]
    public Int64 Id { get; set; }

    [JsonProperty("protocolo")]
    public string? Protocol { get; set; }

    [JsonProperty("protocolo_pai")]
    public string? ParentProtocol { get; set; }

    [JsonProperty("dataAbertura")]
    public DateTime? ProtocolDate { get; set; }

    [JsonProperty("tipoProtocolo")]
    public string? ProtocolType { get; set; }

    [JsonProperty("tipoDemanda")]
    public string? DemandType { get; set; }

    [JsonProperty("idMotivoDemanda")]
    public Int64? DemandTypeId { get; set; }

    [JsonProperty("criticidade")]
    public string? TicketCriticality { get; set; }

    [JsonProperty("idChamadoCriticidade")]
    public Int64? TicketCriticalityId { get; set; }

    [JsonProperty("status")]
    public string? TicketStatus { get; set; }

    [JsonProperty("idChamadoStatus")]
    public Int64? TicketStatusId { get; set; }

    [JsonProperty("slaInicial")]
    public string? SlaSystemRole { get; set; }

    //[JsonProperty("")]
	[Newtonsoft.Json.JsonIgnore]
	public string? Sla { get; set; }

    [JsonProperty("filaGT")]
    public string? QueueGT { get; set; }

    [JsonProperty("idFilaGT")]
    public string? QueueGTId { get; set; }

    [JsonProperty("fila")]
    public string? QueueGenesys { get; set; }

    [JsonProperty("termometro")]
    public string? Thermometer { get; set; }

    [JsonProperty("usuario")]
    public string? User { get; set; }

    [JsonProperty("idUsuario")]
    public Int64? UserId { get; set; }

    [JsonProperty("idCliente")]
	public string? CustomerId { get; set; }

    [JsonProperty("nomeCliente")]
    public string? CustomerName { get; set; }

    [JsonProperty("cliente")]
    public string? CustomerFullName { get; set; }

    [JsonProperty("nomeSocial")]
    public string? SocialAffectionateName { get; set; }

    [JsonProperty("cpf")]
    public string? Cpf { get; set; }

    [JsonProperty("navegacao")]
    public string? CustomerNavigation { get; set; }

    [JsonProperty("solucao")]
    public string? AnySolution { get; set; }

    [JsonProperty("informacaoDemanda")]
    public string? DemandInformation { get; set; }

    [JsonProperty("observacaoDemanda")]
    public string? DemandObservation { get; set; }

    [JsonProperty("anexo")]
    public bool HasAttachment { get; set; }

    [JsonProperty("classificacaoChamado")]
    public List<TicketClassification> TicketClassification { get; set; } = new List<TicketClassification>();

    [JsonProperty("logs")]
    public List<LogDataReceived>? Logs { get; set; }
}

public class TicketClassification
{
    //[JsonProperty("idClassificacaoDemanda")]
    //public Int64 ClassificationDemandId { get; set; }

    //[JsonProperty("idClassificacaoTipo")]
    //public Int64 ClassificationTypeId { get; set; }

    //[JsonProperty("idClassificacaoMotivo")]
    //public Int64 ClassificationReasonId { get; set; }

    //[JsonProperty("idLista")]
    //public Int64 ClassificationReasonListId { get; set; }

    //[JsonProperty("idItemLista")]
    //public Int64 ClassificationReasonListItemId { get; set; }

    //[JsonProperty("ordem")]
    //public Int64 ClassificationOrder { get; set; }

    public Int64 ManifestationTypeId { get; set; }
    public string? ManifestationTypeName { get; set; }
    public Int64 ServiceUnitId { get; set; }
    public string? ServiceUnitName { get; set; }
    public Int64 ServiceId { get; set; }
    public string? ServiceName { get; set; }
    public Int64 Reason01Id { get; set; }
    public string? Reason01Name { get; set; }
    public Int64 Reason02Id { get; set; }
    public string? Reason02Name { get; set; }
}
