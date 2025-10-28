using Alctel.CRM.API.Entities;
using Alctel.CRM.Business.Interfaces;
using Alctel.CRM.Business.Services;
using Alctel.CRM.Web.Models;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Alctel.CRM.Web.Controllers;

public class SlaAlertController : Controller
{
    private const string MODULE_NAME = "lista_item";
    private readonly IMapper _mapper;
    private List<SelectIOptions> selectIOptions = new List<SelectIOptions>();
    private readonly ITicketClassificationService _ticketClassificationService;
    private readonly IServiceUnitService _serviceUnitService;
    private readonly ITicketService _ticketService;
    private readonly ISlaService _slaService;

    public SlaAlertController(IMapper mapper, ITicketClassificationService ticketClassificationService, IServiceUnitService serviceUnitService, ITicketService ticketService, ISlaService slaService)
    {
        _ticketClassificationService = ticketClassificationService;
        _mapper = mapper;
        _serviceUnitService = serviceUnitService;
        _ticketService = ticketService;
        _slaService = slaService;
    }

    public async Task<IActionResult> Index()
    {
        try
        {
            var list = await _slaService.GetSlaTicketConfigAsync();

            if (list != null)
            {
                var model = _mapper.Map<List<SlaTicketConfigModel>>(list);
                return View(model);
            }
        }
        catch (Exception ex)
        {
        }

        return View();
    }

