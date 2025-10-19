using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alctel.CRM.API.Entities;
using Alctel.CRM.API.Interfaces;
using Alctel.CRM.Business.Interfaces;
using Newtonsoft.Json;

namespace Alctel.CRM.Business.Services;

public class TicketAssignmentService : ITicketAssignmentService
{
    private readonly ITicketAssignmentRepository _ticketAssignmentRepository;
    public TicketAssignmentService(ITicketAssignmentRepository ticketAssignmentRepository)
    {            
        _ticketAssignmentRepository = ticketAssignmentRepository;
    }

    public async Task<int> InsertTicketAssignmentUserAsync(TicketAssignmentUserCreateAPI ticketAssignmentUserCreateAPI)
    {
        int ret = -1;
        try
        {
            string json = JsonConvert.SerializeObject(ticketAssignmentUserCreateAPI, Formatting.Indented);

            var apiResponse = await _ticketAssignmentRepository.InsertTicketAssignmentUserAPIAsync(json);

            if (apiResponse.IsSuccessStatusCode)
            {
                return apiResponse.Response;
            }
        }
        catch (Exception ex)
        {
            //message = $"Exception: {ex.Message}. Trace: {ex.StackTrace}";
            Console.WriteLine($"Exception: {ex.Message}. Trace: {ex.StackTrace}");
        }

        return ret;
    }

    public async Task<int> InsertTicketAssignmentQueueUserAsync(TicketAssignmentQueueUserCreateAPI ticketAssignmentQueueUserCreateAPI)
    {
        int ret = -1;
        try
        {
            string json = JsonConvert.SerializeObject(ticketAssignmentQueueUserCreateAPI, Formatting.Indented);

            var apiResponse = await _ticketAssignmentRepository.InsertTicketAssignmentQueueUserAPIAsync(json);

            if (apiResponse.IsSuccessStatusCode)
            {
                return apiResponse.Response;
            }
        }
        catch (Exception ex)
        {
            //message = $"Exception: {ex.Message}. Trace: {ex.StackTrace}";
            Console.WriteLine($"Exception: {ex.Message}. Trace: {ex.StackTrace}");
        }

        return ret;
    }
}
