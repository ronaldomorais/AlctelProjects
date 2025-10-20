using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alctel.CRM.API.Entities;

namespace Alctel.CRM.API.Interfaces;

public interface ITicketClassificationAPIRepository
{
    Task<APIResponse<List<TicketClassificationManifestationType>>> GetTicketClassificationManifestationTypeAPIAsync();
    Task<APIResponse<List<TicketClassificationListAPI>>> GetTicketClassificationListAPIAsync();
    Task<APIResponse<List<TicketClassificationListItemsAPI>>> GetTicketClassificationListItemsAPIAsync(Int64 id);
}
