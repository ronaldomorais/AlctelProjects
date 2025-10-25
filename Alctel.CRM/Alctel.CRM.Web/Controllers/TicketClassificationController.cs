using System.Collections.Generic;
using System.Threading.Tasks;
using Alctel.CRM.API.Entities;
using Alctel.CRM.Business.Interfaces;
using Alctel.CRM.Business.Services;
using Alctel.CRM.Context.InMemory.Entities;
using Alctel.CRM.Web.Models;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Alctel.CRM.Web.Controllers;

class SelectIOptions
{
    public Int64 Id { get; set; }
    public string? Name { get; set; }
}

public class TicketClassificationController : Controller
{
    private const string MODULE_NAME = "lista_item";
    private readonly IMapper _mapper;
    private List<SelectIOptions> selectIOptions = new List<SelectIOptions>();
    private readonly ILoginService _loginService;
    private readonly ITicketClassificationService _ticketClassificationService;
    private readonly ILogControllerService _logControllerService;
    private readonly IServiceUnitService _serviceUnitService;

    public TicketClassificationController(IMapper mapper, ILoginService loginService, ITicketClassificationService ticketClassificationService, ILogControllerService logControllerService, IServiceUnitService serviceUnitService)
    {
        selectIOptions.Add(new SelectIOptions
        {
            Id = 1,
            Name = "Opção 01"
        });

        selectIOptions.Add(new SelectIOptions
        {
            Id = 2,
            Name = "Opção 02"
        });

        selectIOptions.Add(new SelectIOptions
        {
            Id = 3,
            Name = "Opção 03"
        });

        _loginService = loginService;
        _ticketClassificationService = ticketClassificationService;
        _mapper = mapper;
        _logControllerService = logControllerService;
        _serviceUnitService = serviceUnitService;
    }

    //[HttpGet]
    //public async Task<IActionResult> ConfigurationListIndex()
    //{
    //    try
    //    {
    //        if (IsAuthenticated() == false)
    //        {
    //            return RedirectToAction("Create", "Login");
    //        }

    //        TicketClassificationConfigListModel model = new TicketClassificationConfigListModel();
    //        await LoadClassificationListOptions(model);

    //        if (TempData["ScreenMessage"] != null)
    //        {
    //            ViewBag.ScreenMessage = TempData["ScreenMessage"];
    //            TempData["ScreenMessage"] = null;
    //        }

    //        return View(model);
    //    }
    //    catch (Exception ex)
    //    { }

    //    return View();
    //}

    //[HttpGet]
    //public async Task<IActionResult> TicketClassificationListItemsIndex(Int64 id)
    //{
    //    try
    //    {
    //        if (IsAuthenticated() == false)
    //        {
    //            return RedirectToAction("Create", "Login");
    //        }

    //        var listitem = await _ticketClassificationService.GetTicketClassificationListItemsAsync(id);

    //        if (listitem != null)
    //        {
    //            ViewBag.ListId = id.ToString();
    //            var model = _mapper.Map<List<TicketClassificationListItemsModel>>(listitem);
    //            return View(model);
    //        }
    //    }
    //    catch (Exception ex)
    //    { }

    //    return View();
    //}

    //[HttpPost]
    //public async Task<IActionResult> CreateList(string listNameId)
    //{
    //    try
    //    {
    //        int ret = await _ticketClassificationService.InsertTicketClassificationListAsync(listNameId);

    //        if (ret > 0)
    //        {
    //            var username = HttpContext.Session.GetString("Username");
    //            var userid = HttpContext.Session.GetString("UserId");

    //            LogController logController = new LogController();
    //            logController.Id = ret;
    //            logController.Module = MODULE_NAME;
    //            logController.Section = username == null ? string.Empty : username;
    //            logController.Field = "Todos";
    //            logController.Value = "Todos";
    //            logController.UserId = userid != null ? Int64.Parse(userid) : 0;
    //            logController.Action = "Criar";

    //            await _logControllerService.InsertLogAPIAsync(logController);

    //            TempData["ScreenMessage"] = $"Lista {listNameId} criada com sucesso.";
    //        }
    //        else
    //        {
    //            TempData["ScreenMessage"] = $"ERRO: criando lista {listNameId}.";
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        TempData["ScreenMessage"] = $"ERRO: criando item {listNameId}.";
    //    }

    //    return RedirectToAction("ConfigurationListIndex");
    //}

