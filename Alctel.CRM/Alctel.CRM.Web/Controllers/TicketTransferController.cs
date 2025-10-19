using Alctel.CRM.API.Entities;
using Alctel.CRM.Business.Interfaces;
using Alctel.CRM.Business.Model;
using Alctel.CRM.Business.Services;
using Alctel.CRM.Web.Models;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Hosting.Internal;

namespace Alctel.CRM.Web.Controllers;

public class TicketTransferController : Controller
{
    private readonly ITicketService _ticketService;
    private readonly ITicketTransferService _ticketTransferService;
    private readonly IMapper _mapper;
    private const string MODULE_NAME = "chamados";
    private readonly ILogControllerService _logControllerService;
    private readonly IWebHostEnvironment _hostingEnvironment;
    private readonly IConfigService _configService;

    public TicketTransferController(ITicketService ticketService, IMapper mapper, ITicketTransferService ticketTransferService, ILogControllerService logControllerService, IWebHostEnvironment hostingEnvironment, IConfigService configService)
    {
        _ticketService = ticketService;
        _ticketTransferService = ticketTransferService;
        _mapper = mapper;
        _logControllerService = logControllerService;
        _hostingEnvironment = hostingEnvironment;
        _configService = configService;
    }

    public async Task<IActionResult> Create(string ticketid, string protocol, string userid, string queuegt = "")
    {
        var model = new TicketTransferModel();

        string physicalPath = _hostingEnvironment.WebRootPath;
        ViewBag.BaseUrl = _configService.GetBaseUrl(physicalPath);

        try
        {
            model.TicketId = Int64.Parse(ticketid);
            model.Protocol = protocol;
            model.UserId = Int64.Parse(userid);
            model.QueueGT = queuegt;

            var queueGTList = await _ticketService.GetTicketQueueGTAPIAsync();

            if (queueGTList != null)
            {
                var ticketQueueGTModel = _mapper.Map<List<TicketQueueGTModel>>(queueGTList);

                model.TransferQueueGTOptions.Add(new SelectListItem
                {
                    Value = "0",
                    Text = "Opções",
                });

                foreach (var ticketQueueGT in ticketQueueGTModel)
                {
                    var selectListItem = new SelectListItem
                    {
                        Value = ticketQueueGT.Id.ToString(),
                        Text = ticketQueueGT.Name,
                    };
                    model.TransferQueueGTOptions.Add(selectListItem);
                }
            }

            return PartialView("~/Views/Ticket/_TicketTransfer.cshtml", model);
        }
        catch (Exception ex)
        {
        }

        return PartialView("~/Views/Ticket/_TicketTransfer.cshtml", model);
    }

    public async Task<JsonResult> SendTransfer([FromForm] TicketTransferModel model)
    {
        try
        {
            Int64 transferDemandTypeId = 0;
            if (model.TransferDemandTypeId != null && model.TransferDemandTypeId.Contains(":"))
            {
                var transferDemand = model.TransferDemandTypeId.Split(":");
                transferDemandTypeId = Int64.Parse(transferDemand[0]);
            }

            var ticketTransfer = new TicketTransferCreateAPI();
            ticketTransfer.idChamado = model.TicketId;
            ticketTransfer.idUsuarioOrigem = model.UserId;
            ticketTransfer.idFilaGT = model.TransferQueueGTId;
            ticketTransfer.idAreaDemanda = transferDemandTypeId == 0 ? null : transferDemandTypeId;
            ticketTransfer.idLista = model.ListId == 0 ? null : model.ListId;
            ticketTransfer.idListaItem = model.TransferServiceUnitId == 0 ? null : model.TransferServiceUnitId;
            ticketTransfer.motivo = model.TransferReason;
            ticketTransfer.formulario = model.TransferDemandObservation;

            var ret = await _ticketTransferService.InsertTicketTransferAPIAsync(ticketTransfer);

            if (ret != null && ret.IsValid)
            {
                var username = HttpContext.Session.GetString("Username");
                var useridSession = HttpContext.Session.GetString("UserId");

                //string queueGTName = string.Empty;
                //var queueGTCollection = await _ticketService.GetTicketQueueGTAPIAsync();

                //if (queueGTCollection != null && queueGTCollection.Count > 0)
                //{
                //    var queueGT = queueGTCollection.FirstOrDefault(_ => _.Id == model.TransferQueueGTId);

                //    if (queueGT != null)
                //    {
                //        queueGTName = queueGT.Name != null ? queueGT.Name : string.Empty;
                //    }
                //}

                LogController logController = new LogController();
                logController.Id = model.TicketId;
                logController.Module = MODULE_NAME;
                logController.Section = username == null ? string.Empty : username;
                logController.Field = "Queue GT";
                logController.Value = model.QueueGT ?? string.Empty;
                logController.UserId = useridSession != null ? Int64.Parse(useridSession) : 0;
                logController.Action = "Transferência";

                await _logControllerService.InsertLogAPIAsync(logController);
            }

            return new JsonResult(ret);
        }
        catch (Exception ex)
        {
        }

        
        return new JsonResult(new ResponseServiceModel { IsValid = false, Value = string.Empty});
    }

    public async Task<JsonResult> GetAreaByQueue(Int64 id)
    {
        List<TransferAreaQueueAPI> areaQueues = new List<TransferAreaQueueAPI>();
        try
        {
            var areaqueue = await _ticketTransferService.GetAreaByQueueAPIAsync(id);

            if (areaqueue != null)
            {
                areaQueues.AddRange(areaqueue);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
        }

        return new JsonResult(areaQueues);
    }

    public async Task<JsonResult> GetDemandAreaByArea(Int64 id)
    {
        List<TransferDemandAreaByAreaAPI> demandAreas = new List<TransferDemandAreaByAreaAPI>();
        try
        {
            var areaqueue = await _ticketTransferService.GetDemandAreaByAreaAsync(id);

            if (areaqueue != null)
            {
                demandAreas.AddRange(areaqueue);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
        }

        return new JsonResult(demandAreas);
    }

    public async Task<JsonResult> GetTransferForm(Int64 id)
    {
        List<TransferFormAPI> transferForm = new List<TransferFormAPI>();
        try
        {
            var areaqueue = await _ticketTransferService.GetTransferFormAsync(id);

            if (areaqueue != null)
            {
                transferForm.AddRange(areaqueue);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
        }

        return new JsonResult(transferForm);
    }
}
