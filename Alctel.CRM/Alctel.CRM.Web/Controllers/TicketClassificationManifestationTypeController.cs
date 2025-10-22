using Alctel.CRM.API.Entities;
using Alctel.CRM.Business.Interfaces;
using Alctel.CRM.Business.Services;
using Alctel.CRM.Web.Models;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace Alctel.CRM.Web.Controllers;

public class TicketClassificationManifestationTypeController : Controller
{
    private const string MODULE_NAME = "lista_item";
    private readonly ILoginService _loginService;
    private readonly ITicketClassificationService _ticketClassificationService;
    private readonly IMapper _mapper;
    private readonly ILogControllerService _logControllerService;

    public TicketClassificationManifestationTypeController(ITicketClassificationService ticketClassificationService, IMapper mapper, ILoginService loginService, ILogControllerService logControllerService)
    {
        _ticketClassificationService = ticketClassificationService;
        _mapper = mapper;
        _loginService = loginService;
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

            if (TempData["ScreenMessage"] != null)
            {
                ViewBag.ScreenMessage = TempData["ScreenMessage"];
                TempData["ScreenMessage"] = null;
            }

            var list = await _ticketClassificationService.GetTicketClassificationManifestationTypeAPIAsync();

            if (list != null)
            {
                var model = _mapper.Map<List<TicketClassificationManifestationTypeModel>>(list);
                return View(model);
            }
        }
        catch (Exception ex)
        {
        }
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(string manifestationName)
    {
        try
        {
            if (IsAuthenticated() == false)
            {
                return RedirectToAction("Create", "Login");
            }

            int ret = await _ticketClassificationService.InsertTicketClassificationManifestationTypeAsync(manifestationName);

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

                TempData["ScreenMessage"] = $"Manifestação {manifestationName} criada com sucesso.";
            }
            else
            {
                TempData["ScreenMessage"] = $"ERRO: criando Manifestação {manifestationName}.";
            }
        }
        catch (Exception ex)
        {
            TempData["ScreenMessage"] = $"ERRO: criando item {manifestationName}.";
        }

        return RedirectToAction("Index");
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