    //[HttpPost]
    //public async Task<IActionResult> CreateListItem(Int64 listId, string listItemName)
    //{
    //    try
    //    {
    //        int ret = await _ticketClassificationService.InsertTicketClassificationListitemAsync(listId, listItemName);

    //        if (ret > 0)
    //        {
    //            var username = HttpContext.Session.GetString("Username");
    //            var userid = HttpContext.Session.GetString("UserId");

    //            LogController logController = new LogController();
    //            logController.Id = ret;
    //            logController.Module = MODULE_NAME;
    //            logController.Section = username == null ? string.Empty : username;
    //            logController.Field = "Todos";
    //            logController.Value = "Todos";
    //            logController.UserId = userid != null ? Int64.Parse(userid) : 0;
    //            logController.Action = "Criar";

    //            await _logControllerService.InsertLogAPIAsync(logController);

    //            TempData["ScreenMessage"] = $"Item {listItemName} criado com sucesso.";
    //        }
    //        else
    //        {
    //            TempData["ScreenMessage"] = $"ERRO: criando item {listItemName}.";
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        TempData["ScreenMessage"] = $"ERRO: criando item {listItemName}.";
    //    }

    //    return RedirectToAction("ConfigurationListIndex");
    //}


    //[HttpGet]
    //public async Task<IActionResult> EditListItem(int id)
    //{
    //    if (IsAuthenticated() == false)
    //    {
    //        return RedirectToAction("Create", "Login");
    //    }

    //    var item = await _ticketClassificationService.GetTicketClassificationListItemAsync(id);

    //    if (item != null)
    //    {
    //        var model = _mapper.Map<TicketClassificationListItemModel>(item);

    //        model.ListItemDataToCompareIfChanged = new ListItemDataToCompareIfChangedLog();
    //        model.ListItemDataToCompareIfChanged.Id = model.Id;
    //        model.ListItemDataToCompareIfChanged.Active = model.Active;
    //        return View(model);
    //    }

    //    return View();
    //}

    //[HttpPost]
    //public async Task<IActionResult> EditListItem([FromForm] TicketClassificationListItemModel model)
    //{
    //    try
    //    {
    //        if (IsAuthenticated() == false)
    //        {
    //            return RedirectToAction("Create", "Login");
    //        }

    //        var ret = await _ticketClassificationService.UpdateTicketClassificationListItemAsync(model.ListItemId, model.Active);

    //        if (ret > 0)
    //        {
    //            var username = HttpContext.Session.GetString("Username");
    //            var userid = HttpContext.Session.GetString("UserId");

    //            if (model.Active != model.ListItemDataToCompareIfChanged.Active)
    //            {
    //                LogController logController = new LogController();
    //                logController.Id = model.ListItemId;
    //                logController.Module = MODULE_NAME;
    //                logController.Section = username == null ? string.Empty : username;
    //                logController.Field = "Ativar/Desativar";
    //                logController.Value = model.ListItemDataToCompareIfChanged.Active ? "Ativado" : "Desativado";
    //                logController.UserId = userid != null ? Int64.Parse(userid) : 0;
    //                logController.Action = "Editar";

    //                await _logControllerService.InsertLogAPIAsync(logController);

    //                TempData["ScreenMessage"] = $"Item atualizado com sucesso.";
    //            }
    //            else
    //            {
    //                TempData["ScreenMessage"] = $"ERRO: atualizado item.";
    //            }
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        TempData["ScreenMessage"] = $"ERRO: atualizado item.";
    //    }

    //    //return RedirectToAction("EditListItem", new { id = model.ListItemId });
    //    return RedirectToAction("ConfigurationListIndex");

    //}

    //[HttpGet]
    //public async Task<IActionResult> ConfigurationClassificationIndex()
    //{
    //    try
    //    {

    //    }
    //    catch (Exception ex)
    //    {
    //    }

    //    return View();
    //}

    //[HttpGet]
    //public async Task<IActionResult> TicketClassificationCreate()
    //{
    //    try
    //    {

    //    }
    //    catch (Exception ex)
    //    {
    //    }

    //    return View();
    //}

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        try
        {
            TicketClassificationModel model = new TicketClassificationModel();
            await LoadManifestationTypeOptions(model);
            //await LoadServiceUnitOptions(model);
            //await LoadProgramOptions(model);

            return View(model);
        }
        catch (Exception ex)
        {
        }

