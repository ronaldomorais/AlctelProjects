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

    public SlaAlertController(IMapper mapper, ITicketClassificationService ticketClassificationService, IServiceUnitService serviceUnitService, ITicketService ticketService)
    {
        _ticketClassificationService = ticketClassificationService;
        _mapper = mapper;
        _serviceUnitService = serviceUnitService;
        _ticketService = ticketService;
    }

    public IActionResult Index()
    {
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
        await LoadCriticityOptionsAsync(model);
        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> Create(TicketClassificationModel model)
    {
        return View(model);
    }

    [HttpGet]
    public IActionResult AgendaIndex()
    {
        return View();
    }

    [HttpGet]
    public IActionResult AgendaCreate()
    {
        return View();
    }

    [HttpPost]
    public IActionResult AgendaCreate([FromForm] SlaAlertAgendaModel model)
    {
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
