using Alctel.CRM.API.Entities;
using Alctel.CRM.Business.Interfaces;
using Alctel.CRM.Business.Services;
using Alctel.CRM.Context.InMemory.Entities;
using Alctel.CRM.Web.Customs;
using Alctel.CRM.Web.Models;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace Alctel.CRM.Web.Controllers;

public class ClassificationListController : Controller
{
    private const string MODULE_NAME = "classificacao_list";
    private readonly IMapper _mapper;
    private readonly IClassificationListService _classificationListService;
    private readonly ILogControllerService _logControllerService;
    private readonly ILoginService _loginService;

    public ClassificationListController(IMapper mapper, IClassificationListService serviceUnitService, ILogControllerService logControllerService, ILoginService loginService)
    {
        _mapper = mapper;
        _classificationListService = serviceUnitService;
        _logControllerService = logControllerService;
        _loginService = loginService;
    }

    [HttpGet]
    //[CustomAuthorize]
    public async Task<IActionResult> Index()
    {
        try
        {
            //if (IsAuthenticated() == false)
            //{
            //    return RedirectToAction("Create", "Login");
            //}

            var data = await _classificationListService.GetClassificationListListAPIAsync();

            if (data != null && data.Any())
            {
                var dataModel = _mapper.Map<List<ClassificationListModel>>(data);

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
        //if (IsAuthenticated() == false)
        //{
        //    return RedirectToAction("Create", "Login");
        //}

        return View();
    }

    //[HttpPost]
    ////[CustomAuthorize]
    //public async Task<IActionResult> Create([FromForm] ClassificationListModel model)
    //{
    //    if (ModelState.IsValid)
    //    {
    //        var data = _mapper.Map<ClassificationListAPI>(model);
    //        var ret = await _classificationListService.InsertClassificationListAPIAsync(data);

    //        //if (ret)
    //        //{
    //        //    LogController logController = new LogController();
    //        //    logController.Id = model.Id;
    //        //    logController.Module = MODULE_NAME;
    //        //    logController.Section = "";
    //        //    logController.Field = "Ativar/Desativar";
    //        //    logController.Value = "Ativado";
    //        //    logController.Action = "Criar";

    //        //    await _logControllerService.InsertLogAPIAsync(logController);
    //        //}

    //        return RedirectToAction("Index");
    //    }
    //    return View(model);
    //}

    [HttpGet]
    //[CustomAuthorize]
    public async Task<IActionResult> Edit(string id)
    {
        //if (IsAuthenticated() == false)
        //{
        //    return RedirectToAction("Create", "Login");
        //}

        //var data = await _classificationListService.GetClassificationListAPIAsync(long.Parse(id));

        //if (data != null)
        //{
        //    var model = _mapper.Map<ClassificationListModel>(data);

        //    //model.ClassificationListDataToCompareIfChanged = new ClassificationListDataToCompareIfChangedLog();
        //    //model.ClassificationListDataToCompareIfChanged.Id = model.Id;
        //    //model.ClassificationListDataToCompareIfChanged.Active = model.Active;

        //    return View(model);
        //}

        //return View();

        return RedirectToAction($"Index", "ClassificationListItems", new { id = id });
    }

    //[HttpPost]
    ////[CustomAuthorize]
    //public async Task<IActionResult> Edit([FromForm] ClassificationListModel model)
    //{
    //    if (ModelState.IsValid)
    //    {
    //        var data = _mapper.Map<ClassificationListAPI>(model);
    //        var ret = await _classificationListService.UpdateClassificationListAPIAsync(data);

    //        //if (ret)
    //        //{
    //        //    if (model.Active != model.ClassificationListDataToCompareIfChanged.Active)
    //        //    {
    //        //        LogController logController = new LogController();
    //        //        logController.Id = model.Id;
    //        //        logController.Module = MODULE_NAME;
    //        //        logController.Section = "";
    //        //        logController.Field = "Ativar/Desativar";
    //        //        logController.Value = model.ClassificationListDataToCompareIfChanged.Active ? "Ativado" : "Desativado";
    //        //        logController.Action = "Editar";

    //        //        await _logControllerService.InsertLogAPIAsync(logController);
    //        //    }
    //        //}

    //        return RedirectToAction("Index");
    //    }
    //    return View(model);
    //}

    //public async Task<IActionResult> Index(string searchClassificationListType, string searchClassificationListText)
    //{
    //    ViewBag.ShowPageControl = false;
    //    try
    //    {
    //        if (searchClassificationListType.Contains("Tipo Filtro") || searchClassificationListText == string.Empty)
    //        {
    //            ViewBag.AlertFilter = "SHOW";

    //            var allClassificationLists = await _classificationListService.GetClassificationListListAPIAsync();

    //            if (allClassificationLists != null && allClassificationLists.Any())
    //            {
    //                var ClassificationListsModel = _mapper.Map<List<ClassificationListModel>>(allClassificationLists);
    //                return View(ClassificationListsModel);
    //            }
    //        }

    //        //var users = await _userService.GetAllUserAsync();
    //        var ClassificationLists = await _classificationListService.SearchClassificationListAPIAsync(searchClassificationListType, searchClassificationListText);

    //        if (ClassificationLists != null && ClassificationLists.Any())
    //        {
    //            var ClassificationListsModel = _mapper.Map<List<ClassificationListModel>>(ClassificationLists);
    //            return View(ClassificationListsModel);
    //        }

    //    }
    //    catch (Exception ex)
    //    {
    //        Console.WriteLine(ex);
    //    }
    //    return View();
    //}

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