        return View();
    }

    [HttpGet]
    public async Task<IActionResult> Create()
    {
        try
        {
            TicketClassificationModel model = new TicketClassificationModel();
            await LoadManifestationTypeOptions(model);
            await LoadServiceUnitOptions(model);
            await LoadProgramOptions(model);
            //await LoadReason01Options(model);
            //await LoadReason02Options(model);
            await LoadListOptionsAsync(model);
            return View(model);
        }
        catch (Exception ex)
        {
        }

        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromForm] TicketClassificationModel model)
    {
        try
        {
            TicketClassificationReasonCreateModel classificationReasonModel = new TicketClassificationReasonCreateModel();
            classificationReasonModel.ServiceName = model.ServiceName;
            classificationReasonModel.ProgramId = model.ProgramId;
            classificationReasonModel.ManifestationTypeId = model.ManifestationTypeId;

            classificationReasonModel.ticketReason.Add(new TicketReasonCreateModel
            {
                Status = true,
                ParentId = null,
                //ListId = model.Reason01ListId,
                //ReasonName = model.Reason01ListName
                ListId = model.Reason01Id,
                ReasonName = model.Reason01Name
            });

            if (model.Reason02Id != 0)
            {
                classificationReasonModel.ticketReason.Add(new TicketReasonCreateModel
                {
                    Status = true,
                    ParentId = null,
                    //ListId = model.Reason02ListId,
                    //ReasonName = model.Reason02ListName
                    ListId = model.Reason02Id,
                    ReasonName = model.Reason02Name
                });
            }

            var ticketClassificationReasonAPI = _mapper.Map<TicketClassificationReasonCreateAPI>(classificationReasonModel);

            var ret = await _ticketClassificationService.InsertTicketClassificationReasonAsync(ticketClassificationReasonAPI);

            if (ret.IsValid)
            {
                ViewBag.MessageInfo = "Serviço Criado Com Sucesso!";
            }
            else
            {
                ViewBag.MessageInfo = $"Erro: {ret.Value}";
            }
        }
        catch (Exception ex)
        {
        }

        await LoadManifestationTypeOptions(model);
        await LoadServiceUnitOptions(model);
        await LoadProgramOptions(model);
        //await LoadReason01Options(model);
        //await LoadReason02Options(model);
        await LoadListOptionsAsync(model);

        return View(model);
    }

    [HttpGet]
    public async Task<IActionResult> ScreenPopup()
    {
        TicketClassificationModel model = new TicketClassificationModel();
        await LoadManifestationTypeOptions(model);
        await LoadServiceUnitOptions(model);
        //LoadServiceOptions(model);
        //await LoadReason01Options(model);
        //await LoadReason02Options(model);
        return View(model);
    }

    private async Task LoadClassificationListOptions(TicketClassificationConfigListModel model, Int64 selectedItem = 0)
    {
        try
        {
            bool isSelected = false;

            model.ListOptions.Add(new SelectListItem
            {
                Value = "0",
                Text = "Opções",
                Selected = isSelected
            });

            var list = await _ticketClassificationService.GetTicketClassificationListAsync();

            if (list != null)
            {
                foreach (var item in list)
                {
                    isSelected = false;
                    if (item.Id == selectedItem)
                    {
                        isSelected = true;
                    }

                    model.ListOptions.Add(new SelectListItem
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

            //var list = await _serviceUnitService.GetServiceUnitActivatedListAPIAsync();
            var list = await _ticketClassificationService.GetTicketClassificationUnitListAsync("Unidades de Atendimento");

            if (list != null)
            {
                //var serviceUnitList = _mapper.Map<List<ServiceUnitModel>>(list);
                var serviceUnitList = _mapper.Map<List<TicketClassificationUnitModel>>(list);

                model.ServiceUnitOptions.Add(new SelectListItem
                {
                    Value = "0",
                    Text = "Opções",
                    Selected = isSelected
                });

                foreach (var item in serviceUnitList)
                {
                    isSelected = false;
                    if (item.ListItemId == selectedItem)
                    {
                        isSelected = true;
                    }

                    model.ServiceUnitOptions.Add(new SelectListItem
                    {
                        Value = item.ListItemId.ToString(),
                        Text = item.ListItemName,
                        Selected = isSelected
                    });
                }
            }
        }
        catch (Exception ex)
        {
        }
    }

    private void LoadServiceOptions(TicketClassificationModel model, Int64 selectedItem = 0)
    {
        bool isSelected = false;

        model.ServiceOptions.Add(new SelectListItem
        {
            Value = "0",
            Text = "Opções",
            Selected = isSelected
        });

        foreach (var item in selectIOptions)
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

    //private async Task LoadReason02Options(TicketClassificationModel model, Int64 selectedItem = 0)
    //{
    //    try
    //    {
    //        bool isSelected = false;

    //        var list = await _ticketClassificationService.GetTicketClassificationReasonSonListAsync(3);

    //        if (list != null)
    //        {
    //            var listModel = _mapper.Map<List<TicketClassificationReasonListModel>>(list);

    //            model.Reason02Options.Add(new SelectListItem
    //            {
    //                Value = "0",
    //                Text = "Opções",
    //                Selected = isSelected
    //            });

    //            foreach (var item in listModel)
    //            {
    //                isSelected = false;
    //                if (item.Id == selectedItem)
    //                {
    //                    isSelected = true;
    //                }

    //                model.Reason02Options.Add(new SelectListItem
    //                {
    //                    Value = item.Id.ToString(),
    //                    Text = item.Name,
    //                    Selected = isSelected
    //                });
    //            }
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //    }
    //}

    public async Task<JsonResult> GetTicketClassificationServiceByManifestation(Int64 id)
    {
        try
        {
            var list = await _ticketClassificationService.GetTicketClassificationServiceByManifestationAsync(id);

            if (list != null)
            {
                return new JsonResult(list);
            }
        }
        catch (Exception ex)
        { }

        return new JsonResult(null);
    }

    public async Task<JsonResult> GetTicketClassificationProgramByService(Int64 id)
    {
        try
        {
            var list = await _ticketClassificationService.GetTicketClassificationProgramByServiceAsync(id);

            if (list != null)
            {
                return new JsonResult(list);
            }
        }
        catch (Exception ex)
        { }

        return new JsonResult(null);
    }

    public async Task<JsonResult> GetTicketClassificationReasonByManifestationService(Int64 manifestationid, Int64 serviceid)
    {
        try
        {
            var list = await _ticketClassificationService.GetTicketClassificationReasonByManifestationServiceAsync(manifestationid, serviceid);

            if (list != null)
            {
                return new JsonResult(list);
            }
        }
        catch (Exception ex)
        { }

        return new JsonResult(null);
    }

    public async Task<JsonResult> GetTicketClassificationReasonListItems(Int64 manifestationid, Int64 serviceid, Int64? parentId)
    {
        try
        {
            var list = await _ticketClassificationService.GetTicketClassificationReasonListItemsAsync(manifestationid, serviceid, parentId);

            if (list != null)
            {
                return new JsonResult(list);
            }
        }
        catch (Exception ex)
        { }

        return new JsonResult(null);
    }

    public async Task<JsonResult> GetTicketClassificationReasonSonList(Int64 id)
    {
        try
        {
            var list = await _ticketClassificationService.GetTicketClassificationReasonSonListAsync(id);

            if (list != null)
            {
                return new JsonResult(list);
            }
        }
        catch (Exception ex)
        { }

        return new JsonResult(null);
    }

    public async Task<IActionResult> TicketClassificationByManifestationIndex(Int64 id)
    {
        try
        {
            var list = await _ticketClassificationService.GetTicketClassificationByManifestationAsync(id);

            if (list != null)
            {
                var model = _mapper.Map<List<TicketClassficationListModel>>(list);
                return View(model);
            }
        }
        catch (Exception ex)
        { }

        return View();
    }

    private bool IsAuthenticated()
    {
        bool userAuthenticated = false;
        try
        {
            string? isAuthenticated = HttpContext.Session.GetString("isAuthenticated");

            if (isAuthenticated != null && isAuthenticated == "true")
            {
                string? loginUser = HttpContext.Session.GetString("LoginUser");

                if (loginUser != null)
                {
                    string? profilesession = HttpContext.Session.GetString("Profile");

                    if (profilesession != null)
                    {
                        var logininfo = _loginService.GetLoginPIAsync(loginUser).Result;

                        if (logininfo != null)
                        {
                            string profile = logininfo.Profile != null ? logininfo.Profile : string.Empty;
                            if (profilesession != profile)
                            {
                                HttpContext.Session.Clear();
                                return false;
                            }
                        }
                    }
                }

                if (ControllerContext.RouteData.Values["controller"] != null)
                {
                    string? modules = HttpContext.Session.GetString("Module");
                    string? controllerName = ControllerContext.RouteData.Values["controller"]!.ToString();


                    if (modules != null && modules.ToUpper().Contains(controllerName != null ? controllerName.ToUpper() : string.Empty))
                    {
                        userAuthenticated = true;
                    }
                }
            }
        }
        catch (Exception ex)
        {
        }

        return userAuthenticated;
    }
}
