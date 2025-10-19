using System.Security.Claims;
using Alctel.CRM.Web.Tools.LogHelper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Serilog;

namespace Alctel.CRM.Web.Customs;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
public class CustomAuthorize : AuthorizeAttribute, IAuthorizationFilter
{

    public void OnAuthorization(AuthorizationFilterContext context)
    {
        Log.Logger = new LoggerConfiguration()
            .WriteTo.Console(Serilog.Events.LogEventLevel.Debug)
            .WriteTo.File($"C:\\temp\\Alctel.TicketManagement_.log", rollingInterval: RollingInterval.Hour, outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff} [{Level}] {Message}{NewLine}{Exception}")
            .WriteTo.EventLog(source: "Alctel.TicketManagement", logName: "Application", manageEventSource: false)
            .MinimumLevel.Debug()
            .Enrich.FromLogContext()
            .CreateLogger();

        Log.Information("OnAuthorization");
        string? isAuthenticated = context.HttpContext.Session.GetString("isAuthenticated");
        Log.Information($"isAuthenticated {isAuthenticated}");

        if (isAuthenticated != null && isAuthenticated == "true")
        {
            //var user = context.HttpContext.User;
            var routeData = context.RouteData;
            Log.Information($"RouteData");
            if (routeData.Values.FirstOrDefault().Value != null)
            {
                Log.Information($"RouteData Entrou");
                var controllername = routeData.Values.FirstOrDefault().Value!.ToString();
                var modules = context.HttpContext.Session.GetString("Module");
                Log.Information($"controllername: {controllername}");
                Log.Information($"modules: {modules}");


                if (modules != null && controllername != null)
                {
                    if (modules.Contains(controllername))
                    {
                        return;
                    }
                    else
                    {
                        context.Result = new RedirectResult("~/Login/AccessDenied");
                    }
                }
                else
                {
                    context.Result = new RedirectResult("~/Login/AccessDenied");
                }
            }

            return;
        }
        else
        {
            context.Result = new RedirectResult("~/Login/Logout");
        }

            //if (user != null)
            //{
            //    if (user.Identity!.IsAuthenticated)
            //    {
            //        var profile = user.FindFirstValue("Profile");
            //        var module = user.FindFirstValue("Module");
            //        var action = user.FindFirstValue("Action");

            //        //if (profile != null && action != null && module != null)
            //        if (profile != null)
            //        {
            //            if (profile.ToUpper() == "ADMINISTRADOR")
            //            {
            //                return;
            //            }

            //            //bool hasAction = routeData.Values.Any(_ => action.Contains(_.Value.ToString()));
            //            bool hasAction = true;
            //            bool hasModule = routeData.Values.Any(_ => module.Contains(_.Value.ToString()));
            //            if (hasAction && hasModule)
            //            {
            //                return;
            //            }

            //        }
            //    }
            //}

            //context.Result = new StatusCodeResult((int)System.Net.HttpStatusCode.Forbidden);
            //context.Result = new RedirectResult("~/Login/AccessDenied");
        return;
    }
}
