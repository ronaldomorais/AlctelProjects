using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alctel.CRM.API.Entities;

namespace Alctel.CRM.API.Interfaces;

public interface ISlaAPIRepository
{
    Task<APIResponse<int>> InsertSlaCalendarAPIAsync(SlaAlertAgendaAPI data);
    Task<APIResponse<List<SlaAlertAgendaAPI>>> GetSlaCalendarAPIAsync();
    Task<APIResponse<List<SlaTicketConfigAPI>>> GetSlaTicketConfigAPIAsync();
    Task<APIResponse<int>> InsertSlaTicketConfigAPIAsync(SlaTicketCreateAPI data);
    Task<APIResponse<int>> GetBusinessDaysAPI(DateTime startTime, DateTime endTime);
}
