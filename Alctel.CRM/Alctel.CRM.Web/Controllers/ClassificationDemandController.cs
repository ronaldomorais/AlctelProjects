using Alctel.CRM.Business.Interfaces;
using Alctel.CRM.Web.Models;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Alctel.CRM.Web.Controllers;

public class ClassificationDemandController : Controller
{
    private readonly IClassificationDemandService _classificationDemandService;
    private readonly IMapper _mapper;

    public ClassificationDemandController(IMapper mapper, IClassificationDemandService classificationDemandService)
    {
        _mapper = mapper;
        _classificationDemandService = classificationDemandService;
    }

    [HttpGet]
    public async Task<List<ClassificationDemandModel>> GetClassificationDemandList()
    {

        var model = new List<ClassificationDemandModel>();
        try
        {
            var classificationDemandList = await _classificationDemandService.GetClassficationDemandListAPIAsync();

            if (classificationDemandList != null)
            {
                
                var classificationDemandModelList = _mapper.Map<List<ClassificationDemandModel>>(classificationDemandList);
                
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
