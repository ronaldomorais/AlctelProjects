using System.Dynamic;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;
using Alctel.CRM.Business.Interfaces;
using Alctel.CRM.Business.Services;
using Alctel.CRM.Context.InMemory.Entities;
using Alctel.CRM.Web.Models;
using AutoMapper;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Alctel.CRM.Web.Controllers;

public class LoginController : Controller
{
    private readonly ILoginService _loginService;
    private readonly ITicketService _ticketService;
    private readonly IUserService _userService;
    private readonly IWebHostEnvironment _hostingEnvironment;
    private readonly IConfigService _configService;

    public LoginController(ILoginService loginService, ITicketService ticketService, IUserService userService, IWebHostEnvironment hostingEnvironment, IConfigService configService)
    {
        _loginService = loginService;
        _ticketService = ticketService;
        _userService = userService;
        _hostingEnvironment = hostingEnvironment;
        _configService = configService;
    }

    [AllowAnonymous]
    public IActionResult Index()
    {
        return RedirectToAction("Create");
    }

    [HttpGet]
    [AllowAnonymous]
    public IActionResult Create()
    {
        //HttpContext.Session.Clear();
        return View();
    }

    //[HttpPost]
    //[AllowAnonymous]
    //public async Task<IActionResult> Create([FromForm] LoginModel model)
    //{
    //    if (ModelState.IsValid)
    //    {
    //        var isAuthenticated = HttpContext.User.Identity!.IsAuthenticated;

    //        ClaimsIdentity? identity = null;

    //        if (isAuthenticated == false)
    //        {
    //            bool loginValid = false;

    //            if (model.Username != null)
    //            {
    //                var logininfo = await _loginService.GetLoginPIAsync(model.Username);

    //                if (logininfo.UserStatus && logininfo.ProfileStatus)
    //                {
    //                    string profile = logininfo.Profile != null ? logininfo.Profile : string.Empty;
    //                    List<Claim> claims = new List<Claim>();

    //                    claims.Add(new Claim(ClaimTypes.Name, model.Username));
    //                    claims.Add(new Claim("Profile", profile));

    //                    switch (profile.ToUpper())
    //                    {
    //                        case "ADMINISTRADOR":
    //                            var moduleAdminClaim = new Claim("Module", "Home,Customer,User,Ticket,ServiceUnit,Area,ServiceLevel,DemandType,Configuration,ClassificationList,ReasonList,ClassificationTree");

    //                            claims.Add(moduleAdminClaim);
    //                            break;
    //                        case "AGENTE":
    //                            var moduleAgentClaim = new Claim("Module", "Home,Customer,Ticket");

    //                            claims.Add(moduleAgentClaim);
    //                            break;
    //                        case "ASSISTENTE":
    //                            var moduleAssistentClaim = new Claim("Module", "Home,Customer,Ticket");

    //                            claims.Add(moduleAssistentClaim);
    //                            break;
    //                        case "MONITOR":
    //                            var moduleMonitorClaim = new Claim("Module", "Home,Customer,Ticket");

    //                            claims.Add(moduleMonitorClaim);
    //                            break;
    //                    }

    //                    identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

    //                    loginValid = true;
    //                }


    //                if (loginValid)
    //                {
    //                    var principal = new ClaimsPrincipal(identity!);
    //                    var perfil = principal.IsInRole("User");
    //                    var login = HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
    //                    return RedirectToAction("Index", "Home");
    //                }
    //            }
    //        }
    //    }

