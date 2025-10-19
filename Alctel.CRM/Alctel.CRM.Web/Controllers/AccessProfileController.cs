using Alctel.CRM.Business.Interfaces;
using Alctel.CRM.Business.Services;
using Alctel.CRM.Context.InMemory.Entities;
using Alctel.CRM.Web.Customs;
using Alctel.CRM.Web.Models;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Alctel.CRM.Web.Controllers;


public class AccessProfileController : Controller
{
    private readonly IMapper _mapper;
    private readonly IAccessProfileService _accessProfileService;

    public AccessProfileController(IMapper mapper, IAccessProfileService accessProfileService)
    {
        _mapper = mapper;
        _accessProfileService = accessProfileService;
    }

    [HttpGet]
    //[CustomAuthorize]
    public async Task<IActionResult> Index()
    {
        try
        {
            //var accessProfiles = await _accessProfileService.GetAllAccessProfileAsync();
            var accessProfiles = await _accessProfileService.GetAllAccessProfileAPIAsync();

            if (accessProfiles != null && accessProfiles.Any())
            {
                var AccessProfilesModel = _mapper.Map<List<AccessProfileModel>>(accessProfiles);

                return View(AccessProfilesModel);
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
    public async Task<IActionResult> Create([FromForm] AccessProfileModel model)
    {
        if (ModelState.IsValid)
        {
            var accessProfile = _mapper.Map<AccessProfile>(model);
            await _accessProfileService.CreateAccessProfileAsync(accessProfile);

            return RedirectToAction("Index");
        }
        return View(model);
    }

    [HttpGet]
    //[CustomAuthorize]
    public async Task<IActionResult> Edit(string id)
    {
        var accessProfile = await _accessProfileService.GetAccessProfileAsync(long.Parse(id));

        if (accessProfile != null)
        {
            var model = _mapper.Map<AccessProfileModel>(accessProfile);
            return View(model);
        }

        return View();
    }

    [HttpPost]
    //[CustomAuthorize]
    public async Task<IActionResult> Edit([FromForm] AccessProfileModel model)
    {
        if (ModelState.IsValid)
        {
            var accessProfile = _mapper.Map<AccessProfile>(model);
            await _accessProfileService.UpdateAccessProfileAsync(accessProfile);

            return RedirectToAction("Index");
        }
        return View(model);
    }

    [HttpGet]
    //[CustomAuthorize]
    public async Task<IActionResult> Delete(string id)
    {
        var accessProfile = await _accessProfileService.GetAccessProfileAsync(long.Parse(id));

        if (accessProfile != null)
        {
            var model = _mapper.Map<AccessProfileModel>(accessProfile);
            return View(model);
        }

        return View();
    }

    [HttpPost]
    //[CustomAuthorize]
    public async Task<IActionResult> Delete([FromForm] AccessProfileModel model)
    {
        if (ModelState.IsValid)
        {
            var accessProfile = _mapper.Map<AccessProfile>(model);
            await _accessProfileService.DeleteAccessProfileAsync(accessProfile);

            return RedirectToAction("Index");
        }
        return View(model);
    }    
}

