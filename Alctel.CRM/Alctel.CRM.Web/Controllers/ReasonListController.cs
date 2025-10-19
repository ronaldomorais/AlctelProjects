using Alctel.CRM.Business.Interfaces;
using Alctel.CRM.Business.Interfaces.Reason;
using Alctel.CRM.Business.Services;
using Alctel.CRM.Context.InMemory.Entities;
using Alctel.CRM.Context.InMemory.Entities.Classification;
using Alctel.CRM.Web.Customs;
using Alctel.CRM.Web.Models;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace Alctel.CRM.Web.Controllers;

public class ReasonListController : Controller
{
    private readonly IMapper _mapper;
    private readonly IReasonListService _reasonListService;
    private readonly ILoginService _loginService;

    public ReasonListController(IMapper mapper, IReasonListService ClassificationListService, ILoginService loginService)
    {
        _mapper = mapper;
        _reasonListService = ClassificationListService;
        _loginService = loginService;
    }

    [HttpGet]
    //[CustomAuthorize]
    public async Task<IActionResult> Index(Int64 id)
    {
        try
        {
            if (IsAuthenticated() == false)
            {
                return RedirectToAction("Create", "Login");
            }

            var data = await _reasonListService.GetAllReasonListAsync();

            if (data != null && data.Any())
            {
                var dataModel = _mapper.Map<List<ReasonListModel>>(data);

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
    public async Task<IActionResult> Create([FromForm] ReasonListModel model)
    {
        if (IsAuthenticated() == false)
        {
            return RedirectToAction("Create", "Login");
        }

        if (ModelState.IsValid)
        {
            var data = _mapper.Map<ReasonList>(model);
            await _reasonListService.CreateReasonListAsync(data);

            return RedirectToAction("Index");
        }
        return View(model);
    }

    [HttpGet]
    //[CustomAuthorize]
    public async Task<IActionResult> Edit(string id, string classificationListId)
    {
        if (IsAuthenticated() == false)
        {
            return RedirectToAction("Create", "Login");
        }

        var data = await _reasonListService.GetReasonListAsync(long.Parse(id));

        if (data != null)
        {
            var model = _mapper.Map<ReasonListModel>(data);
            return View(model);
        }

        return View();
    }

    [HttpPost]
    //[CustomAuthorize]
    public async Task<IActionResult> Edit([FromForm] ReasonListModel model)
    {
        if (IsAuthenticated() == false)
        {
            return RedirectToAction("Create", "Login");
        }

        if (ModelState.IsValid)
        {
            var data = _mapper.Map<ReasonList>(model);
            await _reasonListService.UpdateReasonListAsync(data);

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

        var data = await _reasonListService.GetReasonListAsync(long.Parse(id));

        if (data != null)
        {
            var model = _mapper.Map<ReasonListModel>(data);
            return View(model);
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
