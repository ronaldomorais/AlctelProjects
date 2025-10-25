using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alctel.CRM.API.Entities;
using Alctel.CRM.API.Interfaces;
using Alctel.CRM.Business.Interfaces;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Alctel.CRM.Business.Services;

public class SlaService : ISlaService
{
    private readonly ISlaAPIRepository _slaAPIRepository;
    public SlaService(ISlaAPIRepository slaAPIRepository)
    {
        _slaAPIRepository = slaAPIRepository;
    }

    public async Task<int> InsertSlaCalendarAsync(SlaAlertAgendaAPI data)
    {
        try
        {
            var apiResponse = await _slaAPIRepository.InsertSlaCalendarAPIAsync(data);

            if (apiResponse.IsSuccessStatusCode)
            {
                return apiResponse.Response;
            }

        }
        catch (Exception ex)
        {
            Console.WriteLine($"Exception: {ex.Message}. Trace: {ex.StackTrace}");
        }

        return -1;
    }

    public async Task<List<SlaAlertAgendaAPI>?> GetSlaCalendarAsync()
    {
        try
        {
            var apiResponse = await _slaAPIRepository.GetSlaCalendarAPIAsync();

            if (apiResponse.IsSuccessStatusCode)
            {
                return apiResponse.Response;
            }

        }
        catch (Exception ex)
        {
            Console.WriteLine($"Exception: {ex.Message}. Trace: {ex.StackTrace}");
        }

        return new List<SlaAlertAgendaAPI>();
    }

    public async Task<List<SlaTicketConfigAPI>?> GetSlaTicketConfigAsync()
    {
        try
        {
            var apiResponse = await _slaAPIRepository.GetSlaTicketConfigAPIAsync();

            if (apiResponse.IsSuccessStatusCode)
            {
                return apiResponse.Response;
            }

        }
        catch (Exception ex)
        {
            Console.WriteLine($"Exception: {ex.Message}. Trace: {ex.StackTrace}");
        }

        return new List<SlaTicketConfigAPI>();
    }

    public async Task<int> InsertSlaTicketConfigAsync(SlaTicketCreateAPI data)
    {
        try
        {
            var apiResponse = await _slaAPIRepository.InsertSlaTicketConfigAPIAsync(data);

            if (apiResponse.IsSuccessStatusCode)
            {
                return apiResponse.Response;
            }

        }
        catch (Exception ex)
        {
            Console.WriteLine($"Exception: {ex.Message}. Trace: {ex.StackTrace}");
        }

        return -1;
    }

    public async Task<int> GetBusinessDays(DateTime startTime, DateTime endTime)
    {
        try
        {
            var apiResponse = await _slaAPIRepository.GetBusinessDaysAPI(startTime, endTime);

            if (apiResponse.IsSuccessStatusCode)
            {
                return apiResponse.Response;
            }

        }
        catch (Exception ex)
        {
            Console.WriteLine($"Exception: {ex.Message}. Trace: {ex.StackTrace}");
        }

        return -1;
    }
}