    //    ModelState.AddModelError("", "Falha ao realizar o login!!");
    //    return View();
    //}

    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> Create([FromForm] LoginModel model)
    {
        try
        {
            if (ModelState.IsValid)
            {
                var loginuser = model.Username;

                string? isAuthenticated = HttpContext.Session.GetString("isAuthenticated");

                if (isAuthenticated == null)
                {
                    if (loginuser != null)
                    {
                        var logininfo = await _loginService.GetLoginPIAsync(loginuser);

                        if (logininfo.UserStatus && logininfo.ProfileStatus)
                        {
                            string profile = logininfo.Profile != null ? logininfo.Profile : string.Empty;
                            string username = logininfo.UserName != null ? logininfo.UserName : string.Empty;
                            Int64 userid = logininfo.UserId;

                            var userData = await _userService.GetUserAPIAsync(userid);

                            if (userData != null && userData.QueueGTId != null)
                            {
                                HttpContext.Session.SetString("UserServiceLevel", userData.QueueGTId);                                
                            }


                            HttpContext.Session.SetString("Profile", profile);
                            HttpContext.Session.SetString("Username", username);
                            HttpContext.Session.SetString("LoginUser", loginuser);
                            HttpContext.Session.SetString("UserId", userid.ToString());

                            string physicalPath = _hostingEnvironment.WebRootPath;
                            string baseUrl = _configService.GetBaseUrl(physicalPath);
                            HttpContext.Session.SetString("BaseUrl", baseUrl);

                            //List<Claim> claims = new List<Claim>();

                            //claims.Add(new Claim(ClaimTypes.Name, logininfo.UserName));
                            //ClaimsIdentity identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                            //var login = HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identity));

                            switch (profile.ToUpper())
                            {
                                case "ADMINISTRADOR":
                                    HttpContext.Session.SetString("Module", "Home,Customer,User,Ticket,ServiceUnit,Area,ServiceLevel,DemandType,Configuration,ClassificationList,ReasonList,ClassificationTree,TicketAssignment,TicketClassification,TicketClassificationList,TicketClassificationListItem,TicketClassificationManifestationType,SlaAlert");
                                    break;
                                case "AGENTE":
                                    HttpContext.Session.SetString("Module", "Home,Customer,Ticket,TicketAssignment");
                                    break;
                                case "ASSISTENTE":
                                    HttpContext.Session.SetString("Module", "Home,Customer,Ticket,TicketAssignment");
                                    break;
                                case "MONITOR":
                                    HttpContext.Session.SetString("Module", "Home,Customer,Ticket,TicketAssignment");
                                    break;
                            }

                            HttpContext.Session.SetString("isAuthenticated", "true");

                            return RedirectToAction("Index", "Home");
                        }
                    }
                }
                else
                {
                    return RedirectToAction("Index", "Home");
                }
            }
        }
        catch (Exception ex)
        {
        }

        return RedirectToAction("Logout", "Login");
    }


    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> ImplicitLogin([FromForm] LoginModel model)
    {
        dynamic jsonResult = new ExpandoObject();
        jsonResult.isAuthenticated = false;
        jsonResult.StatusCode = "401";
        jsonResult.Message = "";

        try
        {
            if (ModelState.IsValid)
            {
                var loginuser = model.Username;
                var access_token = model.Password;

                string? isAuthenticated = HttpContext.Session.GetString("isAuthenticated");

                if (isAuthenticated == null && string.IsNullOrEmpty(access_token) == false)
                {
                    if (loginuser != null)
                    {
                        var logininfo = await _loginService.GetLoginPIAsync(loginuser);

                        if (logininfo.UserStatus && logininfo.ProfileStatus)
                        {
                            string profile = logininfo.Profile != null ? logininfo.Profile : string.Empty;
                            string username = logininfo.UserName != null ? logininfo.UserName : string.Empty;
                            Int64 userid = logininfo.UserId;

                            var userData = await _userService.GetUserAPIAsync(userid);

                            if (userData != null && userData.QueueGTId != null)
                            {
                                HttpContext.Session.SetString("UserServiceLevel", userData.QueueGTId);
                            }

                            HttpContext.Session.SetString("Profile", profile);
                            HttpContext.Session.SetString("Username", username);
                            HttpContext.Session.SetString("LoginUser", loginuser);
                            HttpContext.Session.SetString("UserId", userid.ToString());
                            HttpContext.Session.SetString("Access_Token", access_token);

                            string physicalPath = _hostingEnvironment.WebRootPath;
                            string baseUrl = _configService.GetBaseUrl(physicalPath);
                            HttpContext.Session.SetString("BaseUrl", baseUrl);
                            //List<Claim> claims = new List<Claim>();

                            //claims.Add(new Claim(ClaimTypes.Name, logininfo.UserName));
                            //ClaimsIdentity identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                            //var login = HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identity));

                            switch (profile.ToUpper())
                            {
                                case "ADMINISTRADOR":
                                    HttpContext.Session.SetString("Module", "Home,Customer,User,Ticket,ServiceUnit,Area,ServiceLevel,DemandType,Configuration,ClassificationList,ReasonList,ClassificationTree,TicketAssignment,TicketClassification,TicketClassificationList,TicketClassificationListItem,TicketClassificationManifestationType,SlaAlert");
                                    break;
                                case "AGENTE":
                                    HttpContext.Session.SetString("Module", "Home,Customer,Ticket,TicketAssignment");
                                    break;
                                case "ASSISTENTE":
                                    HttpContext.Session.SetString("Module", "Home,Customer,Ticket,TicketAssignment");
                                    break;
                                case "MONITOR":
                                    HttpContext.Session.SetString("Module", "Home,Customer,Ticket,TicketAssignment");
                                    break;
                            }

                            HttpContext.Session.SetString("isAuthenticated", "true");

                            jsonResult.isAuthenticated = true;
                            jsonResult.StatusCode = "200";
                        }
                    }
                }
                else
                {
                    jsonResult.isAuthenticated = true;
                    jsonResult.StatusCode = "200";

                    if (string.IsNullOrEmpty(access_token) == false)
                    {
                        HttpContext.Session.SetString("Access_Token", access_token);

                        jsonResult.RedirectTo = "/Ticket/GenesysInteractionEvent/?nomeFila=&conversastionid=&email=&cpf=&protocolo=&navegacao=&reload=true";

                    }
                }
            }
        }
        catch (Exception ex)
        {
            jsonResult.Message = ex.Message;
        }

        return new JsonResult(jsonResult);
    }

