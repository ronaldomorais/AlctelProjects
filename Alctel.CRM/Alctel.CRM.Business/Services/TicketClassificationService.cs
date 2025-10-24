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
using Alctel.CRM.Context.InMemory.Entities;

namespace Alctel.CRM.Business.Services;

public class TicketClassificationService : ITicketClassificationService
{
    private readonly ITicketClassificationAPIRepository _ticketClassificationAPIRepository;
    public TicketClassificationService(ITicketClassificationAPIRepository ticketClassificationAPIRepository)
    {
        _ticketClassificationAPIRepository = ticketClassificationAPIRepository;
    }

    public async Task<List<TicketClassificationManifestationTypeAPI>> GetTicketClassificationManifestationTypeAPIAsync()
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

        return new List<TicketClassificationManifestationTypeAPI>();
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

    public async Task<int> InsertTicketClassificationManifestationTypeAsync(string data)
    {
        try
        {
            var apiResponse = await _ticketClassificationAPIRepository.InsertTicketClassificationManifestationTypeAPIAsync(data);

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

    public async Task<List<TicketClassificationProgramAPI>> GetTicketClassificationProgramAsync()
    {
        try
        {
            var apiResponse = await _ticketClassificationAPIRepository.GetTicketClassificationProgramAPIAsync();

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

        return new List<TicketClassificationProgramAPI>();
    }

    public async Task<List<TicketClassificationReasonListAPI>> GetTicketClassificationReasonListAsync()
    {
        try
        {
            var apiResponse = await _ticketClassificationAPIRepository.GetTicketClassificationReasonListAPIAsync();

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

        return new List<TicketClassificationReasonListAPI>();
    }

    public async Task<List<TicketClassificationReasonListAPI>> GetTicketClassificationReasonSonListAsync(Int64 id)
    {
        try
        {
            var apiResponse = await _ticketClassificationAPIRepository.GetTicketClassificationReasonSonListAPIAsync(id);

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

        return new List<TicketClassificationReasonListAPI>();
    }


    public async Task<List<TicketClassificationServiceAPI>> GetTicketClassificationServiceAsync()
    {
        try
        {
            var apiResponse = await _ticketClassificationAPIRepository.GetTicketClassificationServiceAPIAsync();

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

        return new List<TicketClassificationServiceAPI>();
    }

    public async Task<List<TicketClassificationServiceAPI>> GetTicketClassificationServiceByManifestationAsync(Int64 id)
    {
        try
        {
            var apiResponse = await _ticketClassificationAPIRepository.GetTicketClassificationServiceByManifestationAPIAsync(id);

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

        return new List<TicketClassificationServiceAPI>();
    }

    public async Task<List<TicketClassificationProgramAPI>> GetTicketClassificationProgramByServiceAsync(Int64 id)
    {
        try
        {
            var apiResponse = await _ticketClassificationAPIRepository.GetTicketClassificationProgramByServiceAPIAsync(id);

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

        return new List<TicketClassificationProgramAPI>();
    }

    public async Task<List<TicketClassificationReasonAPI>> GetTicketClassificationReasonByManifestationServiceAsync(Int64 manifestationid, Int64 serviceId)
    {
        try
        {
            var apiResponse = await _ticketClassificationAPIRepository.GetTicketClassificationReasonByManifestationServiceAPIAsync(manifestationid, serviceId);

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

        return new List<TicketClassificationReasonAPI>();
    }

    public async Task<List<TicketClassificationReasonListItemAPI>> GetTicketClassificationReasonListItemsAsync(Int64 manifestationid, Int64 serviceId, Int64? parentId)
    {
        try
        {
            var apiResponse = await _ticketClassificationAPIRepository.GetTicketClassificationReasonListItemsAPIAsync(manifestationid, serviceId, parentId);

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

        return new List<TicketClassificationReasonListItemAPI>();
    }

    public async Task<ResponseServiceModel> InsertTicketClassificationReasonAsync(TicketClassificationReasonCreateAPI data)
    {
        ResponseServiceModel responseServiceModel = new ResponseServiceModel();
        try
        {
            var apiResponse = await _ticketClassificationAPIRepository.InsertTicketClassificationReasonAPIAsync(data);

            if (apiResponse.IsSuccessStatusCode)
            {
                responseServiceModel.Value = apiResponse.Response ?? string.Empty;
            }
            else
            {
                string additionalMessage = apiResponse.AdditionalMessage ?? string.Empty;

                if (additionalMessage.Contains("uq_classificacao_servico_manifestacao_programa_nome"))
                {
                    responseServiceModel.Value = "Duplicidade: Já existe um serviço com esse nome. Não é permitido cadastrar serviços com o mesmo nome";
                }
                else
                {
                    responseServiceModel.Value = additionalMessage;
                }
            }

        }
        catch (Exception ex)
        {
            Console.WriteLine($"Exception: {ex.Message}. Trace: {ex.StackTrace}");
        }

        return responseServiceModel;
    }

    public async Task<int> InsertTicketClassificationAPIAsync(TicketClassificationAPI data)
    {
        try
        {
            var apiResponse = await _ticketClassificationAPIRepository.InsertTicketClassificationAPIAsync(data);

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
