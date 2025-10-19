using Alctel.CRM.API.Entities;
using Alctel.CRM.Business.Interfaces;
using Alctel.CRM.Business.Services;
using Alctel.CRM.Context.InMemory.Entities;
using Alctel.CRM.Web.Customs;
using Alctel.CRM.Web.Models;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Alctel.CRM.Web.Controllers;

public class UserController : Controller
{
    private const string MODULE_NAME = "usuarios";
    private const int SIZE_PAGE = 10;

    private readonly IMapper _mapper;
    private readonly IUserService _userService;
    private readonly IAccessProfileService _accessProfileService;
    private readonly IServiceUnitService _unitService;
    private readonly IAreaService _areaService;
    private readonly ILogControllerService _logControllerService;
    private readonly ILoginService _loginService;
    private readonly ITicketService _ticketService;
    private readonly IWebHostEnvironment _hostingEnvironment;
    private readonly IConfigService _configService;

    public UserController(IMapper mapper, IUserService userService, IAccessProfileService accessProfileService, IServiceUnitService unitService, IAreaService areaService, ILogControllerService logControllerService, ILoginService loginService, ITicketService ticketService, IWebHostEnvironment hostingEnvironment, IConfigService configService)
    {
        _mapper = mapper;
        _userService = userService;
        _accessProfileService = accessProfileService;
        _unitService = unitService;
        _areaService = areaService;
        _logControllerService = logControllerService;
        _loginService = loginService;
        _ticketService = ticketService;
        _hostingEnvironment = hostingEnvironment;
        _configService = configService;
    }

    [HttpGet]
    //[CustomAuthorize]

