using Alctel.CRM.Business.Interfaces;
using Alctel.CRM.Business.Services;
using Alctel.CRM.Web.Models.Classification;
using Microsoft.AspNetCore.Mvc;

namespace Alctel.CRM.Web.Controllers;

public class ConfigController : Controller
{
    private readonly IConfigService _configService;

    public ConfigController(IConfigService configService) => _configService = configService;


    [HttpGet]
    public string GetGenesysClientId()
    {
        return _configService.GetGenesysClientId();
    }

    [HttpGet]
    public string GetBaseUrl(string physicalPath)
    {
        return _configService.GetBaseUrl(physicalPath);
    }
}
