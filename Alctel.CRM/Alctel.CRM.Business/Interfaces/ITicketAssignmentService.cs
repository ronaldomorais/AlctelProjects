using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alctel.CRM.API.Entities;

namespace Alctel.CRM.Business.Interfaces;

public interface ITicketAssignmentService
{
    Task<int> InsertTicketAssignmentUserAsync(TicketAssignmentUserCreateAPI ticketAssignmentUserCreateAPI);
    Task<int> InsertTicketAssignmentQueueUserAsync(TicketAssignmentQueueUserCreateAPI ticketAssignmentQueueUserCreateAPI);
}
