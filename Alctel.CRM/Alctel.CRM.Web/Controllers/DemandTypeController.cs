using Alctel.CRM.API.Entities;
using Alctel.CRM.Business.Interfaces;
using Alctel.CRM.Business.Services;
using Alctel.CRM.Context.InMemory.Entities;
using Alctel.CRM.Web.Customs;
using Alctel.CRM.Web.Models;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace Alctel.CRM.Web.Controllers;

public class DemandTypeController : Controller
{
    private const string MODULE_NAME = "motivo_demanda";
    private readonly IMapper _mapper;
    private readonly IDemandTypeService _demandTypeService;
    private readonly ILogControllerService _logControllerService;
    private readonly ILoginService _loginService;

    public DemandTypeController(IMapper mapper, IDemandTypeService DemandTypeService, ILogControllerService logControllerService, ILoginService loginService)
    {
        _mapper = mapper;
        _demandTypeService = DemandTypeService;
        _logControllerService = logControllerService;
        _loginService = loginService;
    }

    [HttpGet]
    //[CustomAuthorize]
    public async Task<IActionResult> Index()
    {
        try
        {
            if (IsAuthenticated() == false)
            {
                return RedirectToAction("Create", "Login");
            }

            var data = await _demandTypeService.GetDemandTypeListAPIAsync();

            if (data != null && data.Any())
            {
                var dataModel = _mapper.Map<List<DemandTypeModel>>(data);

                return View(dataModel);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
        }
        return View();
    }

    [HttpGet]
    //[CustomAuthorize]
    public IActionResult Create()
    {
        if (IsAuthenticated() == false)
        {
            return RedirectToAction("Create", "Login");
        }

        return View();
    }

    [HttpPost]
    //[CustomAuthorize]
    public async Task<IActionResult> Create([FromForm] DemandTypeModel model)
    {
        if (IsAuthenticated() == false)
        {
            return RedirectToAction("Create", "Login");
        }

        if (ModelState.IsValid)
        {
            var username = HttpContext.Session.GetString("Username");
            var userid = HttpContext.Session.GetString("UserId");
            var data = _mapper.Map<DemandTypeAPI>(model);
            var ret = await _demandTypeService.InsertDemandTypeAPIAsync(data);

            if (ret)
            {
                LogController logController = new LogController();
                logController.Id = model.Id;
                logController.Module = MODULE_NAME;
                logController.Section = username == null ? string.Empty : username;
                logController.Field = "Ativar/Desativar";
                logController.Value = "Ativado";
                logController.UserId = userid != null ? Int64.Parse(userid) : 0;
                logController.Action = "Criar";

                await _logControllerService.InsertLogAPIAsync(logController);
            }

            return RedirectToAction("Index");
        }
        return View(model);
    }

    [HttpGet]
    //[CustomAuthorize]
    public async Task<IActionResult> Edit(string id)
    {
        if (IsAuthenticated() == false)
        {
            return RedirectToAction("Create", "Login");
        }

        var data = await _demandTypeService.GetDemandTypeAPIAsync(long.Parse(id));

        if (data != null)
        {
            var model = _mapper.Map<DemandTypeModel>(data);

            model.DemandTypeDataToCompareIfChanged = new DemandTypeDataToCompareIfChangedLog();
            model.DemandTypeDataToCompareIfChanged.Id = model.Id;
            model.DemandTypeDataToCompareIfChanged.Active = model.Active;

            return View(model);
        }

        return View();
    }

    [HttpPost]
    //[CustomAuthorize]
    public async Task<IActionResult> Edit([FromForm] DemandTypeModel model)
    {
        if (IsAuthenticated() == false)
        {
            return RedirectToAction("Create", "Login");
        }

        if (ModelState.IsValid)
        {
            var username = HttpContext.Session.GetString("Username");
            var userid = HttpContext.Session.GetString("UserId");
            var data = _mapper.Map<DemandTypeAPI>(model);
            var ret = await _demandTypeService.UpdateDemandTypeAPIAsync(data);

            if (ret)
            {
                if (model.Active != model.DemandTypeDataToCompareIfChanged.Active)
                {
                    LogController logController = new LogController();
                    logController.Id = model.Id;
                    logController.Module = MODULE_NAME;
                    logController.Section = username == null ? string.Empty : username;
                    logController.Field = "Ativar/Desativar";
                    logController.Value = model.DemandTypeDataToCompareIfChanged.Active ? "Ativado" : "Desativado";
                    logController.UserId = userid != null ? Int64.Parse(userid) : 0;
                    logController.Action = "Editar";

                    await _logControllerService.InsertLogAPIAsync(logController);
                }
            }

            return RedirectToAction("Index");
        }
        return View(model);
    }

    [HttpGet]
    //[CustomAuthorize]
    public async Task<IActionResult> Delete(string id)
    {
        if (IsAuthenticated() == false)
        {
            return RedirectToAction("Create", "Login");
        }

        var data = await _demandTypeService.GetDemandTypeAsync(long.Parse(id));

        if (data != null)
        {
            var model = _mapper.Map<DemandTypeModel>(data);
            return View(model);
        }

        return View();
    }

    [HttpPost]
    //[CustomAuthorize]
    public async Task<IActionResult> Delete([FromForm] DemandTypeModel model)
    {
        if (IsAuthenticated() == false)
        {
            return RedirectToAction("Create", "Login");
        }

        if (ModelState.IsValid)
        {
            var data = _mapper.Map<DemandType>(model);
            await _demandTypeService.DeleteDemandTypeAsync(data);

            return RedirectToAction("Index");
        }
        return View(model);
    }

    public async Task<IActionResult> Index(string searchDemandTypeType, string searchDemandTypeText)
    {
        if (IsAuthenticated() == false)
        {
            return RedirectToAction("Create", "Login");
        }

        ViewBag.ShowPageControl = false;
        try
        {
            if (searchDemandTypeType.Contains("Tipo Filtro") || searchDemandTypeText == string.Empty)
            {
                ViewBag.AlertFilter = "SHOW";

                var allDemandTypes = await _demandTypeService.GetDemandTypeListAPIAsync();

                if (allDemandTypes != null && allDemandTypes.Any())
                {
                    var DemandTypesModel = _mapper.Map<List<DemandTypeModel>>(allDemandTypes);
                    return View(DemandTypesModel);
                }
            }

            var DemandTypes = await _demandTypeService.SearchDemandTypeAPIAsync(searchDemandTypeType, searchDemandTypeText);

            if (DemandTypes != null && DemandTypes.Any())
            {
                var DemandTypesModel = _mapper.Map<List<DemandTypeModel>>(DemandTypes);
                return View(DemandTypesModel);
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
