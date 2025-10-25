using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alctel.CRM.API.Entities;

namespace Alctel.CRM.API.Interfaces;

public interface ITicketClassificationAPIRepository
{
    Task<APIResponse<List<TicketClassificationManifestationTypeAPI>>> GetTicketClassificationManifestationTypeAPIAsync();
    Task<APIResponse<List<TicketClassificationListAPI>>> GetTicketClassificationListAPIAsync();
    Task<APIResponse<List<TicketClassificationListItemsAPI>>> GetTicketClassificationListItemsAPIAsync(Int64 id);
    Task<APIResponse<int>> InsertTicketClassificationListAPIAsync(string data);
    Task<APIResponse<TicketClassificationListItemAPI>> GetTicketClassificationListItemAPIAsync(Int64 id);
    Task<APIResponse<int>> UpdateTicketClassificationListItemAPIAsync(Int64 id, bool status);
    Task<APIResponse<int>> InsertTicketClassificationListItemAPIAsync(Int64 listId, string data);
    Task<APIResponse<int>> InsertTicketClassificationManifestationTypeAPIAsync(string data);
    Task<APIResponse<List<TicketClassificationProgramAPI>>> GetTicketClassificationProgramAPIAsync();
    Task<APIResponse<List<TicketClassificationReasonListAPI>>> GetTicketClassificationReasonListAPIAsync();
    Task<APIResponse<List<TicketClassificationReasonListAPI>>> GetTicketClassificationReasonSonListAPIAsync(Int64 id);
    Task<APIResponse<List<TicketClassificationServiceAPI>>> GetTicketClassificationServiceAPIAsync();
    Task<APIResponse<List<TicketClassificationServiceAPI>>> GetTicketClassificationServiceByManifestationAPIAsync(Int64 id);
    Task<APIResponse<List<TicketClassificationProgramAPI>>> GetTicketClassificationProgramByServiceAPIAsync(Int64 id);
    Task<APIResponse<List<TicketClassificationReasonAPI>>> GetTicketClassificationReasonByManifestationServiceAPIAsync(Int64 manifestationid, Int64 serviceId);
    Task<APIResponse<List<TicketClassificationReasonListItemAPI>>> GetTicketClassificationReasonListItemsAPIAsync(Int64 manifestationid, Int64 serviceId, Int64? parentId);
    Task<APIResponse<string>> InsertTicketClassificationReasonAPIAsync(TicketClassificationReasonCreateAPI data);
    Task<APIResponse<int>> InsertTicketClassificationAPIAsync(TicketClassificationAPI data);
    Task<APIResponse<List<TicketClassficationListAPI>>> GetTicketClassificationByManifestationAPIAsync(Int64 id);
    Task<APIResponse<List<TicketClassificationUnitAPI>>> GetTicketClassificationUnitListAPIAsync(string data);
}
