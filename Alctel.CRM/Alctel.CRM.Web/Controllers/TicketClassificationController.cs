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
    private const string MODULE_NAME = "classificacao";
    private readonly IMapper _mapper;
    private List<SelectIOptions> selectIOptions = new List<SelectIOptions>();
    private readonly ILoginService _loginService;
    private readonly ITicketClassificationService _ticketClassificationService;
    private readonly ILogControllerService _logControllerService;

    public TicketClassificationController(IMapper mapper, ILoginService loginService, ITicketClassificationService ticketClassificationService, ILogControllerService logControllerService)
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
    }

    [HttpGet]
    public async Task<IActionResult> ConfigurationListIndex()
    {
        try
        {
            if (IsAuthenticated() == false)
            {
                return RedirectToAction("Create", "Login");
            }

            TicketClassificationConfigListModel model = new TicketClassificationConfigListModel();
            await LoadClassificationListOptions(model);

            return View(model);
        }
        catch (Exception ex)
        { }

        return View();
    }

    [HttpGet]
    public async Task<IActionResult> TicketClassificationListItemsIndex(Int64 id)
    {
        try
        {
            if (IsAuthenticated() == false)
            {
                return RedirectToAction("Create", "Login");
            }

            var listitem = await _ticketClassificationService.GetTicketClassificationListItemsAsync(id);

            if (listitem != null)
            {
                ViewBag.ListId = id.ToString();
                var model = _mapper.Map<List<TicketClassificationListItemsModel>>(listitem);
                return View(model);
            }
        }
        catch (Exception ex)
        { }

        return View();
    }

    [HttpPost]
    public async Task<IActionResult> CreateList(string listNameId)
    {
        try
        {
            int ret = await _ticketClassificationService.InsertTicketClassificationListAsync(listNameId);

            if (ret > 0)
            {
                var username = HttpContext.Session.GetString("Username");
                var userid = HttpContext.Session.GetString("UserId");

                LogController logController = new LogController();
                logController.Id = ret;
                logController.Module = MODULE_NAME;
                logController.Section = username == null ? string.Empty : username;
                logController.Field = "Todos";
                logController.Value = "Todos";
                logController.UserId = userid != null ? Int64.Parse(userid) : 0;
                logController.Action = "Criar";

                await _logControllerService.InsertLogAPIAsync(logController);
            }
        }
        catch (Exception ex)
        {

        }

        return RedirectToAction("ConfigurationListIndex");
    }

    [HttpPost]
    public async Task<IActionResult> CreateListItem(Int64 listId, string listItemName)
    {
        try
        {
            int ret = await _ticketClassificationService.InsertTicketClassificationListitemAsync(listId, listItemName);

            if (ret > 0)
            {
                var username = HttpContext.Session.GetString("Username");
                var userid = HttpContext.Session.GetString("UserId");

                LogController logController = new LogController();
                logController.Id = ret;
                logController.Module = MODULE_NAME;
                logController.Section = username == null ? string.Empty : username;
                logController.Field = "Todos";
                logController.Value = "Todos";
                logController.UserId = userid != null ? Int64.Parse(userid) : 0;
                logController.Action = "Criar";

                await _logControllerService.InsertLogAPIAsync(logController);
            }
        }
        catch (Exception ex)
        {

        }

        return RedirectToAction("ConfigurationListIndex");
    }


    [HttpGet]
    public async Task<IActionResult> EditListItem(int id)
    {
        if (IsAuthenticated() == false)
        {
            return RedirectToAction("Create", "Login");
        }

        var item = await _ticketClassificationService.GetTicketClassificationListItemAsync(id);

        if (item != null)
        {
            var model = _mapper.Map<TicketClassificationListItemModel>(item);

            model.ListItemDataToCompareIfChanged = new ListItemDataToCompareIfChangedLog();
            model.ListItemDataToCompareIfChanged.Id = model.Id;
            model.ListItemDataToCompareIfChanged.Active = model.Active;
            return View(model);
        }

        return View();
    }

    [HttpPost]
    public async Task<IActionResult> EditListItem([FromForm] TicketClassificationListItemModel model)
    {
        try
        {
            if (IsAuthenticated() == false)
            {
                return RedirectToAction("Create", "Login");
            }

            var ret = await _ticketClassificationService.UpdateTicketClassificationListItemAsync(model.ListItemId, model.Active);

            if (ret > 0)
            {
                var username = HttpContext.Session.GetString("Username");
                var userid = HttpContext.Session.GetString("UserId");

                if (model.Active != model.ListItemDataToCompareIfChanged.Active)
                {
                    LogController logController = new LogController();
                    logController.Id = model.Id;
                    logController.Module = MODULE_NAME;
                    logController.Section = username == null ? string.Empty : username;
                    logController.Field = "Ativar/Desativar";
                    logController.Value = model.ListItemDataToCompareIfChanged.Active ? "Ativado" : "Desativado";
                    logController.UserId = userid != null ? Int64.Parse(userid) : 0;
                    logController.Action = "Editar";

                    await _logControllerService.InsertLogAPIAsync(logController);
                }
            }
        }
        catch (Exception ex)
        { }

        //return RedirectToAction("EditListItem", new { id = model.ListItemId });
        return RedirectToAction("ConfigurationListIndex");

    }

    [HttpGet]
    public async Task<IActionResult> ScreenPopup()
    {
        TicketClassificationModel model = new TicketClassificationModel();
        await LoadManifestationTypeOptions(model);
        LoadServiceUnitOptions(model);
        LoadServiceOptions(model);
        LoadReason01Options(model);
        LoadReason02Options(model);
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

    private void LoadServiceUnitOptions(TicketClassificationModel model, Int64 selectedItem = 0)
    {
        bool isSelected = false;

        model.ServiceUnitOptions.Add(new SelectListItem
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

            model.ServiceUnitOptions.Add(new SelectListItem
            {
                Value = item.Id.ToString(),
                Text = item.Name,
                Selected = isSelected
            });
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

    private void LoadReason01Options(TicketClassificationModel model, Int64 selectedItem = 0)
    {
        bool isSelected = false;

        model.Reason01Options.Add(new SelectListItem
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

            model.Reason01Options.Add(new SelectListItem
            {
                Value = item.Id.ToString(),
                Text = item.Name,
                Selected = isSelected
            });
        }
    }

    private void LoadReason02Options(TicketClassificationModel model, Int64 selectedItem = 0)
    {
        bool isSelected = false;

        model.Reason02Options.Add(new SelectListItem
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

            model.Reason02Options.Add(new SelectListItem
            {
                Value = item.Id.ToString(),
                Text = item.Name,
                Selected = isSelected
            });
        }
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
