using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alctel.CRM.API.Entities;
using Alctel.CRM.API.Interfaces;
using Alctel.CRM.API.Repositories;
using Alctel.CRM.Business.Interfaces;
using Alctel.CRM.Context.InMemory.Entities;

namespace Alctel.CRM.Business.Services;

public class TicketClassificationService : ITicketClassificationService
{
    private readonly ITicketClassificationAPIRepository _ticketClassificationAPIRepository;
    public TicketClassificationService(ITicketClassificationAPIRepository ticketClassificationAPIRepository)
    {
        _ticketClassificationAPIRepository = ticketClassificationAPIRepository;
    }

    public async Task<List<TicketClassificationManifestationType>> GetTicketClassificationManifestationTypeAPIAsync()
    {
        try
        {
            var apiResponse = await _ticketClassificationAPIRepository.GetTicketClassificationManifestationTypeAPIAsync();

            if (apiResponse.IsSuccessStatusCode)
            {
                if (apiResponse.Response != null)
                {
                    return apiResponse.Response;
                }
            }

        }
        catch (Exception ex)
        {
            Console.WriteLine($"Exception: {ex.Message}. Trace: {ex.StackTrace}");
        }

        return new List<TicketClassificationManifestationType>();
    }

    public async Task<List<TicketClassificationListAPI>> GetTicketClassificationListAsync()
    {
        try
        {
            var apiResponse = await _ticketClassificationAPIRepository.GetTicketClassificationListAPIAsync();

            if (apiResponse.IsSuccessStatusCode)
            {
                if (apiResponse.Response != null)
                {
                    return apiResponse.Response;
                }
            }

        }
        catch (Exception ex)
        {
            Console.WriteLine($"Exception: {ex.Message}. Trace: {ex.StackTrace}");
        }

        return new List<TicketClassificationListAPI>();
    }

    public async Task<List<TicketClassificationListItemsAPI>> GetTicketClassificationListItemsAsync(Int64 id)
    {
        try
        {
            var apiResponse = await _ticketClassificationAPIRepository.GetTicketClassificationListItemsAPIAsync(id);

            if (apiResponse.IsSuccessStatusCode)
            {
                if (apiResponse.Response != null)
                {
                    return apiResponse.Response;
                }
            }

        }
        catch (Exception ex)
        {
            Console.WriteLine($"Exception: {ex.Message}. Trace: {ex.StackTrace}");
        }

        return new List<TicketClassificationListItemsAPI>();
    }

    public async Task<int> InsertTicketClassificationListAsync(string data)
    {
        try
        {
            var apiResponse = await _ticketClassificationAPIRepository.InsertTicketClassificationListAPIAsync(data);

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

    public async Task<TicketClassificationListItemAPI> GetTicketClassificationListItemAsync(Int64 id)
    {
        try
        {
            var apiResponse = await _ticketClassificationAPIRepository.GetTicketClassificationListItemAPIAsync(id);

            if (apiResponse.IsSuccessStatusCode)
            {
                if (apiResponse.Response != null)
                {
                    return apiResponse.Response;
                }
            }

        }
        catch (Exception ex)
        {
            Console.WriteLine($"Exception: {ex.Message}. Trace: {ex.StackTrace}");
        }

        return new TicketClassificationListItemAPI();
    }


    public async Task<int> UpdateTicketClassificationListItemAsync(Int64 id, bool status)
    {
        try
        {
            var apiResponse = await _ticketClassificationAPIRepository.UpdateTicketClassificationListItemAPIAsync(id, status);

            if (apiResponse.IsSuccessStatusCode)
            {
                return apiResponse.Response;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Exception: {ex.Message}. Trace: {ex.StackTrace}");
        }

        return 0;
    }

    public async Task<int> InsertTicketClassificationListitemAsync(Int64 listId, string data)
    {
        try
        {
            var apiResponse = await _ticketClassificationAPIRepository.InsertTicketClassificationListItemAPIAsync(listId, data);

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
