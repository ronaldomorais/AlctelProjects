using Alctel.CRM.API.Entities;
using Alctel.CRM.Business.Interfaces;
using Alctel.CRM.Business.Services;
using Alctel.CRM.Context.InMemory.Entities;
using Alctel.CRM.Web.Customs;
using Alctel.CRM.Web.Models;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace Alctel.CRM.Web.Controllers;

public class ServiceUnitController : Controller
{
    private const string MODULE_NAME = "unidades";
    private readonly IMapper _mapper;
    private readonly IServiceUnitService _serviceUnitService;
    private readonly ILogControllerService _logControllerService;
    private readonly ILoginService _loginService;
    private readonly IWebHostEnvironment _hostingEnvironment;
    private readonly IConfigService _configService;

    public ServiceUnitController(IMapper mapper, IServiceUnitService serviceUnitService, ILogControllerService logControllerService, ILoginService loginService, IWebHostEnvironment hostingEnvironment, IConfigService configService)
    {
        _mapper = mapper;
        _serviceUnitService = serviceUnitService;
        _logControllerService = logControllerService;
        _loginService = loginService;
        _hostingEnvironment = hostingEnvironment;
        _configService = configService;
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

            string physicalPath = _hostingEnvironment.WebRootPath;
            ViewBag.BaseUrl = _configService.GetBaseUrl(physicalPath);

            //var data = await _serviceUnitService.GetAllServiceUnitAsync();
            var data = await _serviceUnitService.GetServiceUnitListAPIAsync();

            if (data != null && data.Any())
            {
                var dataModel = _mapper.Map<List<ServiceUnitModel>>(data);

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
    public async Task<IActionResult> Create([FromForm] ServiceUnitModel model)
    {
        if (IsAuthenticated() == false)
        {
            return RedirectToAction("Create", "Login");
        }

        if (ModelState.IsValid)
        {
            var username = HttpContext.Session.GetString("Username");
            var userid = HttpContext.Session.GetString("UserId");
            var data = _mapper.Map<ServiceUnitAPI>(model);
            var ret = await _serviceUnitService.InsertServiceUnitAPIAsync(data);

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

        var data = await _serviceUnitService.GetServiceUnitAPIAsync(long.Parse(id));

        if (data != null)
        {
            var model = _mapper.Map<ServiceUnitModel>(data);

            model.ServiceUnitDataToCompareIfChanged = new ServiceUnitDataToCompareIfChangedLog();
            model.ServiceUnitDataToCompareIfChanged.Id = model.Id;
            model.ServiceUnitDataToCompareIfChanged.Active = model.Active;

            return View(model);
        }

        return View();
    }

    [HttpPost]
    //[CustomAuthorize]
    public async Task<IActionResult> Edit([FromForm] ServiceUnitModel model)
    {
        if (IsAuthenticated() == false)
        {
            return RedirectToAction("Create", "Login");
        }

        if (ModelState.IsValid)
        {
            var username = HttpContext.Session.GetString("Username");
            var userid = HttpContext.Session.GetString("UserId");
            var data = _mapper.Map<ServiceUnitAPI>(model);
            var ret =await _serviceUnitService.UpdateServiceUnitAPIAsync(data);

            if (ret)
            {
                if (model.Active != model.ServiceUnitDataToCompareIfChanged.Active)
                {
                    LogController logController = new LogController();
                    logController.Id = model.Id;
                    logController.Module = MODULE_NAME;
                    logController.Section = username == null ? string.Empty : username;
                    logController.Field = "Ativar/Desativar";
                    logController.Value = model.ServiceUnitDataToCompareIfChanged.Active ? "Ativado" : "Desativado";
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

        var data = await _serviceUnitService.GetServiceUnitAsync(long.Parse(id));

        if (data != null)
        {
            var model = _mapper.Map<ServiceUnitModel>(data);
            return View(model);
        }

        return View();
    }

    [HttpPost]
    //[CustomAuthorize]
    public async Task<IActionResult> Delete([FromForm] ServiceUnitModel model)
    {
        if (IsAuthenticated() == false)
        {
            return RedirectToAction("Create", "Login");
        }

        if (ModelState.IsValid)
        {
            var data = _mapper.Map<ServiceUnit>(model);
            await _serviceUnitService.DeleteServiceUnitAsync(data);

            return RedirectToAction("Index");
        }
        return View(model);
    }

    public async Task<IActionResult> Index(string searchServiceUnitType, string searchServiceUnitText)
    {
        if (IsAuthenticated() == false)
        {
            return RedirectToAction("Create", "Login");
        }

        ViewBag.ShowPageControl = false;
        try
        {
            if (searchServiceUnitType.Contains("Tipo Filtro") || searchServiceUnitText == string.Empty)
            {
                ViewBag.AlertFilter = "SHOW";

                var allServiceUnits = await _serviceUnitService.GetServiceUnitListAPIAsync();

                if (allServiceUnits != null && allServiceUnits.Any())
                {
                    var ServiceUnitsModel = _mapper.Map<List<ServiceUnitModel>>(allServiceUnits);
                    return View(ServiceUnitsModel);
                }
            }

            //var users = await _userService.GetAllUserAsync();
            var ServiceUnits = await _serviceUnitService.SearchServiceUnitAPIAsync(searchServiceUnitType, searchServiceUnitText);

            if (ServiceUnits != null && ServiceUnits.Any())
            {
                var ServiceUnitsModel = _mapper.Map<List<ServiceUnitModel>>(ServiceUnits);
                return View(ServiceUnitsModel);
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
    public async Task<JsonResult> GetList()
    {
        try
        {
            var data = await _serviceUnitService.GetServiceUnitListAPIAsync();

            if (data != null && data.Any())
            {
                return new JsonResult(_mapper.Map<List<ServiceUnitModel>>(data));

                
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
        }
        return new JsonResult(new List<ServiceUnitModel>());
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
