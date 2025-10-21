using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alctel.CRM.API.Entities;

namespace Alctel.CRM.Business.Interfaces;

public interface ITicketClassificationService
{
    Task<List<TicketClassificationManifestationType>> GetTicketClassificationManifestationTypeAPIAsync();
    Task<List<TicketClassificationListAPI>> GetTicketClassificationListAsync();
    Task<List<TicketClassificationListItemsAPI>> GetTicketClassificationListItemsAsync(Int64 id);
    Task<int> InsertTicketClassificationListAsync(string data);
    Task<TicketClassificationListItemAPI> GetTicketClassificationListItemAsync(Int64 id);
    Task<int> UpdateTicketClassificationListItemAsync(Int64 id, bool status);
    Task<int> InsertTicketClassificationListitemAsync(Int64 listId, string data);
}
