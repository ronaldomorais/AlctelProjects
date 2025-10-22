using Alctel.CRM.Web.Models;
using Microsoft.AspNetCore.Mvc;

namespace Alctel.CRM.Web.Controllers;

public class SlaAlertController : Controller
{
    public IActionResult Index()
    {
        return View();
    }

    [HttpGet]
    public IActionResult AgendaIndex()
    {
        return View();
    }

    [HttpGet]
    public IActionResult AgendaCreate()
    {
        return View();
    }

    [HttpPost]
    public IActionResult AgendaCreate([FromForm] SlaAlertAgendaModel model)
    {
        return View(model);
    }
}
