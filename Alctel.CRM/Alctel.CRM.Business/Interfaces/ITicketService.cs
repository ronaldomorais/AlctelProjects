using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alctel.CRM.API.Entities;
using Alctel.CRM.Business.Model;

namespace Alctel.CRM.Business.Interfaces;

public interface ITicketService
{
    Task<List<TicketAPI>> GetTicketListAPIAsync();
    Task<List<TicketAPI>> GetTicketActivatedListAPIAsync();
    Task<TicketAPI> GetTicketAPIAsync(Int64 id);
    Task<TicketAPI> GetTicketAPIAsync(string protocol);
    Task<ResponseServiceModel> InsertTicketAPIAsync(TicketCreateAPI ticketAPI);
    Task<string> InsertTicketTestAPIAsync(TicketCreateAPI ticketAPI);
    Task<bool> UpdateTicketAPIAsync(TicketAPI TicketAPI);
    Task<List<TicketAPI>> SearchTicketAPIAsync(string filter, string value);
    Task<List<TicketStatusAPI>> GetTicketStatusAPIAsync();
    Task<List<TicketCriticalityAPI>> GetTicketCriticalityAPIAsync();
    Task<int> InsertTicketClassificationAPIAsync(TicketClassificationCreateAPI ticketClassificationCreateAPI);
    Task<List<TicketQueueGTAPI>> GetTicketQueueGTAPIAsync();
    Task<List<TicketAPI>> GetCustomerTicketAPIAsync(Int64 id);
    Task<bool> TicketHasSavedAPIAsync(string protocol);
    Task<bool> UploadTicketAttachmentAPIAsync(TicketAttachmentCreateAPI ticketAttachment);
    Task<List<TicketAttachmentAPI>> DownloadTicketAttachmentAPIAsync(Int64 id);
    Task<List<TicketAPI>> GetTicketListGCAPIAsync(string userid);
    Task<List<TicketAPI>> GetTicketListGCSupervisorAPIAsync(int page = 1, int sizepage = 50);
    Task<int> GetTicketCountAsync();
    Task<List<TicketAgentStatusAPI>> GetTicketAgentStatusAsync();
    Task<List<TicketAssistentStatusAPI>> GetTicketAssistentStatusAsync();
}
