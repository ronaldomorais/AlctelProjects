using Alctel.CRM.Web.Models;

public class OngoingInteraction
{
    public string? userEmail { get; set; }
    public string? QueueName { get; set; }
    public string? ConversationId { get; set; }
    public string? Cpf { get; set; }
    public string? CustomerEmail { get; set; }
    public string? ParentTicket { get; set; }
    public string? Protocol { get; set; }
    public string? ProtocolType { get; set; }
    public DateTime TicketDate { get; set; }
    //public string? MediaType { get; set; }
    public string? CustomerNavigation { get; set; }
    public IEnumerable<IFormFile>? Files { get; set; }
    public AutoSaveDataModel AutoSaveData { get; set; } = new AutoSaveDataModel();
}

public class OngoingInteractions : List<OngoingInteraction> {}
