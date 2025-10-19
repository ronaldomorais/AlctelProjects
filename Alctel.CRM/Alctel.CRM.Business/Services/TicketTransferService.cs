using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alctel.CRM.API.Entities;
using Alctel.CRM.API.Interfaces;
using Alctel.CRM.API.Repositories;
using Alctel.CRM.Business.Interfaces;
using Alctel.CRM.Business.Model;
using Newtonsoft.Json;

namespace Alctel.CRM.Business.Services;

public class TicketTransferService : ITicketTransferService
{
    private readonly ITicketTransferAPIRepository _ticketTransferAPIRepository;

    public TicketTransferService(ITicketTransferAPIRepository ticketTransferAPIRepository)
    {
        _ticketTransferAPIRepository = ticketTransferAPIRepository;
    }

    public async Task<List<TransferAreaQueueAPI>> GetAreaByQueueAPIAsync(Int64 id)
    {
        try
        {
            var apiResponse = await _ticketTransferAPIRepository.GetAreaByQueueAPIAsync(id);

            if (apiResponse.IsSuccessStatusCode)
            {
                if (apiResponse.Response != null)
                    return apiResponse.Response;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Exception: {ex.Message}. Trace: {ex.StackTrace}");
        }

        return new List<TransferAreaQueueAPI>();
    }

    public async Task<List<TransferDemandAreaByAreaAPI>> GetDemandAreaByAreaAsync(Int64 id)
    {
        try
        {
            var apiResponse = await _ticketTransferAPIRepository.GetDemandAreaByAreaAPIAsync(id);

            if (apiResponse.IsSuccessStatusCode)
            {
                if (apiResponse.Response != null)
                    return apiResponse.Response;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Exception: {ex.Message}. Trace: {ex.StackTrace}");
        }

        return new List<TransferDemandAreaByAreaAPI>();
    }

    public async Task<List<TransferFormAPI>> GetTransferFormAsync(Int64 id)
    {
        try
        {
            var apiResponse = await _ticketTransferAPIRepository.GetTransferFormAPIAsync(id);

            if (apiResponse.IsSuccessStatusCode)
            {
                if (apiResponse.Response != null)
                    return apiResponse.Response;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Exception: {ex.Message}. Trace: {ex.StackTrace}");
        }

        return new List<TransferFormAPI>();
    }

    public async Task<ResponseServiceModel> InsertTicketTransferAPIAsync(TicketTransferCreateAPI ticketTransferCreateAPI)
    {
        ResponseServiceModel responseServiceModel = new ResponseServiceModel();
        try
        {
            string json = JsonConvert.SerializeObject(ticketTransferCreateAPI, Formatting.Indented);

            var apiResponse = await _ticketTransferAPIRepository.InsertTicketTransferAPIAsync(json);

            responseServiceModel.IsValid = apiResponse.IsSuccessStatusCode;

            if (apiResponse.IsSuccessStatusCode)
            {
                responseServiceModel.Value = apiResponse.Response ?? string.Empty;
            }
            else
            {
                string additionalMessage = apiResponse.AdditionalMessage ?? string.Empty;
                responseServiceModel.Value = additionalMessage;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Exception: {ex.Message}. Trace: {ex.StackTrace}");
        }

        return responseServiceModel;
    }

    public async Task<bool> IsTicketInTransferQueueAsync(string protocol)
    {
        try
        {
            var apiResponse = await _ticketTransferAPIRepository.IsTicketInTransferQueueAPIAsync(protocol);

            if (apiResponse.IsSuccessStatusCode)
            {
                return apiResponse.Response;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Exception: {ex.Message}. Trace: {ex.StackTrace}");
        }

        return false;
    }
}
