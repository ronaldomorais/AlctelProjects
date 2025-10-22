using Alctel.CRM.API.Entities;
using Alctel.CRM.Business.Interfaces;
using Alctel.CRM.Web.Models;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace Alctel.CRM.Web.Controllers;

public class TicketClassificationListItemController : Controller
{
    private const string MODULE_NAME = "lista_item";
    private readonly IMapper _mapper;
    private readonly ILoginService _loginService;
    private readonly ITicketClassificationService _ticketClassificationService;
    private readonly ILogControllerService _logControllerService;

    public TicketClassificationListItemController(IMapper mapper, ILoginService loginService, ITicketClassificationService ticketClassificationService, ILogControllerService logControllerService)
    {
        _loginService = loginService;
        _ticketClassificationService = ticketClassificationService;
        _mapper = mapper;
        _logControllerService = logControllerService;
    }

    public async Task<IActionResult> Index(Int64 id)
    {
        try
        {
            if (IsAuthenticated() == false)
            {
                return RedirectToAction("Create", "Login");
            }

            if (TempData["ScreenMessage"] != null)
            {
                ViewBag.ScreenMessage = TempData["ScreenMessage"];
                TempData["ScreenMessage"] = null;
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

    [HttpGet]
    public async Task<IActionResult> Edit(int id)
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
    public async Task<IActionResult> Edit([FromForm] TicketClassificationListItemModel model)
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
                    logController.Id = model.ListItemId;
                    logController.Module = MODULE_NAME;
                    logController.Section = username == null ? string.Empty : username;
                    logController.Field = "Ativar/Desativar";
                    logController.Value = model.ListItemDataToCompareIfChanged.Active ? "Ativado" : "Desativado";
                    logController.UserId = userid != null ? Int64.Parse(userid) : 0;
                    logController.Action = "Editar";

                    await _logControllerService.InsertLogAPIAsync(logController);

                    TempData["ScreenMessage"] = $"Item atualizado com sucesso.";
                }
                else
                {
                    TempData["ScreenMessage"] = $"ERRO: atualizado item.";
                }
            }
        }
        catch (Exception ex)
        {
            TempData["ScreenMessage"] = $"ERRO: atualizado item.";
        }

        //return RedirectToAction("EditListItem", new { id = model.ListItemId });
        return RedirectToAction("Index", new { id = model.Id });

    }

    [HttpPost]
    public async Task<IActionResult> Create(Int64 listId, string listItemName)
    {
        try
        {
            if (IsAuthenticated() == false)
            {
                return RedirectToAction("Create", "Login");
            }

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

                TempData["ScreenMessage"] = $"Item {listItemName} criado com sucesso.";
            }
            else
            {
                TempData["ScreenMessage"] = $"ERRO: criando item {listItemName}.";
            }
        }
        catch (Exception ex)
        {
            TempData["ScreenMessage"] = $"ERRO: criando item {listItemName}.";
        }

        return RedirectToAction("Index", new { id = listId });
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
