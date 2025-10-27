using Alctel.CRM.API.Entities;
using Alctel.CRM.Business.Interfaces;
using Alctel.CRM.Business.Services;
using Alctel.CRM.Web.Models;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace Alctel.CRM.Web.Controllers;

public class TicketClassificationListController : Controller
{
    private const string MODULE_NAME = "lista_item";
    private readonly IMapper _mapper;
    private readonly ILoginService _loginService;
    private readonly ITicketClassificationService _ticketClassificationService;
    private readonly ILogControllerService _logControllerService;

    public TicketClassificationListController(IMapper mapper, ILoginService loginService, ITicketClassificationService ticketClassificationService, ILogControllerService logControllerService)
    {
        _loginService = loginService;
        _ticketClassificationService = ticketClassificationService;
        _mapper = mapper;
        _logControllerService = logControllerService;
    }

    public async Task<IActionResult> Index()
    {
        try
        {
            if (IsAuthenticated() == false)
            {
                return RedirectToAction("Create", "Login");
            }

            var list = await _ticketClassificationService.GetTicketClassificationListAsync();

            if (list != null)
            {
                if (TempData["ScreenMessage"] != null)
                {
                    ViewBag.ScreenMessage = TempData["ScreenMessage"];
                    TempData["ScreenMessage"] = null;
                }

                var model = _mapper.Map<List<TicketClassificationListModel>>(list);
                return View(model);
            }
        }
        catch (Exception ex)
        { }
        return View();
    }


    [HttpPost]
    public async Task<IActionResult> Create(string listName)
    {
        try
        {
            int ret = await _ticketClassificationService.InsertTicketClassificationListAsync(listName);

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

                TempData["ScreenMessage"] = $"Lista {listName} criada com sucesso.";
            }
            else
            {
                TempData["ScreenMessage"] = $"ERRO: criando lista {listName}.";
            }
        }
        catch (Exception ex)
        {
            TempData["ScreenMessage"] = $"ERRO: criando item {listName}.";
        }

        return RedirectToAction("Index");
    }

    public async Task<IActionResult> Search(string searchlistType, string searchlistText)
    {
        if (IsAuthenticated() == false)
        {
            return RedirectToAction("Create", "Login");
        }

        try
        {
            if (searchlistType.Contains("Tipo Filtro") || searchlistText == string.Empty)
            {
                ViewBag.AlertFilter = "SHOW";

                var alllists = await _ticketClassificationService.GetTicketClassificationListAsync();

                if (alllists != null && alllists.Any())
                {
                    var listsModel = _mapper.Map<List<TicketClassificationListModel>>(alllists);
                    return View(listsModel);
                }
            }


            //var users = await _userService.GetAllUserAsync();
            var list = await _ticketClassificationService.SearchTicketClassificationListAsync(searchlistType, searchlistText);

            if (list != null && list.Any())
            {
                var customersModel = _mapper.Map<List<TicketClassificationListModel>>(list);
                return View(customersModel);
            }

        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
        }
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