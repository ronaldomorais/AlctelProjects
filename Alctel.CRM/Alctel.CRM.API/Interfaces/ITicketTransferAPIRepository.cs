using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alctel.CRM.API.Entities;

namespace Alctel.CRM.API.Interfaces;

public interface ITicketTransferAPIRepository
{
    Task<APIResponse<List<TransferAreaQueueAPI>>> GetAreaByQueueAPIAsync(Int64 id);
    Task<APIResponse<List<TransferDemandAreaByAreaAPI>>> GetDemandAreaByAreaAPIAsync(Int64 id);
    Task<APIResponse<List<TransferFormAPI>>> GetTransferFormAPIAsync(Int64 id);
    Task<APIResponse<string>> InsertTicketTransferAPIAsync(string data);
    Task<APIResponse<bool>> IsTicketInTransferQueueAPIAsync(string protocol);
}