    [HttpGet]
    public bool CheckIfIsAuthenticated()
    {
        bool isAuthenticated = false;
        if (HttpContext.User.Identity != null)
        {
            isAuthenticated = HttpContext.User.Identity.IsAuthenticated;
        }

        return isAuthenticated;
    }

    [HttpGet]
    [AllowAnonymous]
    public IActionResult AccessDenied()
    {
        return View();
    }

    //[HttpGet]
    //[AllowAnonymous]
    //public IActionResult Logout()
    //{
    //    HttpContext.SignOutAsync(
    //    CookieAuthenticationDefaults.AuthenticationScheme);
    //    return RedirectToAction("Index", "Home");
    //}

    [HttpGet]
    [AllowAnonymous]
    public IActionResult Logout()
    {
        HttpContext.Session.Clear();
        //HttpContext.SignOutAsync(
        //CookieAuthenticationDefaults.AuthenticationScheme);
        return RedirectToAction("Index", "Home");
    }

    [HttpGet]
    [AllowAnonymous]
    public IActionResult GetSessionInfo()
    {
        dynamic sessionInfo = new ExpandoObject();
        sessionInfo.IsAuthenticated = false;

        try
        {
            var isAuthenticatedSession = HttpContext.Session.GetString("isAuthenticated");
            //var accessTokenSession = HttpContext.Session.GetString("Access_Token");
            //         var userSession = HttpContext.Session.GetString("Username");

            if (isAuthenticatedSession != null)
            {
                var accessTokenSession = HttpContext.Session.GetString("Access_Token");
                var userSession = HttpContext.Session.GetString("LoginUser");

                sessionInfo.IsAuthenticated = isAuthenticatedSession;
                sessionInfo.AccessToken = accessTokenSession;
                sessionInfo.Username = userSession;
            }
        }
        catch (Exception ex)
        {

        }

        return new JsonResult(sessionInfo);
    }
}
