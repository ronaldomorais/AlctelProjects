using Alctel.CRM.API.Entities;
using Alctel.CRM.Business.Interfaces;
using Alctel.CRM.Business.Interfaces.Classification;
using Alctel.CRM.Business.Services;
using Alctel.CRM.Web.Customs;
using Alctel.CRM.Web.Models;
using Alctel.CRM.Web.Models.Classification;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace Alctel.CRM.Web.Controllers;

public class ClassificationListItemsController : Controller
{
    private const string MODULE_NAME = "lista_item";
    private readonly IClassificationReasonService _classificationReasonService;
    private readonly IClassificationListItemsService _classificationListItemsService;
    private readonly IMapper _mapper;
    private readonly ILogControllerService _logControllerService;
    private readonly ILoginService _loginService;

    public ClassificationListItemsController(IMapper mapper, IClassificationReasonService classificationReasonService, IClassificationListItemsService classificationListItemsService, ILogControllerService logControllerService, ILoginService loginService)
    {
        _mapper = mapper;
        _classificationReasonService = classificationReasonService;
        _classificationListItemsService = classificationListItemsService;
        _logControllerService = logControllerService;
        _loginService = loginService;
    }

    [HttpGet]
    //[CustomAuthorize]
    public async Task<IActionResult> Index(Int64 id)
    {
        try
        {
            //if (IsAuthenticated() == false)
            //{
            //    return RedirectToAction("Create", "Login");
            //}

            if (id == 0)
            {
                return RedirectToAction($"Index", "ClassificationList");
            }
            ViewBag.ClassificationListId = id;

            var data = await _classificationListItemsService.GetClassificationListItemsListAPIAsync(id);

            if (data != null && data.Any())
            {
                var dataModel = _mapper.Map<List<ClassificationListItemsModel>>(data);
                
                return View(dataModel);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
        }


        return View(new List<ClassificationListItemsModel>());
    }

    [HttpGet]
    //[CustomAuthorize]
    public IActionResult Create(int id)
    {
        //if (IsAuthenticated() == false)
        //{
        //    return RedirectToAction("Create", "Login");
        //}

        if (id == 0)
        {
            return RedirectToAction($"Index", "ClassificationList");
        }
        var model = new ClassificationListItemsModel();
        model.ClassificationListId = id;
        return View(model);
    }

    [HttpPost]
    //[CustomAuthorize]
    public async Task<IActionResult> Create([FromForm] ClassificationListItemsModel model)
    {
        //if (IsAuthenticated() == false)
        //{
        //    return RedirectToAction("Create", "Login");
        //}

        if (ModelState.IsValid)
        {
            var username = HttpContext.Session.GetString("Username");
            var userid = HttpContext.Session.GetString("UserId");
            var data = _mapper.Map<ClassificationListItemsAPI>(model);
            var ret = await _classificationListItemsService.InsertClassificationListItemsAPIAsync(data);

            if (ret > 0)
            {
                LogController logController = new LogController();
                logController.Id = ret;
                logController.Module = MODULE_NAME;
                logController.Section = username == null ? string.Empty : username;
                logController.Field = "Ativar/Desativar";
                logController.Value = "Ativado";
                logController.UserId = userid != null ? Int64.Parse(userid) : 0;
                logController.Action = "Criar";

                await _logControllerService.InsertLogAPIAsync(logController);

                return RedirectToAction($"Index", "ClassificationListItems", new { id = model.ClassificationListId });
            }

        }

        ModelState.AddModelError("", "Falha na criação de item");
        return View(model);
    }


    [HttpGet]
    //[CustomAuthorize]
    public async Task<IActionResult> Edit(string id)
    {
        //if (IsAuthenticated() == false)
        //{
        //    return RedirectToAction("Create", "Login");
        //}

        var data = await _classificationListItemsService.GetClassificationListItemAPIAsync(long.Parse(id));

        if (data != null)
        {
            var model = _mapper.Map<ClassificationListItemsModel>(data);

            model.ClassificationListItemsDataToCompareIfChanged = new ClassificationListItemsDataToCompareIfChangedLog();
            model.ClassificationListItemsDataToCompareIfChanged.Id = model.Id;
            model.ClassificationListItemsDataToCompareIfChanged.Active = model.Active;

            return View(model);
        }

        return View();
    }

    [HttpPost]
    //[CustomAuthorize]
    public async Task<IActionResult> Edit([FromForm] ClassificationListItemsModel model)
    {
        //if (IsAuthenticated() == false)
        //{
        //    return RedirectToAction("Create", "Login");
        //}

        if (ModelState.IsValid)
        {
            var username = HttpContext.Session.GetString("Username");
            var userid = HttpContext.Session.GetString("UserId");
            var data = _mapper.Map<ClassificationListItemsAPI>(model);
            var ret = await _classificationListItemsService.UpdateClassificationListItemsAPIAsync(data);

            if (ret)
            {
                if (model.Active != model.ClassificationListItemsDataToCompareIfChanged.Active)
                {
                    LogController logController = new LogController();
                    logController.Id = model.Id;
                    logController.Module = MODULE_NAME;
                    logController.Section = username == null ? string.Empty : username;
                    logController.Field = "Ativar/Desativar";
                    logController.Value = model.ClassificationListItemsDataToCompareIfChanged.Active ? "Ativado" : "Desativado";
                    logController.UserId = userid != null ? Int64.Parse(userid) : 0;
                    logController.Action = "Editar";

                    await _logControllerService.InsertLogAPIAsync(logController);
                }
            }

            return RedirectToAction($"Index", "ClassificationListItems", new { id = model.ClassificationListId });
        }
        return View(model);
    }


    [HttpGet]
    public async Task<List<ClassificationReasonModel>> GetClassificationReasonList(Int64 id)
    {
        var model = new List<ClassificationReasonModel>();
        try
        {
            var classificationDemandList = await _classificationReasonService.GetClassificationReasonAPIAsync(id);

            if (classificationDemandList != null)
            {

                var classificationDemandModelList = _mapper.Map<List<ClassificationReasonModel>>(classificationDemandList);

                model.AddRange(classificationDemandModelList);
            }
        }
        catch (Exception ex) { }

        return model;
    }

    [HttpGet]
    public async Task<List<ClassificationListItemsModel>> GetClassificationListItemsActive(Int64 id)
    {
        var model = new List<ClassificationListItemsModel>();
        try
        {
            var classificationDemandList = await _classificationListItemsService.GetClassificationListItemsActiveAPIAsync(id);

            if (classificationDemandList != null)
            {

                var classificationDemandModelList = _mapper.Map<List<ClassificationListItemsModel>>(classificationDemandList);

                model.AddRange(classificationDemandModelList);
            }
        }
        catch (Exception ex) { }

        return model;
    }

    [HttpGet]
    public async Task<List<ClassificationReasonChildrenModel>> ClassificationReasonChildren(Int64 id)
    {
        var model = new List<ClassificationReasonChildrenModel>();
        try
        {
            var classificationDemandList = await _classificationReasonService.GetClassificationReasonListChildrenAPIAsync(id);

            if (classificationDemandList != null)
            {

                var classificationDemandModelList = _mapper.Map<List<ClassificationReasonChildrenModel>>(classificationDemandList);

                model.AddRange(classificationDemandModelList);
            }
        }
        catch (Exception ex) { }

        return model;
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