    public async Task<IActionResult> Index()
    {
        try
        {
            if (IsAuthenticated() == false)
            {
                return RedirectToAction("Create", "Login");
            }

            string physicalPath = _hostingEnvironment.WebRootPath;
            ViewBag.BaseUrl = _configService.GetBaseUrl(physicalPath);

            //var users = await _userService.GetAllUserAsync();
            var users = await _userService.GetAllUsersAPIAsync();

            if (users != null && users.Any())
            {
                var usersModel = _mapper.Map<List<UserModel>>(users);
                return View(usersModel);
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
        if (IsAuthenticated() == false)
        {
            return RedirectToAction("Create", "Login");
        }

        return View();
    }

    [HttpPost]
    //[CustomAuthorize]
    public async Task<IActionResult> Create([FromForm] UserModel model)
    {
        if (IsAuthenticated() == false)
        {
            return RedirectToAction("Create", "Login");
        }

        if (ModelState.IsValid)
        {
            var user = _mapper.Map<User>(model);
            await _userService.CreateUserAsync(user);

            return RedirectToAction("Index");
        }
        return View(model);
    }

    [HttpGet]
    //[CustomAuthorize]
    public async Task<IActionResult> Edit(string id)
    {
        if (IsAuthenticated() == false)
        {
            return RedirectToAction("Create", "Login");
        }

        string physicalPath = _hostingEnvironment.WebRootPath;
        ViewBag.BaseUrl = _configService.GetBaseUrl(physicalPath);

        //var user = await _userService.GetUserAsync(long.Parse(id));
        var user = await _userService.GetUserAPIAsync(long.Parse(id));

        if (user != null)
        {
            var model = _mapper.Map<UserModel>(user);

            model.UserDataToCompareIfChanged = new UserDataToCompareIfChangedLog();

            model.UserDataToCompareIfChanged.Id = model.Id;
            model.UserDataToCompareIfChanged.Active = model.Active;
            model.UserDataToCompareIfChanged.Area = model.Area;
            model.UserDataToCompareIfChanged.Unit = model.Unit;
            model.UserDataToCompareIfChanged.AccessProfile = model.AccessProfile;
            model.UserDataToCompareIfChanged.QueueGTId = model.QueueGTId;

            var accessprofiles = await _accessProfileService.GetAccessProfileActivatedListAPIAsync();

            if (accessprofiles != null && accessprofiles.Any())
            {
                model.AccessProfileOptions = new List<Microsoft.AspNetCore.Mvc.Rendering.SelectListItem>();

                foreach (var accessprofile in accessprofiles)
                {
                    var selectListItem = new SelectListItem
                    {
                        //Value = accessprofile.Id.ToString(),
                        Value = accessprofile.Name,
                        Text = accessprofile.Name,
                    };

                    model.AccessProfileOptions.Add(selectListItem);
                }
            }

            var unitServices = await _unitService.GetServiceUnitActivatedListAPIAsync();

            if (unitServices != null && unitServices.Any())
            {
                model.UnitServiceOptions = new List<Microsoft.AspNetCore.Mvc.Rendering.SelectListItem>();

                foreach (var unitService in unitServices)
                {
                    var selectListItem = new SelectListItem
                    {
                        //Value = accessprofile.Id.ToString(),
                        Value = unitService.Name,
                        Text = unitService.Name,
                    };

                    model.UnitServiceOptions.Add(selectListItem);
                }
            }

            var areaServices = await _areaService.GetAreaActivatedListAPIAsync();

            if (areaServices != null && areaServices.Any())
            {
                model.AreaOptions = new List<Microsoft.AspNetCore.Mvc.Rendering.SelectListItem>();

                foreach (var areaService in areaServices)
                {
                    var selectListItem = new SelectListItem
                    {
                        //Value = accessprofile.Id.ToString(),
                        Value = areaService.Name,
                        Text = areaService.Name,
                    };

                    model.AreaOptions.Add(selectListItem);
                }
            }

            var ticketServices = await _ticketService.GetTicketQueueGTAPIAsync();

            if (ticketServices != null && ticketServices.Any())
            {
                model.QueueGTOptions = new List<Microsoft.AspNetCore.Mvc.Rendering.SelectListItem>();

                foreach (var ticketService in ticketServices)
                {
                    var selectListItem = new SelectListItem
                    {
                        //Value = accessprofile.Id.ToString(),
                        Value = ticketService.Id.ToString(),
                        Text = ticketService.Name,
                    };

                    model.QueueGTOptions.Add(selectListItem);
                }
            }

            return View(model);
        }

        return View();
    }

    [HttpPost]
    //[CustomAuthorize]
    public async Task<IActionResult> Edit([FromForm] UserModel model)
    {
        if (IsAuthenticated() == false)
        {
            return RedirectToAction("Create", "Login");
        }

        string physicalPath = _hostingEnvironment.WebRootPath;
        ViewBag.BaseUrl = _configService.GetBaseUrl(physicalPath);

        if (ModelState.IsValid)
        {
            var username = HttpContext.Session.GetString("Username");
            var userid = HttpContext.Session.GetString("UserId");
            var user = _mapper.Map<UserAPI>(model);

            var ticketServices = await _ticketService.GetTicketQueueGTAPIAsync();

            if (ticketServices != null && ticketServices.Count > 0)
            {
                var queueGT = ticketServices.FirstOrDefault(_ => _.Id == model.QueueGTId);

                if (queueGT != null)
                {
                    user.QueueGT = queueGT.Name;
                }
            }

            //await _userService.UpdateUserAsync(user);
            var ret = await _userService.UpdateUserAPIAsync(user);

            if (ret)
            {
                //Log
                if (model.Active != model.UserDataToCompareIfChanged.Active)
                {
                    LogController logController = new LogController();
                    logController.Id = model.Id;
                    logController.Module = MODULE_NAME;
                    logController.Section = username == null ? string.Empty : username;
                    logController.Field = "Ativar/Desativar";
                    logController.Value = model.UserDataToCompareIfChanged.Active ? "Ativado" : "Desativado";
                    logController.UserId = userid != null ? Int64.Parse(userid) : 0;
                    logController.Action = "Editar";

                    await _logControllerService.InsertLogAPIAsync(logController);
                }

                if (model.AccessProfile != model.UserDataToCompareIfChanged.AccessProfile)
                {
                    LogController logController = new LogController();
                    logController.Id = model.Id;
                    logController.Module = MODULE_NAME;
                    logController.Section = username == null ? string.Empty : username;
                    logController.Field = "Perfil";
                    logController.Value = model.UserDataToCompareIfChanged.AccessProfile;
                    logController.UserId = userid != null ? Int64.Parse(userid) : 0;
                    logController.Action = "Editar";

                    await _logControllerService.InsertLogAPIAsync(logController);
                }

                if (model.Unit != model.UserDataToCompareIfChanged.Unit)
                {
                    LogController logController = new LogController();
                    logController.Id = model.Id;
                    logController.Module = MODULE_NAME;
                    logController.Section = username == null ? string.Empty : username;
                    logController.Field = "Unidade";
                    logController.Value = model.UserDataToCompareIfChanged.Unit;
                    logController.UserId = userid != null ? Int64.Parse(userid) : 0;
                    logController.Action = "Editar";

                    await _logControllerService.InsertLogAPIAsync(logController);
                }

                if (model.Area != model.UserDataToCompareIfChanged.Area)
                {
                    LogController logController = new LogController();
                    logController.Id = model.Id;
                    logController.Module = MODULE_NAME;
                    logController.Section = username == null ? string.Empty : username;
                    logController.Field = "Área";
                    logController.Value = model.UserDataToCompareIfChanged.Area;
                    logController.UserId = userid != null ? Int64.Parse(userid) : 0;
                    logController.Action = "Editar";

                    await _logControllerService.InsertLogAPIAsync(logController);
                }

                if (model.QueueGTId != model.UserDataToCompareIfChanged.QueueGTId)
                {
                    string queueGTName = string.Empty;
                    if (ticketServices != null && ticketServices.Count > 0)
                    {
                        var queueGT = ticketServices.FirstOrDefault(_ => _.Id == model.UserDataToCompareIfChanged.QueueGTId);

                        if (queueGT != null)
                        {
                            queueGTName = queueGT.Name ?? string.Empty;
                        }
                    }

                    LogController logController = new LogController();
                    logController.Id = model.Id;
                    logController.Module = MODULE_NAME;
                    logController.Section = username == null ? string.Empty : username;
                    logController.Field = "Nível Atendimento";
                    logController.Value = queueGTName;
                    logController.UserId = userid != null ? Int64.Parse(userid) : 0;
                    logController.Action = "Editar";

                    await _logControllerService.InsertLogAPIAsync(logController);
                }
            }

            return RedirectToAction("Index");
        }
        else
        {
            var accessprofiles = await _accessProfileService.GetAllAccessProfileAPIAsync();

            if (accessprofiles != null && accessprofiles.Any())
            {
                model.AccessProfileOptions = new List<Microsoft.AspNetCore.Mvc.Rendering.SelectListItem>();

                foreach (var accessprofile in accessprofiles)
                {
                    var selectListItem = new SelectListItem
                    {
                        //Value = accessprofile.Id.ToString(),
                        Value = accessprofile.Name,
                        Text = accessprofile.Name,
                    };

                    model.AccessProfileOptions.Add(selectListItem);
                }
            }

            var unitServices = await _unitService.GetServiceUnitActivatedListAPIAsync();

            if (unitServices != null && unitServices.Any())
            {
                model.UnitServiceOptions = new List<Microsoft.AspNetCore.Mvc.Rendering.SelectListItem>();

                foreach (var unitService in unitServices)
                {
                    var selectListItem = new SelectListItem
                    {
                        //Value = accessprofile.Id.ToString(),
                        Value = unitService.Name,
                        Text = unitService.Name,
                    };

                    model.UnitServiceOptions.Add(selectListItem);
                }
            }

            var areaServices = await _areaService.GetAreaActivatedListAPIAsync();

            if (areaServices != null && areaServices.Any())
            {
                model.AreaOptions = new List<Microsoft.AspNetCore.Mvc.Rendering.SelectListItem>();

                foreach (var areaService in areaServices)
                {
                    var selectListItem = new SelectListItem
                    {
                        //Value = accessprofile.Id.ToString(),
                        Value = areaService.Name,
                        Text = areaService.Name,
                    };

                    model.AreaOptions.Add(selectListItem);
                }
            }

            var ticketServices = await _ticketService.GetTicketQueueGTAPIAsync();

            if (ticketServices != null && ticketServices.Any())
            {
                model.QueueGTOptions = new List<Microsoft.AspNetCore.Mvc.Rendering.SelectListItem>();

                foreach (var ticketService in ticketServices)
                {
                    var selectListItem = new SelectListItem
                    {
                        //Value = accessprofile.Id.ToString(),
                        Value = ticketService.Id.ToString(),
                        Text = ticketService.Name,
                    };

                    model.QueueGTOptions.Add(selectListItem);
                }
            }
        }
        return View(model);
    }

    [HttpGet]
    //[CustomAuthorize]
    public async Task<IActionResult> Delete(string id)
    {
        if (IsAuthenticated() == false)
        {
            return RedirectToAction("Create", "Login");
        }

        var user = await _userService.GetUserAsync(long.Parse(id));

        if (user != null)
        {
            var model = _mapper.Map<UserModel>(user);
            return View(model);
        }

        return View();
    }

    [HttpPost]
    //[CustomAuthorize]
    public async Task<IActionResult> Delete([FromForm] UserModel model)
    {
        if (IsAuthenticated() == false)
        {
            return RedirectToAction("Create", "Login");
        }

        if (ModelState.IsValid)
        {
            var user = _mapper.Map<User>(model);
            await _userService.DeleteUserAsync(user);

            return RedirectToAction("Index");
        }
        return View(model);
    }

    [HttpPost]
    //[CustomAuthorize]

    public async Task<IActionResult> Index(string searchUserType, string searchUserText)
    {
        if (IsAuthenticated() == false)
        {
            return RedirectToAction("Create", "Login");
        }

        try
        {
            if (searchUserType.Contains("Tipo Filtro") || searchUserText == string.Empty)
            {
                ViewBag.AlertFilter = "SHOW";

                var allusers = await _userService.GetAllUsersAPIAsync();

                if (allusers != null && allusers.Any())
                {
                    var usersModel = _mapper.Map<List<UserModel>>(allusers);
                    return View(usersModel);
                }
            }


            if (searchUserType.Contains("telefone") || searchUserType.Contains("cpf") || searchUserType.Contains("cnpj") || searchUserType.Contains("matricula"))
            {
                searchUserText = string.Join("", searchUserText.Where(char.IsDigit).ToList());
            }

            //var users = await _userService.GetAllUserAsync();
            var users = await _userService.SearchUserAPIAsync(searchUserType, searchUserText);

            if (users != null && users.Any())
            {
                var usersModel = _mapper.Map<List<UserModel>>(users);
                return View(usersModel);
            }

        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
        }
        return View();
    }

    public async Task<IActionResult> Search(string searchUserType, string searchUserText)
    {
        if (IsAuthenticated() == false)
        {
            return RedirectToAction("Create", "Login");
        }

        string physicalPath = _hostingEnvironment.WebRootPath;
        ViewBag.BaseUrl = _configService.GetBaseUrl(physicalPath);

        try
        {
            if (searchUserType.Contains("Tipo Filtro") || searchUserText == string.Empty)
            {
                ViewBag.AlertFilter = "SHOW";

                var allusers = await _userService.GetAllUsersAPIAsync();

                if (allusers != null && allusers.Any())
                {
                    var usersModel = _mapper.Map<List<UserModel>>(allusers);
                    return View(usersModel);
                }
            }


            if (searchUserType.Contains("telefone") || searchUserType.Contains("cpf") || searchUserType.Contains("cnpj") || searchUserType.Contains("matricula"))
            {
                searchUserText = string.Join("", searchUserText.Where(char.IsDigit).ToList());
            }

            //var users = await _userService.GetAllUserAsync();
            var users = await _userService.SearchUserAPIAsync(searchUserType, searchUserText);

            if (users != null && users.Any())
            {
                var usersModel = _mapper.Map<List<UserModel>>(users);
                return View(usersModel);
            }

        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
        }
        return View();
    }

    private void CreateDataPagination(int currentpage, int linenumbers, int sizepage, int totalitems)
    {
        int uniquePagesNumber = 9;
        int startPageCounter = currentpage + 1;
        ViewBag.ShowPageControl = true;

        try
        {
            int lastpage = ((int)(totalitems / sizepage)) + 1;
            ViewBag.PreviousPage = (currentpage - 1) == 0 ? 1 : currentpage - 1;
            ViewBag.NextPage = (currentpage + 1) > lastpage ? lastpage : currentpage + 1;
            ViewBag.FirstPage = 1;
            ViewBag.LastPage = lastpage;
            ViewBag.CurrentPage = currentpage;

            var startIndex = (currentpage - 1) * sizepage + 1;
            var endIndex = Math.Min(currentpage * sizepage, totalitems);
            ViewBag.StartIndex = startIndex;
            ViewBag.InfoPage = $"Mostrando de {startIndex} até {endIndex} de {totalitems} registros";

            int pagesExceededNumber = ((startPageCounter + uniquePagesNumber) - lastpage);

            if (pagesExceededNumber > 0)
            {
                startPageCounter = lastpage - uniquePagesNumber;
            }

            List<int> previousFirstPages = new List<int>();
            for (int i = 0; i < uniquePagesNumber; i++)
            {
                previousFirstPages.Add(startPageCounter + i);
            }

            ViewBag.PreviousFirstPages = previousFirstPages;

            //List<int> nextLastPages = new List<int>();
            //for (int i = uniquePagesNumber; i > 0; i--)
            //{
            //    nextLastPages.Add(lastpage - i);
            //}

            //ViewBag.NextLastPages = nextLastPages;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
        }
    }

    private bool IsAuthenticated()
    {
        bool userAuthenticated = false;
        try
        {
            string? isAuthenticated = HttpContext.Session.GetString("isAuthenticated");

            if (isAuthenticated != null && isAuthenticated == "true")
            {
                string? loginUser = HttpContext.Session.GetString("LoginUser");

                if (loginUser != null)
                {
                    string? profilesession = HttpContext.Session.GetString("Profile");

                    if (profilesession != null)
                    {
                        var logininfo = _loginService.GetLoginPIAsync(loginUser).Result;

                        if (logininfo != null)
                        {
                            string profile = logininfo.Profile != null ? logininfo.Profile : string.Empty;
                            if (profilesession != profile)
                            {
                                HttpContext.Session.Clear();
                                return false;
                            }
                        }
                    }
                }

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

    [HttpGet]
    //[CustomAuthorize]

    public async Task<JsonResult> GetAgentsAssistantList()
    {
        List<AgentsAssistantsDataModel> agentsAssistantsList = new List<AgentsAssistantsDataModel>();
        try
        {
            var users = await _userService.GetAgentsAssistantListAsync();

            if (users != null && users.Any())
            {
                var ret = _mapper.Map<List<AgentsAssistantsDataModel>>(users);

                if (ret != null && ret.Count > 0)
                {
                    agentsAssistantsList.AddRange(ret.OrderBy(_ => _.Username).ToList());
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
        }
        return new JsonResult(agentsAssistantsList);
    }
}
