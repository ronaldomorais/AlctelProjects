using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alctel.CRM.API.Entities;
using Alctel.CRM.Business.Model;

namespace Alctel.CRM.Business.Interfaces;

public interface ITicketTransferService
{
    Task<List<TransferAreaQueueAPI>> GetAreaByQueueAPIAsync(Int64 id);
    Task<List<TransferDemandAreaByAreaAPI>> GetDemandAreaByAreaAsync(Int64 id);
    Task<List<TransferFormAPI>> GetTransferFormAsync(Int64 id);
    Task<ResponseServiceModel> InsertTicketTransferAPIAsync(TicketTransferCreateAPI ticketTransferCreateAPI);
    Task<bool> IsTicketInTransferQueueAsync(string protocol);
}
