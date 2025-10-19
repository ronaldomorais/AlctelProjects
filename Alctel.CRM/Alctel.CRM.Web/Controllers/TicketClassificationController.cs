using Alctel.CRM.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Alctel.CRM.Web.Controllers;

class SelectIOptions
{
    public Int64 Id { get; set; }
    public string? Name { get; set; }
}




public class TicketClassificationController : Controller
{
    private List<SelectIOptions> selectIOptions = new List<SelectIOptions>();

    public TicketClassificationController()
    {
        selectIOptions.Add(new SelectIOptions
            {
                Id = 1,
                Name = "Opção 01"
            });

        selectIOptions.Add(new SelectIOptions
        {
            Id = 2,
            Name = "Opção 02"
        });

        selectIOptions.Add(new SelectIOptions
        {
            Id = 3,
            Name = "Opção 03"
        });

    }

    [HttpGet]
    public IActionResult ScreenPopup()
    {
        TicketClassificationModel model = new TicketClassificationModel();
        LoadManifestationTypeOptions(model);
        LoadServiceUnitOptions(model);
        LoadServiceOptions(model);
        LoadReason01Options(model);
        LoadReason02Options(model);
        return View(model);
    }

    private void LoadManifestationTypeOptions(TicketClassificationModel model, Int64 selectedItem = 0)
    {
        bool isSelected = false;

        model.ManifestationTypeOptions.Add(new SelectListItem
        {
            Value = "0",
            Text = "Opções",
            Selected = isSelected
        });

        foreach (var item in selectIOptions)
        {
            isSelected = false;
            if (item.Id == selectedItem)
            {
                isSelected = true;
            }

            model.ManifestationTypeOptions.Add(new SelectListItem
            {
                Value = item.Id.ToString(),
                Text = item.Name,
                Selected = isSelected
            });
        }
    }

    private void LoadServiceUnitOptions(TicketClassificationModel model, Int64 selectedItem = 0)
    {
        bool isSelected = false;

        model.ServiceUnitOptions.Add(new SelectListItem
        {
            Value = "0",
            Text = "Opções",
            Selected = isSelected
        });

        foreach (var item in selectIOptions)
        {
            isSelected = false;
            if (item.Id == selectedItem)
            {
                isSelected = true;
            }

            model.ServiceUnitOptions.Add(new SelectListItem
            {
                Value = item.Id.ToString(),
                Text = item.Name,
                Selected = isSelected
            });
        }
    }

    private void LoadServiceOptions(TicketClassificationModel model, Int64 selectedItem = 0)
    {
        bool isSelected = false;

        model.ServiceOptions.Add(new SelectListItem
        {
            Value = "0",
            Text = "Opções",
            Selected = isSelected
        });

        foreach (var item in selectIOptions)
        {
            isSelected = false;
            if (item.Id == selectedItem)
            {
                isSelected = true;
            }

            model.ServiceOptions.Add(new SelectListItem
            {
                Value = item.Id.ToString(),
                Text = item.Name,
                Selected = isSelected
            });
        }
    }

    private void LoadReason01Options(TicketClassificationModel model, Int64 selectedItem = 0)
    {
        bool isSelected = false;

        model.Reason01Options.Add(new SelectListItem
        {
            Value = "0",
            Text = "Opções",
            Selected = isSelected
        });

        foreach (var item in selectIOptions)
        {
            isSelected = false;
            if (item.Id == selectedItem)
            {
                isSelected = true;
            }

            model.Reason01Options.Add(new SelectListItem
            {
                Value = item.Id.ToString(),
                Text = item.Name,
                Selected = isSelected
            });
        }
    }

    private void LoadReason02Options(TicketClassificationModel model, Int64 selectedItem = 0)
    {
        bool isSelected = false;

        model.Reason02Options.Add(new SelectListItem
        {
            Value = "0",
            Text = "Opções",
            Selected = isSelected
        });

        foreach (var item in selectIOptions)
        {
            isSelected = false;
            if (item.Id == selectedItem)
            {
                isSelected = true;
            }

            model.Reason02Options.Add(new SelectListItem
            {
                Value = item.Id.ToString(),
                Text = item.Name,
                Selected = isSelected
            });
        }
    }
}
