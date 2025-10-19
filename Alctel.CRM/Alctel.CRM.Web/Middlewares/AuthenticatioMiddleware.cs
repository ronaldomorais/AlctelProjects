using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;

namespace Alctel.CRM.Web.Middlewares;

public class AuthenticationMiddleware
{
    private readonly RequestDelegate _next;

    public AuthenticationMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext httpContext)
    {
        var user = httpContext.Session.GetString("username");
        if (user != null)
        {
            //var identity = new ClaimsIdentity(new[]
            //{
            //    new Claim(ClaimTypes.Name, username),
            //    new Claim(ClaimTypes.Role, "Admin")
            //},
            //"custom");
            //httpContext.User = new ClaimsPrincipal(identity);

            var isAuthenticated = httpContext.Session.GetString("IsAuthenticated");

            if (isAuthenticated == null)
            {
                if (httpContext.User.Identity.IsAuthenticated == false)
                {
                    var identity = new ClaimsIdentity(new[]
                    {
                        new Claim(ClaimTypes.Name,"ronaldo.morais@alctel.com.br"),
                        new Claim("Profile", "Admin"),
                        new Claim("Module", "Home,Customer,User"),
                        new Claim("Action", "Index,Create,Edit,Delete")
                        //new Claim("Action", "Create,Edit,Delete")
                    }, CookieAuthenticationDefaults.AuthenticationScheme);

                    var principal = new ClaimsPrincipal(identity!);
                    var perfil = principal.IsInRole("User");
                    var login = httpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

                    httpContext.User = new ClaimsPrincipal(identity);
                    httpContext.Session.SetString("IsAuthenticated", "true");
                    httpContext.Response.Redirect("/Home/Index");
                }
            }
        }
        else
        {
            httpContext.Response.Redirect("/Login/Create");
        }

        await _next(httpContext);
    }
}
