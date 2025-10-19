using Alctel.CRM.Business.Interfaces;
using Alctel.CRM.Business.Services;
using Alctel.CRM.Context.InMemory.Entities;
using Alctel.CRM.Context.InMemory.Interfaces;
using Alctel.CRM.Context.InMemory.Repositories;
using Alctel.CRM.Web.Customs;
using Alctel.CRM.Web.Models;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Alctel.CRM.Web.Controllers;


public class ActionPermissionController : Controller
{
    private readonly IMapper _mapper;
    private readonly IActionPermissionService _actionPermissionService;

    public ActionPermissionController(IMapper mapper, IActionPermissionService actionPermissionService)
    {
        _mapper = mapper;
        _actionPermissionService = actionPermissionService;
    }

    [HttpGet]
    //[CustomAuthorize]
    public async Task<IActionResult> Index()
    {
        try
        {
            var actionPermissions = await _actionPermissionService.GetAllActionPermissionAsync();

            if (actionPermissions != null && actionPermissions.Any())
            {
                var ActionPermissionsModel = _mapper.Map<List<ActionPermissionModel>>(actionPermissions);

                return View(ActionPermissionsModel);
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

        return View();
    }

    [HttpPost]
    //[CustomAuthorize]
    public async Task<IActionResult> Create([FromForm] ActionPermissionModel model)
    {
        if (ModelState.IsValid)
        {
            var actionPermission = _mapper.Map<ActionPermission>(model);            
            await _actionPermissionService.CreateActionPermissionAsync(actionPermission);

            return RedirectToAction("Index");
        }
        return View(model);
    }

    [HttpGet]
    //[CustomAuthorize]
    public async Task<IActionResult> Edit(string id)
    {
        var actionPermission = await _actionPermissionService.GetActionPermissionAsync(long.Parse(id));

        if (actionPermission != null)
        {
            var model = _mapper.Map<ActionPermissionModel>(actionPermission);
            return View(model);
        }

        return View();
    }

    [HttpPost]
    //[CustomAuthorize]
    public async Task<IActionResult> Edit([FromForm] ActionPermissionModel model)
    {
        if (ModelState.IsValid)
        {
            var actionPermission = _mapper.Map<ActionPermission>(model);
            await _actionPermissionService.UpdateActionPermissionAsync(actionPermission);

            return RedirectToAction("Index");
        }
        return View(model);
    }

    [HttpGet]
    //[CustomAuthorize]
    public async Task<IActionResult> Delete(string id)
    {
        var actionPermission = await _actionPermissionService.GetActionPermissionAsync(long.Parse(id));

        if (actionPermission != null)
        {
            var model = _mapper.Map<ActionPermissionModel>(actionPermission);
            return View(model);
        }

        return View();
    }

    [HttpPost]
    //[CustomAuthorize]
    public async Task<IActionResult> Delete([FromForm] ActionPermissionModel model)
    {
        if (ModelState.IsValid)
        {
            var actionPermission = _mapper.Map<ActionPermission>(model);
            await _actionPermissionService.DeleteActionPermissionAsync(actionPermission);

            return RedirectToAction("Index");
        }
        return View(model);
    }
}

