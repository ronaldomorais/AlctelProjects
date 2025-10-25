using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alctel.CRM.API.Entities;

namespace Alctel.CRM.Business.Interfaces;

public interface ISlaService
{
    Task<int> InsertSlaCalendarAsync(SlaAlertAgendaAPI data);
    Task<List<SlaAlertAgendaAPI>?> GetSlaCalendarAsync();
    Task<List<SlaTicketConfigAPI>?> GetSlaTicketConfigAsync();
    Task<int> InsertSlaTicketConfigAsync(SlaTicketCreateAPI data);
    Task<int> GetBusinessDays(DateTime startTime, DateTime endTime);
}