    [HttpGet]
    public async Task<IActionResult> Create()
    {
        TicketClassificationModel model = new TicketClassificationModel();
        await LoadManifestationTypeOptions(model);
        //await LoadServiceUnitOptions(model);
        //await LoadServiceOptions(model);
        //await LoadProgramOptions(model);
        //await LoadReason01Options(model);
        //await LoadReason02Options(model);
        //await LoadListOptionsAsync(model);
        await LoadCriticityOptionsAsync(model);
        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> Create(TicketClassificationModel model)
    {
        try
        {
            SlaTicketCreateModel slaTicketCreateModel = new SlaTicketCreateModel();
            slaTicketCreateModel.ManifestationTypeId = model.ManifestationTypeId;
            slaTicketCreateModel.ServiceId = model.ServiceId;
            slaTicketCreateModel.CriticalityId = model.CriticalityId;
            slaTicketCreateModel.Sla = model.SlaInDays;
            slaTicketCreateModel.Alarm = model.Alarm;

            if (model.Reason01Id != 0)
            {
                slaTicketCreateModel.SlaReasons.Add(new SlaReasonModel { ReasonId = model.Reason01Id });
            }

            if (model.Reason02Id != 0)
            {
                slaTicketCreateModel.SlaReasons.Add(new SlaReasonModel { ReasonId = model.Reason02Id });
            }

            var data = _mapper.Map<SlaTicketCreateAPI>(slaTicketCreateModel);

            var ret = await _slaService.InsertSlaTicketConfigAsync(data);

            if (ret.IsValid)
            {
                ViewBag.MessageInfo = "Sla criado com sucesso.";
            }
            else
            {
                ViewBag.MessageInfo = ret.Value;
            }
        }
        catch (Exception ex)
        { }

        await LoadManifestationTypeOptions(model);
        //await LoadServiceUnitOptions(model);
        //await LoadServiceOptions(model);
        //await LoadProgramOptions(model);
        //await LoadReason01Options(model);
        //await LoadReason02Options(model);
        //await LoadListOptionsAsync(model);
        await LoadCriticityOptionsAsync(model);

        return View(model);
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        try
        {
            var data = await _slaService.GetSlaTicketConfigAsync(id);

            if (data != null)
            {
                TicketClassificationModel ticketClassificationModel = new TicketClassificationModel();
                ticketClassificationModel.SlaTicketId = data.SlaTicketId;
                ticketClassificationModel.ManifestationTypeName = data.ManifestationTypeName;
                ticketClassificationModel.ServiceName = data.ServiceName;
                ticketClassificationModel.CriticalityIdName = data.Criticality;
                ticketClassificationModel.CriticalityId = string.IsNullOrEmpty(data.CriticalityId) ? 0 : Int64.Parse(data.CriticalityId);
                ticketClassificationModel.SlaInDays = string.IsNullOrEmpty(data.Sla) ? 0 : int.Parse(data.Sla);
                ticketClassificationModel.Alarm = string.IsNullOrEmpty(data.Alarm) ? 0 : int.Parse(data.Alarm);
                ticketClassificationModel.Reason01Name = data.Reasons;

                //await LoadManifestationTypeOptions(ticketClassificationModel);
                await LoadCriticityOptionsAsync(ticketClassificationModel);

                return View(ticketClassificationModel);
            }
        }
        catch (Exception ex)
        { }
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Edit([FromForm] TicketClassificationModel model)
    {
        try
        {
            var data = new SlaTicketConfigAPI();
            data.SlaTicketId = model.SlaTicketId;
            data.CriticalityId = model.CriticalityId.ToString();
            data.Sla = model.SlaInDays.ToString();
            data.Alarm = model.Alarm.ToString();

            int ret = await _slaService.UpdateSlaTicketConfigAsync(data);

            if (ret > 0)
            {
                ViewBag.MessageInfo = "SLA Atualizado com Sucesso.";
            }
            else
            {
                ViewBag.MessageInfo = "Erro Atualizando SLA";
            }
        }
        catch (Exception ex)
        { }

        await LoadCriticityOptionsAsync(model);
        return View(model);
    }

    [HttpGet]
    public async Task<IActionResult> AgendaIndex()
    {
        try
        {
            var slareturn = await _slaService.GetSlaCalendarAsync();

            if (slareturn != null)
            {
                var model = _mapper.Map<List<SlaAlertAgendaModel>>(slareturn);
                return View(model);
            }
        }
        catch (Exception ex)
        { }
        return View();
    }

    [HttpGet]
    public IActionResult AgendaCreate()
    {

        return View();
    }

    [HttpPost]
    public async Task<IActionResult> AgendaCreate([FromForm] SlaAlertAgendaModel model)
    {
        try
        {
            var userid = HttpContext.Session.GetString("UserId");

            if (userid != null)
            {
                model.UserId = Int64.Parse(userid);
            }

            var slamodel = _mapper.Map<SlaAlertAgendaAPI>(model);
            var ret = await _slaService.InsertSlaCalendarAsync(slamodel);

            if (ret > 0)
            {
                ViewBag.MessageInfo = "Calendário criado com sucesso!";
            }
            else
            {
                ViewBag.MessageInfo = "Erro: criando calendário";
            }
        }
        catch (Exception ex)
        { }
        return View(model);
    }

    private async Task LoadManifestationTypeOptions(TicketClassificationModel model, Int64 selectedItem = 0)
    {
        try
        {
            bool isSelected = false;

            model.ManifestationTypeOptions.Add(new SelectListItem
            {
                Value = "0",
                Text = "Opções",
                Selected = isSelected
            });

            var list = await _ticketClassificationService.GetTicketClassificationManifestationTypeAPIAsync();

            if (list != null)
            {
                foreach (var item in list)
                {
                    isSelected = false;
                    if (item.Id == selectedItem)
                    {
                        isSelected = true;
                    }

                    model.ManifestationTypeOptions.Add(new SelectListItem
                    {
                        Value = item.Id.ToString(),
                        Text = item.Name,
                        Selected = isSelected
                    });
                }
            }
        }
        catch (Exception ex)
        {
        }
    }

    private async Task LoadServiceUnitOptions(TicketClassificationModel model, Int64 selectedItem = 0)
    {
        try
        {
            bool isSelected = false;

            var list = await _serviceUnitService.GetServiceUnitActivatedListAPIAsync();

            if (list != null)
            {
                var serviceUnitList = _mapper.Map<List<ServiceUnitModel>>(list);

                model.ServiceUnitOptions.Add(new SelectListItem
                {
                    Value = "0",
                    Text = "Opções",
                    Selected = isSelected
                });

                foreach (var item in serviceUnitList)
                {
                    isSelected = false;
                    if (item.Id == selectedItem)
                    {
                        isSelected = true;
                    }

                    model.ServiceUnitOptions.Add(new SelectListItem
                    {
                        Value = item.Id.ToString(),
                        Text = item.Name,
                        Selected = isSelected
                    });
                }
            }
        }
        catch (Exception ex)
        {
        }
    }

    private async Task LoadServiceOptions(TicketClassificationModel model, Int64 selectedItem = 0)
    {
        try
        {
            bool isSelected = false;

            var list = await _ticketClassificationService.GetTicketClassificationServiceAsync();

            if (list != null)
            {
                var modellist = _mapper.Map<List<TicketClassificationServiceModel>>(list);

                model.ServiceOptions.Add(new SelectListItem
                {
                    Value = "0",
                    Text = "Opções",
                    Selected = isSelected
                });

                foreach (var item in modellist)
                {
                    isSelected = false;
                    if (item.Id == selectedItem)
                    {
                        isSelected = true;
                    }

                    model.ServiceOptions.Add(new SelectListItem
                    {
                        Value = item.Id.ToString(),
                        Text = item.Name,
                        Selected = isSelected
                    });
                }
            }
        }
        catch (Exception ex)
        {
        }
    }

    private async Task LoadProgramOptions(TicketClassificationModel model, Int64 selectedItem = 0)
    {
        try
        {
            bool isSelected = false;

            var list = await _ticketClassificationService.GetTicketClassificationProgramAsync();

            if (list != null)
            {
                var listModel = _mapper.Map<List<TicketClassificationProgramModel>>(list);

                model.ProgramOptions.Add(new SelectListItem
                {
                    Value = "0",
                    Text = "Opções",
                    Selected = isSelected
                });

                foreach (var item in listModel)
                {
                    isSelected = false;
                    if (item.Id == selectedItem)
                    {
                        isSelected = true;
                    }

                    model.ProgramOptions.Add(new SelectListItem
                    {
                        Value = item.Id.ToString(),
                        Text = item.Name,
                        Selected = isSelected
                    });
                }
            }
        }
        catch (Exception ex)
        {
        }
    }

    private async Task LoadListOptionsAsync(TicketClassificationModel model, Int64 selectedItem = 0)
    {
        try
        {
            bool isSelected = false;

            var list = await _ticketClassificationService.GetTicketClassificationListAsync();

            if (list != null)
            {
                var listModel = _mapper.Map<List<TicketClassificationListModel>>(list);

                model.Reason01Options.Add(new SelectListItem
                {
                    Value = "0",
                    Text = "Opções",
                    Selected = isSelected
                });

                model.Reason02Options.Add(new SelectListItem
                {
                    Value = "0",
                    Text = "Opções",
                    Selected = isSelected
                });

                foreach (var item in listModel)
                {
                    isSelected = false;
                    if (item.Id == selectedItem)
                    {
                        isSelected = true;
                    }

                    model.Reason01Options.Add(new SelectListItem
                    {
                        Value = item.Id.ToString(),
                        Text = item.Name,
                        Selected = isSelected
                    });

                    model.Reason02Options.Add(new SelectListItem
                    {
                        Value = item.Id.ToString(),
                        Text = item.Name,
                        Selected = isSelected
                    });
                }
            }
        }
        catch (Exception ex)
        {
        }
    }

    private async Task LoadReason01Options(TicketClassificationModel model, Int64 selectedItem = 0)
    {
        try
        {
            bool isSelected = false;

            var list = await _ticketClassificationService.GetTicketClassificationReasonListAsync();

            if (list != null)
            {
                var listModel = _mapper.Map<List<TicketClassificationReasonListModel>>(list);

                model.Reason01Options.Add(new SelectListItem
                {
                    Value = "0",
                    Text = "Opções",
                    Selected = isSelected
                });

                foreach (var item in listModel)
                {
                    isSelected = false;
                    if (item.Id == selectedItem)
                    {
                        isSelected = true;
                    }

                    model.Reason01Options.Add(new SelectListItem
                    {
                        Value = item.Id.ToString(),
                        Text = item.Name,
                        Selected = isSelected
                    });
                }
            }
        }
        catch (Exception ex)
        {
        }
    }

    private async Task LoadCriticityOptionsAsync(TicketClassificationModel model, Int64 selectedItem = 0)
    {
        try
        {
            try
            {
                bool isSelected = false;

                var list = await _ticketService.GetTicketCriticalityAPIAsync();

                if (list != null)
                {
                    var listModel = _mapper.Map<List<TicketCriticalityModel>>(list);

                    model.CriticalityOptions.Add(new SelectListItem
                    {
                        Value = "0",
                        Text = "Opções",
                        Selected = isSelected
                    });

                    foreach (var item in listModel)
                    {
                        isSelected = false;
                        if (item.Id == selectedItem)
                        {
                            isSelected = true;
                        }

                        model.CriticalityOptions.Add(new SelectListItem
                        {
                            Value = item.Id.ToString(),
                            Text = item.Name,
                            Selected = isSelected
                        });
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }
        catch (Exception ex)
        { }
    }
}
