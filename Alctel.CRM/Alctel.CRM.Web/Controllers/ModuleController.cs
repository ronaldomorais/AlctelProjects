using System.Collections.Generic;
using Alctel.CRM.Business.Interfaces;
using Alctel.CRM.Business.Services;
using Alctel.CRM.Context.InMemory.Entities;
using Alctel.CRM.Web.Customs;
using Alctel.CRM.Web.Models;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Alctel.CRM.Web.Controllers;


public class ModuleController : Controller
{
    private readonly IMapper _mapper;
    private readonly IModuleService _moduleService;
    private readonly IActionPermissionService _actionPermissionService;

    public ModuleController(IMapper mapper, IModuleService moduleService, IActionPermissionService actionPermissionService)
    {
        _mapper = mapper;
        _moduleService = moduleService;
        _actionPermissionService = actionPermissionService;
    }

    [HttpGet]
    //[CustomAuthorize]
    public async Task<IActionResult> Index()
    {
        try
        {
            var Modules = await _moduleService.GetAllModuleAsync();

            if (Modules != null && Modules.Any())
            {
                var ModulesModel = _mapper.Map<List<ModuleModel>>(Modules);

                return View(ModulesModel);
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
    public async Task<IActionResult> Create()
    {
        try
        {
            var actionPermissionList = await _actionPermissionService.GetAllActionPermissionAsync();

            var actionPermissionModelList = _mapper.Map<List<ActionPermissionModel>>(actionPermissionList);

            //ModuleModel model = new ModuleModel();
            //model.Name = string.Empty;
            //model.Description = string.Empty;
            //model.ActionPermissionModels = new List<ActionPermissionModel>();
            //model.ActionPermissionModels.AddRange(actionPermissionModelList);

            //return View(model);

            if (actionPermissionModelList != null && actionPermissionModelList.Any())
            {
                var selectListItem = new List<SelectListItem>();
                foreach (var item in actionPermissionModelList)
                {
                    selectListItem.Add(new SelectListItem
                    {
                        Text = item.Name,
                        Value = item.Id.ToString()
                    });
                }

                ViewBag.ActionPermissionOption = selectListItem;
            }


        }
        catch (Exception ex)
        {
            Console.WriteLine($"Exception: {ex.Message}. Trace: {ex.StackTrace}");
        }
        return View();
    }

    [HttpPost]
    //[CustomAuthorize]
    public async Task<IActionResult> Create([FromForm] ModuleModel model)
    {
        if (ModelState.IsValid)
        {
            model.ActionPermissions = new List<ActionPermissionModel>();
            //if (model.ActionPermissionSelected != null)
            //{
            //    foreach (var item in model.ActionPermissionSelected)
            //    {
            //        if (string.IsNullOrEmpty(item) == false)
            //        {
            //            var actionPermition = await _actionPermissionService.GetActionPermissionAsync(Guid.Parse(item));

            //            var actionPermitionModel = _mapper.Map<ActionPermissionModel>(actionPermition);

            //            model.ActionPermissions.Add(actionPermitionModel);
            //        }
            //    }

            //    var module = _mapper.Map<Module>(model);
            //    await _moduleService.CreateModuleAsync(module);

            //    return RedirectToAction("Index");
            //}
        }
        return View(model);
    }

    [HttpGet]
    //[CustomAuthorize]
    public async Task<IActionResult> Edit(string id)
    {
        var module = await _moduleService.GetModuleAsync(long.Parse(id));

        if (module != null)
        {
            var model = _mapper.Map<ModuleModel>(module);
            return View(model);
        }

        return View();
    }

    [HttpPost]
    //[CustomAuthorize]
    public async Task<IActionResult> Edit([FromForm] ModuleModel model)
    {
        if (ModelState.IsValid)
        {
            var module = _mapper.Map<Module>(model);
            await _moduleService.UpdateModuleAsync(module);

            return RedirectToAction("Index");
        }
        return View(model);
    }

    [HttpGet]
    //[CustomAuthorize]
    public async Task<IActionResult> Delete(string id)
    {
        var module = await _moduleService.GetModuleAsync(long.Parse(id));

        if (module != null)
        {
            var model = _mapper.Map<ModuleModel>(module);
            return View(model);
        }

        return View();
    }

    [HttpPost]
    //[CustomAuthorize]
    public async Task<IActionResult> Delete([FromForm] ModuleModel model)
    {
        if (ModelState.IsValid)
        {
            var module = _mapper.Map<Module>(model);
            await _moduleService.DeleteModuleAsync(module);

            return RedirectToAction("Index");
        }
        return View(model);
    }

    private bool IsAuthenticated()
    {
        bool userAuthenticated = false;
        try
        {
            string? isAuthenticated = HttpContext.Session.GetString("isAuthenticated");

            if (isAuthenticated != null && isAuthenticated == "true")
            {
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

