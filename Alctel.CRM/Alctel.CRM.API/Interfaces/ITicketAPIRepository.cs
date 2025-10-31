using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alctel.CRM.API.Entities;

namespace Alctel.CRM.API.Interfaces;

public interface ITicketAPIRepository
{
    Task<APIResponse<List<TicketAPI>>> GetTicketListAPIAsync();
    Task<APIResponse<List<TicketAPI>>> GetTicketActivatedListAPIAsync();
    Task<APIResponse<TicketAPI>> GetTicketAPIAsync(Int64 id);
    Task<APIResponse<TicketAPI>> GetTicketAPIAsync(string protocol);
    Task<APIResponse<string>> InsertTicketAPIAsync(string data);
    Task<APIResponse<string>> InsertTicketTestAPIAsync(string data);
    Task<APIResponse<bool>> UpdateTicketAPIAsync(string data);
    Task<APIResponse<List<TicketAPI>>> SearchDataAsync(string data);
    Task<APIResponse<List<TicketStatusAPI>>> GetTicketStatusAPIAsync();
    Task<APIResponse<List<TicketCriticalityAPI>>> GetTicketCriticalityAPIAsync();
    Task<APIResponse<int>> InsertTicketClassificationAPIAsync(string data);
    Task<APIResponse<List<TicketQueueGTAPI>>> GetTicketQueueGTAPIAsync();
    Task<APIResponse<List<TicketQueueGTAPI>>> GetAllTicketQueueGTAPIAsync();
    Task<APIResponse<List<TicketAPI>>> GetCustomerTicketAPIAsync(Int64 id);
    Task<APIResponse<bool>> TicketHasSavedAPIAsync(string protocol);
    Task<APIResponse<bool>> UploadTicketAttachmentAPIAsync(TicketAttachmentCreateAPI ticketAttachment);
    Task<APIResponse<List<TicketAttachmentAPI>>> DownloadTicketAttachmentAPIAsync(Int64 id);
    Task<APIResponse<List<TicketAPI>>> GetTicketListGCAPIAsync(string userid);
    Task<APIResponse<List<TicketAPI>>> GetTicketListGCSupervisorAPIAsync(int page = 1, int sizepage = 50);
    Task<APIResponse<int>> GetDataAsync();
    Task<APIResponse<List<TicketAgentStatusAPI>>> GetTicketAgentStatusAPIAsync();
    Task<APIResponse<List<TicketAssistentStatusAPI>>> GetTicketAssistentStatusAPIAsync();
}
