using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alctel.CRM.API.Entities;
using Alctel.CRM.Business.Model;

namespace Alctel.CRM.Business.Interfaces;

public interface ITicketClassificationService
{
    Task<List<TicketClassificationManifestationTypeAPI>> GetTicketClassificationManifestationTypeAPIAsync();
    Task<List<TicketClassificationListAPI>> GetTicketClassificationListAsync();
    Task<List<TicketClassificationListItemsAPI>> GetTicketClassificationListItemsAsync(Int64 id);
    Task<int> InsertTicketClassificationListAsync(string data);
    Task<TicketClassificationListItemAPI> GetTicketClassificationListItemAsync(Int64 id);
    Task<int> UpdateTicketClassificationListItemAsync(Int64 id, bool status);
    Task<int> InsertTicketClassificationListitemAsync(Int64 listId, string data);
    Task<int> InsertTicketClassificationManifestationTypeAsync(string data);
    Task<List<TicketClassificationProgramAPI>> GetTicketClassificationProgramAsync();
    Task<List<TicketClassificationReasonListAPI>> GetTicketClassificationReasonListAsync();
    Task<List<TicketClassificationReasonListAPI>> GetTicketClassificationReasonSonListAsync(Int64 id);
    Task<List<TicketClassificationServiceAPI>> GetTicketClassificationServiceAsync();
    Task<List<TicketClassificationServiceAPI>> GetTicketClassificationServiceByManifestationAsync(Int64 id);
    Task<List<TicketClassificationProgramAPI>> GetTicketClassificationProgramByServiceAsync(Int64 id);
    Task<List<TicketClassificationReasonAPI>> GetTicketClassificationReasonByManifestationServiceAsync(Int64 manifestationid, Int64 serviceId);
    Task<List<TicketClassificationReasonListItemAPI>> GetTicketClassificationReasonListItemsAsync(Int64 manifestationid, Int64 serviceId, Int64? parentId);
    Task<ResponseServiceModel> InsertTicketClassificationReasonAsync(TicketClassificationReasonCreateAPI data);
    Task<int> InsertTicketClassificationAPIAsync(TicketClassificationAPI data);
    Task<List<TicketClassficationListAPI>> GetTicketClassificationByManifestationAsync(Int64 id);
    Task<List<TicketClassificationUnitAPI>> GetTicketClassificationUnitListAsync(string data);
    Task<List<TicketClassificationListAPI>> SearchTicketClassificationListAsync(string searchlistType, string searchlistText);
    Task<int> UpdateTicketClassificationAsync(TicketClassificationUpdateAPI data);
}
