using Alctel.CRM.Business.Interfaces;
using Alctel.CRM.Web.Models;
using Alctel.CRM.Web.Models.Classification;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace Alctel.CRM.Web.Controllers;

public class ClassificationDemandTypeController : Controller
{
    private readonly IClassificationDemandTypeService _classificationDemandTypeService;
    private readonly IMapper _mapper;

    public ClassificationDemandTypeController(IMapper mapper, IClassificationDemandTypeService classificationDemandTypeService)
    {
        _mapper = mapper;
        _classificationDemandTypeService = classificationDemandTypeService;
    }

    [HttpGet]
    public async Task<List<ClassificationDemandTypeModel>> GetClassificationDemandTypeList(Int64 id)
    {
        var model = new List<ClassificationDemandTypeModel>();
        try
        {
            var classificationDemandList = await _classificationDemandTypeService.GetClassificationDemandTypeListAPIAsync(id);

            if (classificationDemandList != null)
            {

                var classificationDemandModelList = _mapper.Map<List<ClassificationDemandTypeModel>>(classificationDemandList);

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
