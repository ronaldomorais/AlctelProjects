using Alctel.CRM.Business.Interfaces;
using Alctel.CRM.Business.Services;
using Alctel.CRM.Context.InMemory.Entities;
using Alctel.CRM.Web.Models;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Alctel.CRM.Web.Controllers;

public class ClassificationTreeController : Controller
{
    private readonly IClassificationDemandService _classificationDemandService;
    private readonly IMapper _mapper;
    private readonly ILoginService _loginService;

    public ClassificationTreeController(IMapper mapper, IClassificationDemandService classificationDemandService, ILoginService loginService)
    {
        _mapper = mapper;
        _classificationDemandService = classificationDemandService;
        _loginService = loginService;
    }

    public async Task<IActionResult> Index()
    {
        //if (IsAuthenticated() == false)
        //{
        //    return RedirectToAction("Create", "Login");
        //}

        var classificationDemandList = await _classificationDemandService.GetClassficationDemandListAPIAsync();

        if (classificationDemandList != null)
        {
            var model = new ClassificationTreeModel();
            var classificationDemandModel = _mapper.Map<List<ClassificationDemandModel>>(classificationDemandList);
            foreach (var classificationDemand in classificationDemandModel)
            {
                var selectListItem = new SelectListItem
                {
                    //Value = accessprofile.Id.ToString(),
                    Value = classificationDemand.Id.ToString(),
                    Text = classificationDemand.Name,
                };

                model.ClassificationDemandOptions.Add(selectListItem);
            }


            return View(model);
        }

        return View();
    }

    public async Task<IActionResult> Create(bool result, string message)
    {
        //if (IsAuthenticated() == false)
        //{
        //    return RedirectToAction("Create", "Login");
        //}

        if (result)
        {
            ViewBag.ShowMessage = "TRUE";
            ViewBag.Message = message;
        }

        var classificationDemandList = await _classificationDemandService.GetClassficationDemandListAPIAsync();

        if (classificationDemandList != null)
        {
            var model = new ClassificationTreeModel();
            var classificationDemandModel = _mapper.Map<List<ClassificationDemandModel>>(classificationDemandList);
            foreach (var classificationDemand in classificationDemandModel)
            {
                var selectListItem = new SelectListItem
                {
                    //Value = accessprofile.Id.ToString(),
                    Value = classificationDemand.Id.ToString(),
                    Text = classificationDemand.Name,
                };

                model.ClassificationDemandOptions.Add(selectListItem);
            }


            return View(model);
        }

        return View();
    }

    [HttpPost]
    //[CustomAuthorize]
    public async Task<IActionResult> Edit([FromForm] ClassificationTreeModel model)
    {
        return RedirectToAction("Create", new { result = "TRUE", message = "Classificação Ativa com sucesso!" });
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
