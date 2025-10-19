using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alctel.CRM.API.Entities;

namespace Alctel.CRM.API.Interfaces;

public interface ITicketAssignmentRepository
{
    Task<APIResponse<int>> InsertTicketAssignmentUserAPIAsync(string data);
    Task<APIResponse<int>> InsertTicketAssignmentQueueUserAPIAsync(string data);
}
