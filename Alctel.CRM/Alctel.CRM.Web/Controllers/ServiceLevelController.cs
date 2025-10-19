using Alctel.CRM.API.Entities;
using Alctel.CRM.Business.Interfaces;
using Alctel.CRM.Business.Services;
using Alctel.CRM.Context.InMemory.Entities;
using Alctel.CRM.Web.Customs;
using Alctel.CRM.Web.Models;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace Alctel.CRM.Web.Controllers;

public class ServiceLevelController : Controller
{
    private const string MODULE_NAME = "nivel_atendimento";
    private readonly IMapper _mapper;
    private readonly IServiceLevelService _serviceLevelService;
    private readonly ILogControllerService _logControllerService;
    private readonly ILoginService _loginService;
    private readonly IWebHostEnvironment _hostingEnvironment;
    private readonly IConfigService _configService;

    public ServiceLevelController(IMapper mapper, IServiceLevelService serviceLevelService, ILogControllerService logControllerService, ILoginService loginService, IWebHostEnvironment hostingEnvironment, IConfigService configService)
    {
        _mapper = mapper;
        _serviceLevelService = serviceLevelService;
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

            var data = await _serviceLevelService.GetServiceLevelListAPIAsync();

            if (data != null && data.Any())
            {
                var dataModel = _mapper.Map<List<ServiceLevelModel>>(data);

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
    public async Task<IActionResult> Create([FromForm] ServiceLevelModel model)
    {
        if (IsAuthenticated() == false)
        {
            return RedirectToAction("Create", "Login");
        }

        if (ModelState.IsValid)
        {
            var username = HttpContext.Session.GetString("Username");
            var userid = HttpContext.Session.GetString("UserId");
            var data = _mapper.Map<ServiceLevelAPI>(model);
            var ret = await _serviceLevelService.InsertServiceLevelAPIAsync(data);

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

        var data = await _serviceLevelService.GetServiceLevelAPIAsync(long.Parse(id));

        if (data != null)
        {
            var model = _mapper.Map<ServiceLevelModel>(data);

            model.ServiceLevelDataToCompareIfChanged = new ServiceLevelDataToCompareIfChangedLog();
            model.ServiceLevelDataToCompareIfChanged.Id = model.Id;
            model.ServiceLevelDataToCompareIfChanged.Active = model.Active;

            return View(model);
        }

        return View();
    }

    [HttpPost]
    //[CustomAuthorize]
    public async Task<IActionResult> Edit([FromForm] ServiceLevelModel model)
    {
        if (IsAuthenticated() == false)
        {
            return RedirectToAction("Create", "Login");
        }

        if (ModelState.IsValid)
        {
            var username = HttpContext.Session.GetString("Username");
            var userid = HttpContext.Session.GetString("UserId");
            var data = _mapper.Map<ServiceLevelAPI>(model);
            var ret = await _serviceLevelService.UpdateServiceLevelAPIAsync(data);

            if (ret)
            {
                if (model.Active != model.ServiceLevelDataToCompareIfChanged.Active)
                {
                    LogController logController = new LogController();
                    logController.Id = model.Id;
                    logController.Module = MODULE_NAME;
                    logController.Section = username == null ? string.Empty : username;
                    logController.Field = "Ativar/Desativar";
                    logController.Value = model.ServiceLevelDataToCompareIfChanged.Active ? "Ativado" : "Desativado";
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

        var data = await _serviceLevelService.GetServiceLevelAPIAsync(long.Parse(id));

        if (data != null)
        {
            var model = _mapper.Map<ServiceLevelModel>(data);
            return View(model);
        }

        return View();
    }

    [HttpPost]
    //[CustomAuthorize]
    public async Task<IActionResult> Delete([FromForm] ServiceLevelModel model)
    {
        if (IsAuthenticated() == false)
        {
            return RedirectToAction("Create", "Login");
        }

        if (ModelState.IsValid)
        {
            var data = _mapper.Map<ServiceLevel>(model);
            await _serviceLevelService.DeleteServiceLevelAsync(data);

            return RedirectToAction("Index");
        }
        return View(model);
    }

    public async Task<IActionResult> Index(string searchServiceLevelType, string searchServiceLevelText)
    {
        if (IsAuthenticated() == false)
        {
            return RedirectToAction("Create", "Login");
        }

        ViewBag.ShowPageControl = false;
        try
        {
            if (searchServiceLevelType.Contains("Tipo Filtro") || searchServiceLevelText == string.Empty)
            {
                ViewBag.AlertFilter = "SHOW";

                var allServiceLevels = await _serviceLevelService.GetServiceLevelListAPIAsync();

                if (allServiceLevels != null && allServiceLevels.Any())
                {
                    var ServiceLevelsModel = _mapper.Map<List<ServiceLevelModel>>(allServiceLevels);
                    return View(ServiceLevelsModel);
                }
            }

            var ServiceLevels = await _serviceLevelService.SearchServiceLevelAPIAsync(searchServiceLevelType, searchServiceLevelText);

            if (ServiceLevels != null && ServiceLevels.Any())
            {
                var ServiceLevelsModel = _mapper.Map<List<ServiceLevelModel>>(ServiceLevels);
                return View(ServiceLevelsModel);
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
