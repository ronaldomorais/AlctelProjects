using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alctel.CRM.API.Entities;
using Alctel.CRM.API.Interfaces;
using Alctel.CRM.API.Repositories;
using Alctel.CRM.Business.Interfaces;
using Alctel.CRM.Business.Model;
using Alctel.CRM.Context.InMemory.Entities;
using Alctel.CRM.Context.InMemory.Interfaces;
using Newtonsoft.Json;

namespace Alctel.CRM.Business.Services;

public class TicketService : ITicketService
{
    private readonly ITicketAPIRepository _ticketAPIRepository;
    private readonly ICustomerService _customerService;

    private List<TicketAPI> _ticketApis = new List<TicketAPI>();


    public TicketService(ITicketAPIRepository ticketAPIRepository, ICustomerService customerService)
    {
        _ticketAPIRepository = ticketAPIRepository;
        _customerService = customerService;

        //var customer_01 = _customerService.GetCustomerAPIAsync(6).Result;

        //_ticketApis.Add(new TicketAPI
        //{
        //    Id = 1,
        //    Protocol = "20250913100001",
        //    ProtocolDate = DateTime.Now.AddDays(-10),
        //    ProtocolType = "Pai",
        //    DemandType = "Demanda",
        //    Criticality = "Alta",
        //    Status = "Em Atendimento",
        //    SlaSystemRole = "1 hora",
        //    QueueGT = "Nível 1",
        //    QueueGenesys = "Suporte",
        //    Thermometer = "Integração",
        //    User = "Gisele",
        //    AnySolution = "Sim",
        //    Customer = customer_01
        //});

        //var customer_02 = _customerService.GetCustomerAPIAsync(7).Result;
        //_ticketApis.Add(new TicketAPI
        //{
        //    Id = 2,
        //    Protocol = "20250913100002",
        //    ProtocolDate = DateTime.Now.AddDays(-3),
        //    ProtocolType = "Filho",
        //    DemandType = "Demanda",
        //    Criticality = "Baixa",
        //    Status = "Resolvido",
        //    SlaSystemRole = "3 hora",
        //    QueueGT = "Nível 2",
        //    QueueGenesys = "Financeiro",
        //    Thermometer = "Integração",
        //    User = "Adalberto",
        //    AnySolution = "Não",
        //    Customer = customer_02
        //});
    }

    public async Task<List<TicketAPI>> GetTicketListAPIAsync()
    {
        try
        {
            var apiResponse = await _ticketAPIRepository.GetTicketListAPIAsync();

            if (apiResponse.IsSuccessStatusCode)
            {
                if (apiResponse.Response != null)
                {
                    return apiResponse.Response;
                    //List<TicketAPI> tickets = apiResponse.Response as List<TicketAPI>;
                    //for (int i = 0; i < tickets.Count; i++)
                    //{
                    //    TicketAPI ticket = tickets[i];
                    //    var customerId = ticket.CustomerId;

                    //    if (customerId != null)
                    //    {
                    //        var customer = await _customerService.GetCustomerAPIAsync(Int64.Parse(customerId));

                    //        if (customer != null)
                    //        {
                    //            tickets[i].CustomerName = customer.FirstName;
                    //        }
                    //    }
                    //}

                    //return tickets;
                }
            }

        }
        catch (Exception ex)
        {
            Console.WriteLine($"Exception: {ex.Message}. Trace: {ex.StackTrace}");
        }

        return new List<TicketAPI>();
    }

    public async Task<List<TicketAPI>> GetTicketActivatedListAPIAsync()
    {
        //try
        //{
        //    var apiResponse = await _ticketAPIRepository.GetTicketActivatedListAPIAsync();

        //    if (apiResponse.IsSuccessStatusCode)
        //    {
        //        if (apiResponse.Response != null)
        //            return apiResponse.Response;
        //    }
        //}
        //catch (Exception ex)
        //{
        //    Console.WriteLine($"Exception: {ex.Message}. Trace: {ex.StackTrace}");
        //}

        //return new List<TicketAPI>();

        return _ticketApis;
    }

