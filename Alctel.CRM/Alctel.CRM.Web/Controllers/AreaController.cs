using Alctel.CRM.API.Entities;
using Alctel.CRM.Business.Interfaces;
using Alctel.CRM.Business.Services;
using Alctel.CRM.Context.InMemory.Entities;
using Alctel.CRM.Web.Customs;
using Alctel.CRM.Web.Models;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace Alctel.CRM.Web.Controllers;

public class AreaController : Controller
{
    private const string MODULE_NAME = "areas";
    private readonly IMapper _mapper;
    private readonly IAreaService _areaService;
    private readonly ILogControllerService _logControllerService;
    private readonly ILoginService _loginService;
    private readonly IWebHostEnvironment _hostingEnvironment;
    private readonly IConfigService _configService;

    public AreaController(IMapper mapper, IAreaService serviceUnitService, ILogControllerService logControllerService, ILoginService loginService, IWebHostEnvironment hostingEnvironment, IConfigService configService
)
    {
        _mapper = mapper;
        _areaService = serviceUnitService;
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

            var data = await _areaService.GetAreaListAPIAsync();

            if (data != null && data.Any())
            {
                var dataModel = _mapper.Map<List<AreaModel>>(data);

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
    public async Task<IActionResult> Create([FromForm] AreaModel model)
    {
        if (IsAuthenticated() == false)
        {
            return RedirectToAction("Create", "Login");
        }

        if (ModelState.IsValid)
        {
            var username = HttpContext.Session.GetString("Username");
            var userid = HttpContext.Session.GetString("UserId");
            var data = _mapper.Map<AreaAPI>(model);
            var ret = await _areaService.InsertAreaAPIAsync(data);

            if (ret)
            {
                LogController logController = new LogController();
                logController.Id = model.Id;
                logController.Module = MODULE_NAME;
                logController.Section = username == null ? string.Empty : username;
                logController.Field = "Ativar/Desativar";
                logController.Value = "Ativado";
                logController.Action = "Criar";
                logController.UserId = userid != null ? Int64.Parse(userid) : 0;

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

        var data = await _areaService.GetAreaAPIAsync(long.Parse(id));

        if (data != null)
        {
            var model = _mapper.Map<AreaModel>(data);

            model.AreaDataToCompareIfChanged = new AreaDataToCompareIfChangedLog();
            model.AreaDataToCompareIfChanged.Id = model.Id;
            model.AreaDataToCompareIfChanged.Active = model.Active;

            return View(model);
        }

        return View();
    }

    [HttpPost]
    //[CustomAuthorize]
    public async Task<IActionResult> Edit([FromForm] AreaModel model)
    {
        if (IsAuthenticated() == false)
        {
            return RedirectToAction("Create", "Login");
        }

        if (ModelState.IsValid)
        {
            var username = HttpContext.Session.GetString("Username");
            var userid = HttpContext.Session.GetString("UserId");
            var data = _mapper.Map<AreaAPI>(model);
            var ret = await _areaService.UpdateAreaAPIAsync(data);

            if (ret)
            {
                if (model.Active != model.AreaDataToCompareIfChanged.Active)
                {
                    LogController logController = new LogController();
                    logController.Id = model.Id;
                    logController.Module = MODULE_NAME;
                    logController.Section = username == null ? string.Empty : username;
                    logController.Field = "Ativar/Desativar";
                    logController.Value = model.AreaDataToCompareIfChanged.Active ? "Ativado" : "Desativado";
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

        var data = await _areaService.GetAreaAsync(long.Parse(id));

        if (data != null)
        {
            var model = _mapper.Map<AreaModel>(data);
            return View(model);
        }

        return View();
    }

    [HttpPost]
    //[CustomAuthorize]
    public async Task<IActionResult> Delete([FromForm] AreaModel model)
    {
        if (IsAuthenticated() == false)
        {
            return RedirectToAction("Create", "Login");
        }

        if (ModelState.IsValid)
        {
            var data = _mapper.Map<Area>(model);
            await _areaService.DeleteAreaAsync(data);

            return RedirectToAction("Index");
        }
        return View(model);
    }

    public async Task<IActionResult> Index(string searchAreaType, string searchAreaText)
    {
        if (IsAuthenticated() == false)
        {
            return RedirectToAction("Create", "Login");
        }

        ViewBag.ShowPageControl = false;
        try
        {
            if (searchAreaType.Contains("Tipo Filtro") || searchAreaText == string.Empty)
            {
                ViewBag.AlertFilter = "SHOW";

                var allAreas = await _areaService.GetAreaListAPIAsync();

                if (allAreas != null && allAreas.Any())
                {
                    var AreasModel = _mapper.Map<List<AreaModel>>(allAreas);
                    return View(AreasModel);
                }
            }

            //var users = await _userService.GetAllUserAsync();
            var Areas = await _areaService.SearchAreaAPIAsync(searchAreaType, searchAreaText);

            if (Areas != null && Areas.Any())
            {
                var AreasModel = _mapper.Map<List<AreaModel>>(Areas);
                return View(AreasModel);
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
