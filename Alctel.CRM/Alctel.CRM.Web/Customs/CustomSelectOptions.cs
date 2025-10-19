using Alctel.CRM.Business.Interfaces;
using Alctel.CRM.Business.Services;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.DependencyInjection;

namespace Alctel.CRM.Web.Customs;

public static class CustomSelectOptions
{
    private static IServiceCollection services = new ServiceCollection();
    private static IServiceProvider serviceProvider;
    public static async Task<IEnumerable<SelectListItem>> GetActionPermissionsAsync()
    {
        List<SelectListItem> selectListItems = new List<SelectListItem>();
        try
        {
            services.AddTransient<IActionPermissionService, ActionPermissionService>();
            serviceProvider = services.BuildServiceProvider();

            var actionPermissionService = serviceProvider.GetService<IActionPermissionService>();
            var actionPermissionList = await actionPermissionService.GetAllActionPermissionAsync();

            if (actionPermissionList != null && actionPermissionList.Any())
            {
                foreach (var item in actionPermissionList)
                {
                    selectListItems.Add(new SelectListItem
                    {
                        Text = item.Name,
                        Value = item.Id.ToString(),
                    });
                }
            }

        }
        catch (Exception ex) { }

        return selectListItems;
    }
}