    public async Task<TicketAPI> GetTicketAPIAsync(Int64 id)
    {
        try
        {
            var apiResponse = await _ticketAPIRepository.GetTicketAPIAsync(id);

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

        return new TicketAPI();
    }

    public async Task<ResponseServiceModel> InsertTicketAPIAsync(TicketCreateAPI ticketAPI)
    {
        ResponseServiceModel responseServiceModel = new ResponseServiceModel();
        try
        {
            string json = JsonConvert.SerializeObject(ticketAPI, Formatting.Indented);

            var apiResponse = await _ticketAPIRepository.InsertTicketAPIAsync(json);

            responseServiceModel.IsValid = apiResponse.IsSuccessStatusCode;

            if (apiResponse.IsSuccessStatusCode)
            {
                responseServiceModel.Value = apiResponse.Response ?? string.Empty;
            }
            else
            {
                string additionalMessage = apiResponse.AdditionalMessage ?? string.Empty;

                if (additionalMessage.Contains("chamado_protocolo_key"))
                {
                    responseServiceModel.Value = "Duplicidade: Já existe um Chamado aberto com este Protocolo.";
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

    public async Task<string> InsertTicketTestAPIAsync(TicketCreateAPI ticketAPI)
    {
        string message = string.Empty;
        try
        {
            string json = JsonConvert.SerializeObject(ticketAPI, Formatting.Indented);

            var apiResponse = await _ticketAPIRepository.InsertTicketTestAPIAsync(json);

            if (apiResponse.IsSuccessStatusCode)
            {
                return json;
            }
        }
        catch (Exception ex)
        {
            message = $"Exception: {ex.Message}. Trace: {ex.StackTrace}";
            Console.WriteLine($"Exception: {ex.Message}. Trace: {ex.StackTrace}");
        }

        return message;
    }

    public async Task<bool> UpdateTicketAPIAsync(TicketAPI ticketAPI)
    {
        try
        {
            dynamic obj = new ExpandoObject();
            obj.idChamado = ticketAPI.Id;
            obj.idFilaGT = ticketAPI.QueueGTId;
            obj.observacaoDemanda = ticketAPI.DemandObservation;
            //obj.informacaoDemanda = ticketAPI.DemandInformation;
            obj.idStatusChamado = ticketAPI.TicketStatusId;
            obj.idCriticidadeChamado = ticketAPI.TicketCriticalityId;
            obj.solucionado = ticketAPI.AnySolution == "0" ? false : true;
            obj.idMotivoDemanda = ticketAPI.DemandTypeId;

            string json = JsonConvert.SerializeObject(obj, Formatting.Indented);
            var apiResponse = await _ticketAPIRepository.UpdateTicketAPIAsync(json);

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

    public async Task<List<TicketAPI>> SearchTicketAPIAsync(string filter, string value)
    {
        try
        {
            dynamic obj = new ExpandoObject();
            obj.chave = filter;
            obj.valor = value;

            var json = JsonConvert.SerializeObject(obj);
            var apiResponse = await _ticketAPIRepository.SearchDataAsync(json);

            if (apiResponse != null)
            {
                if (apiResponse.IsSuccessStatusCode)
                {
                    if (apiResponse.Response != null)
                    {
                        List<TicketAPI>? tickets = apiResponse.Response as List<TicketAPI>;
                        if (tickets != null)
                        {
                            for (int i = 0; i < tickets.Count; i++)
                            {
                                TicketAPI ticket = tickets[i];
                                var customerId = ticket.CustomerId;

                                if (customerId != null)
                                {
                                    var customer = await _customerService.GetCustomerAPIAsync(Int64.Parse(customerId));

                                    if (customer != null)
                                    {
                                        tickets[i].CustomerName = customer.FirstName;
                                    }
                                }
                            }
                            return tickets;
                        }
                    }

                }

            }

        }
        catch (Exception ex)
        {
            Console.WriteLine($"Exception: {ex.Message}. Trace: {ex.StackTrace}");
        }

        return new List<TicketAPI>();
    }

    public async Task<List<TicketStatusAPI>> GetTicketStatusAPIAsync()
    {
        try
        {
            var apiResponse = await _ticketAPIRepository.GetTicketStatusAPIAsync();

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

        return new List<TicketStatusAPI>();
    }

    public async Task<List<TicketCriticalityAPI>> GetTicketCriticalityAPIAsync()
    {
        try
        {
            var apiResponse = await _ticketAPIRepository.GetTicketCriticalityAPIAsync();

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

        return new List<TicketCriticalityAPI>();
    }

    public async Task<int> InsertTicketClassificationAPIAsync(TicketClassificationCreateAPI ticketClassificationCreateAPI)
    {
        try
        {
            ticketClassificationCreateAPI.Order = 1;
            string json = JsonConvert.SerializeObject(ticketClassificationCreateAPI, Formatting.Indented);

            var apiResponse = await _ticketAPIRepository.InsertTicketClassificationAPIAsync(json);

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

    public async Task<List<TicketQueueGTAPI>> GetTicketQueueGTAPIAsync()
    {
        try
        {
            var apiResponse = await _ticketAPIRepository.GetTicketQueueGTAPIAsync();

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

        return new List<TicketQueueGTAPI>();
    }

    public async Task<List<TicketAPI>> GetCustomerTicketAPIAsync(Int64 id)
    {
        try
        {
            var apiResponse = await _ticketAPIRepository.GetCustomerTicketAPIAsync(id);

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

        return new List<TicketAPI>();
    }

    public async Task<bool> TicketHasSavedAPIAsync(string protocol)
    {
        try
        {
            var apiResponse = await _ticketAPIRepository.TicketHasSavedAPIAsync(protocol);

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

    public async Task<TicketAPI> GetTicketAPIAsync(string protocol)
    {
        try
        {
            var apiResponse = await _ticketAPIRepository.GetTicketAPIAsync(protocol);

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

        return new TicketAPI();
    }

    public async Task<bool> UploadTicketAttachmentAPIAsync(TicketAttachmentCreateAPI ticketAttachment)
    {
        try
        {
            var apiResponse = await _ticketAPIRepository.UploadTicketAttachmentAPIAsync(ticketAttachment);

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
    
    public async Task<List<TicketAttachmentAPI>> DownloadTicketAttachmentAPIAsync(Int64 id)
    {
        try
        {
            var apiResponse = await _ticketAPIRepository.DownloadTicketAttachmentAPIAsync(id);

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

        return new List<TicketAttachmentAPI>();
    }
    public async Task<List<TicketAPI>> GetTicketListGCAPIAsync(string userid)
    {
        try
        {
            var apiResponse = await _ticketAPIRepository.GetTicketListGCAPIAsync(userid);

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

        return new List<TicketAPI>();
    }

    public async Task<List<TicketAPI>> GetTicketListGCSupervisorAPIAsync(int page = 1, int sizepage = 50)
    {
        try
        {
            var apiResponse = await _ticketAPIRepository.GetTicketListGCSupervisorAPIAsync(page, sizepage);

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

        return new List<TicketAPI>();
    }

    public async Task<int> GetTicketCountAsync()
    {
        try
        {
            var apiResponse = await _ticketAPIRepository.GetDataAsync();

            if (apiResponse.IsSuccessStatusCode)
            {
                return apiResponse.Response;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Exception: {ex.Message}. Trace: {ex.StackTrace}");
        }

        return new int();
    }

    public async Task<List<TicketAgentStatusAPI>> GetTicketAgentStatusAsync()
    {
        try
        {
            var apiResponse = await _ticketAPIRepository.GetTicketAgentStatusAPIAsync();

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

        return new List<TicketAgentStatusAPI>();
    }

    public async Task<List<TicketAssistentStatusAPI>> GetTicketAssistentStatusAsync()
    {
        try
        {
            var apiResponse = await _ticketAPIRepository.GetTicketAssistentStatusAPIAsync();

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

        return new List<TicketAssistentStatusAPI>();
    }
}
