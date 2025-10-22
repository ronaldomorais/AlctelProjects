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
}
