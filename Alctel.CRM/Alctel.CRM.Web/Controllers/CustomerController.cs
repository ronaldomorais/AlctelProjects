using Alctel.CRM.API.Entities;
using Alctel.CRM.Business.Interfaces;
using Alctel.CRM.Business.Services;
using Alctel.CRM.Context.InMemory.Entities;
using Alctel.CRM.Web.Customs;
using Alctel.CRM.Web.Models;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Serialization;

namespace Alctel.CRM.Web.Controllers;


public class CustomerController : Controller
{
    private const string MODULE_NAME = "clientes";
    private const int SIZE_PAGE = 10;
    private readonly IMapper _mapper;
    private readonly ICustomerService _customerService;
    private readonly ILogControllerService _logControllerService;
    private readonly ILoginService _loginService;
    private readonly ITicketService _ticketService;
    private readonly IWebHostEnvironment _hostingEnvironment;
    private readonly IConfigService _configService;

    public CustomerController(IMapper mapper, ICustomerService customerService, ILogControllerService logControllerService, ILoginService loginService, ITicketService ticketService, IWebHostEnvironment hostingEnvironment, IConfigService configService)
    {
        _mapper = mapper;
        _customerService = customerService;
        _logControllerService = logControllerService;
        _loginService = loginService;
        _ticketService = ticketService;
        _hostingEnvironment = hostingEnvironment;
        _configService = configService;
    }

    [HttpGet]
    //[CustomAuthorize]
    public async Task<IActionResult> Index(int currentpage = 1)
    {
        try
        {
            if (IsAuthenticated() == false)
            {
                return RedirectToAction("Create", "Login");
            }

            string physicalPath = _hostingEnvironment.WebRootPath;
            ViewBag.BaseUrl = _configService.GetBaseUrl(physicalPath);

            //var customers = await _customerService.GetAllCustomersAsync();
            //var customers = await _customerService.GetAllCustomerAPIAsync();
            var customers = await _customerService.GetPaginatedCustomerAPIAsync(currentpage, SIZE_PAGE);

            if (customers != null && customers.Any())
            {
                var customersModel = _mapper.Map<List<CustomerModel>>(customers);
                var customerCount = await _customerService.GetCustomerCountAPIAsync(); 
                CreateDataPagination(currentpage, customers.Count, SIZE_PAGE, customerCount);
                return View(customersModel);
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

        string physicalPath = _hostingEnvironment.WebRootPath;
        ViewBag.BaseUrl = _configService.GetBaseUrl(physicalPath);

        return View();
    }

    [HttpPost]
    //[CustomAuthorize]
    public async Task<IActionResult> Create([FromForm] CustomerCreateModel model)
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
            var customer = _mapper.Map<CustomerCreateAPI>(model);
            var ret = await _customerService.InsertCustomerAPIAsync(customer);

            if (ret.IsValid)
            {
                LogController logController = new LogController();
                logController.Id = Int64.Parse(ret.Value);
                logController.Module = MODULE_NAME;
                logController.Section = username == null ? string.Empty : username;
                logController.Field = "Todos";
                logController.Value = "Todos";
                logController.UserId = userid != null ? Int64.Parse(userid) : 0;
                logController.Action = "Criar";

                await _logControllerService.InsertLogAPIAsync(logController);
                return RedirectToAction("Index");
            }
            else
            {
                ModelState.AddModelError("", ret.Value);
            }
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

        //var customer = await _customerService.GetCustomerAsync(long.Parse(id));
        var customer = await _customerService.GetCustomerAPIAsync(long.Parse(id));

        if (customer != null)
        {
            var model = _mapper.Map<CustomerModel>(customer);

            if (model.FirstName != null && model.FirstName != string.Empty)
            {
                var idx = model.FirstName.IndexOf(' ');

                if (idx > -1)
                {
                    model.LastName = model.FirstName.Substring(idx + 1);
                    model.FirstName = model.FirstName.Substring(0, idx);
                }
            }

            var customerJourney = await _ticketService.GetCustomerTicketAPIAsync(model.Id);

            if (customerJourney != null && customerJourney.Count > 0)
            {
                customerJourney = customerJourney.OrderByDescending(o => o.ProtocolDate).ToList();
                model.TicketCustomer = _mapper.Map<List<TicketModel>>(customerJourney);
            }

            model.CustomerDataToCompareIfChanged.Id = customer.Id;
            model.CustomerDataToCompareIfChanged.SocialAffectionateName = customer.SocialAffectionateName;
            model.CustomerDataToCompareIfChanged.PhoneNumber2 = customer.PhoneNumber2;
            model.CustomerDataToCompareIfChanged.Email2 = customer.Email2;
            model.CustomerDataToCompareIfChanged.Cnpj = customer.Cnpj;
            model.CustomerDataToCompareIfChanged.CompanyName = customer.CompanyName;
            model.CustomerDataToCompareIfChanged.PhoneNumberCompany = customer.PhoneNumberCompany;
            model.CustomerDataToCompareIfChanged.ExtraField01 = customer.ExtraField01;
            model.CustomerDataToCompareIfChanged.ExtraField02 = customer.ExtraField02;
            model.CustomerDataToCompareIfChanged.ExtraField03 = customer.ExtraField03;

            return View(model);
        }

        return View();
    }

    [HttpPost]
    //[CustomAuthorize]
    public async Task<IActionResult> Edit([FromForm] CustomerModel model)
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
            var customer = _mapper.Map<CustomerAPI>(model);
            //await _customerService.UpdateCustomerAsync(customer);

            bool result = await _customerService.UpdateCustomerAPIAsync(customer);

            if (result)
            {
                if (model.CustomerDataToCompareIfChanged.SocialAffectionateName != customer.SocialAffectionateName)
                {
                    LogController logController = new LogController();
                    logController.Id = model.Id;
                    logController.Module = MODULE_NAME;
                    logController.Section = username == null ? string.Empty : username;
                    logController.Field = "Nome Social";
                    logController.Value = model.CustomerDataToCompareIfChanged.SocialAffectionateName;
                    logController.UserId = userid != null ? Int64.Parse(userid) : 0;
                    logController.Action = "Editar";

                    await _logControllerService.InsertLogAPIAsync(logController);
                }

                if (model.CustomerDataToCompareIfChanged.PhoneNumber2 != customer.PhoneNumber2)
                {
                    LogController logController = new LogController();
                    logController.Id = model.Id;
                    logController.Module = MODULE_NAME;
                    logController.Section = username == null ? string.Empty : username;
                    logController.Field = "Telefone 2";
                    logController.Value = model.CustomerDataToCompareIfChanged.PhoneNumber2;
                    logController.UserId = userid != null ? Int64.Parse(userid) : 0;
                    logController.Action = "Editar";

                    await _logControllerService.InsertLogAPIAsync(logController);
                }

                if (model.CustomerDataToCompareIfChanged.Email2 != customer.Email2)
                {
                    LogController logController = new LogController();
                    logController.Id = model.Id;
                    logController.Module = MODULE_NAME;
                    logController.Section = username == null ? string.Empty : username;
                    logController.Field = "Email 2";
                    logController.Value = model.CustomerDataToCompareIfChanged.Email2;
                    logController.UserId = userid != null ? Int64.Parse(userid) : 0;
                    logController.Action = "Editar";

                    await _logControllerService.InsertLogAPIAsync(logController);
                }

                if (model.CustomerDataToCompareIfChanged.Cnpj != customer.Cnpj)
                {
                    LogController logController = new LogController();
                    logController.Id = model.Id;
                    logController.Module = MODULE_NAME;
                    logController.Section = username == null ? string.Empty : username;
                    logController.Field = "Cnpj";
                    logController.Value = model.CustomerDataToCompareIfChanged.Cnpj;
                    logController.UserId = userid != null ? Int64.Parse(userid) : 0;
                    logController.Action = "Editar";

                    await _logControllerService.InsertLogAPIAsync(logController);
                }

                if (model.CustomerDataToCompareIfChanged.CompanyName != customer.CompanyName)
                {
                    LogController logController = new LogController();
                    logController.Id = model.Id;
                    logController.Module = MODULE_NAME;
                    logController.Section = username == null ? string.Empty : username;
                    logController.Field = "Nome da Empresa";
                    logController.Value = model.CustomerDataToCompareIfChanged.CompanyName;
                    logController.UserId = userid != null ? Int64.Parse(userid) : 0;
                    logController.Action = "Editar";

                    await _logControllerService.InsertLogAPIAsync(logController);
                }

                if (model.CustomerDataToCompareIfChanged.PhoneNumberCompany != customer.PhoneNumberCompany)
                {
                    LogController logController = new LogController();
                    logController.Id = model.Id;
                    logController.Module = MODULE_NAME;
                    logController.Section = username == null ? string.Empty : username;
                    logController.Field = "Telefone Corporativo";
                    logController.Value = model.CustomerDataToCompareIfChanged.PhoneNumberCompany;
                    logController.UserId = userid != null ? Int64.Parse(userid) : 0;
                    logController.Action = "Editar";

                    await _logControllerService.InsertLogAPIAsync(logController);
                }

                //if (model.CustomerDataToCompareIfChanged.ExtraField01 != customer.ExtraField01)
                //{
                //    LogController logController = new LogController();
                //    logController.Id = model.Id;
                //    logController.Module = MODULE_NAME;
                //    logController.Section = username == null ? string.Empty : username;
                //    logController.Field = "Campo 1";
                //    logController.Value = model.CustomerDataToCompareIfChanged.ExtraField01;
                //    logController.UserId = userid != null ? Int64.Parse(userid) : 0;
                //    logController.Action = "Editar";

                //    await _logControllerService.InsertLogAPIAsync(logController);
                //}

                //if (model.CustomerDataToCompareIfChanged.ExtraField02 != customer.ExtraField02)
                //{
                //    LogController logController = new LogController();
                //    logController.Id = model.Id;
                //    logController.Module = MODULE_NAME;
                //    logController.Section = username == null ? string.Empty : username;
                //    logController.Field = "Campo 2";
                //    logController.Value = model.CustomerDataToCompareIfChanged.ExtraField02;
                //    logController.UserId = userid != null ? Int64.Parse(userid) : 0;
                //    logController.Action = "Editar";

                //    await _logControllerService.InsertLogAPIAsync(logController);
                //}

                //if (model.CustomerDataToCompareIfChanged.ExtraField03 != customer.ExtraField03)
                //{
                //    LogController logController = new LogController();
                //    logController.Id = model.Id;
                //    logController.Module = MODULE_NAME;
                //    logController.Section = username == null ? string.Empty : username;
                //    logController.Field = "Campo 3";
                //    logController.Value = model.CustomerDataToCompareIfChanged.ExtraField03;
                //    logController.UserId = userid != null ? Int64.Parse(userid) : 0;
                //    logController.Action = "Editar";

                //    await _logControllerService.InsertLogAPIAsync(logController);
                //}
            }

            return RedirectToAction("Index");
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

        var customer = await _customerService.GetCustomerAsync(long.Parse(id));

        if (customer != null)
        {
            var model = _mapper.Map<CustomerModel>(customer);
            return View(model);
        }

        return View();
    }

    [HttpPost]
    //[CustomAuthorize]
    public async Task<IActionResult> Delete([FromForm] CustomerModel model)
    {
        if (ModelState.IsValid)
        {
            var customer = _mapper.Map<Customer>(model);
            await _customerService.DeleteCustomerAsync(customer);

            return RedirectToAction("Index");
        }
        return View(model);
    }

    public async Task<IActionResult> Index(string searchCustomerType, string searchCustomerText)
    {
        if (IsAuthenticated() == false)
        {
            return RedirectToAction("Create", "Login");
        }

        ViewBag.ShowPageControl = false;
        try
        {
            if (searchCustomerType.Contains("Tipo Filtro") || searchCustomerText == string.Empty)
            {
                ViewBag.AlertFilter = "SHOW";

                var allcustomers = await _customerService.GetAllCustomerAPIAsync();

                if (allcustomers != null && allcustomers.Any())
                {
                    var customersModel = _mapper.Map<List<CustomerModel>>(allcustomers);
                    return View(customersModel);
                }
            }

            if (searchCustomerType.Contains("telefone") || searchCustomerType.Contains("cpf") || searchCustomerType.Contains("cnpj") || searchCustomerType.Contains("matricula"))
            {
                searchCustomerText = string.Join("", searchCustomerText.Where(char.IsDigit).ToList());
            }



            //var users = await _userService.GetAllUserAsync();
            var customers = await _customerService.SearchCustomerAPIAsync(searchCustomerType, searchCustomerText);

            if (customers != null && customers.Any())
            {
                var customersModel = _mapper.Map<List<CustomerModel>>(customers);
                return View(customersModel);
            }

        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
        }
        return View();
    }

    public async Task<IActionResult> Search(string searchCustomerType, string searchCustomerText)
    {
        if (IsAuthenticated() == false)
        {
            return RedirectToAction("Create", "Login");
        }


        string physicalPath = _hostingEnvironment.WebRootPath;
        ViewBag.BaseUrl = _configService.GetBaseUrl(physicalPath);

        ViewBag.ShowPageControl = false;
        try
        {
            if (searchCustomerType.Contains("Tipo Filtro") || searchCustomerText == string.Empty)
            {
                ViewBag.AlertFilter = "SHOW";

                var allcustomers = await _customerService.GetAllCustomerAPIAsync();

                if (allcustomers != null && allcustomers.Any())
                {
                    var customersModel = _mapper.Map<List<CustomerModel>>(allcustomers);
                    return View(customersModel);
                }
            }

            if (searchCustomerType.Contains("telefone") || searchCustomerType.Contains("cpf") || searchCustomerType.Contains("cnpj") || searchCustomerType.Contains("matricula"))
            {
                searchCustomerText = string.Join("", searchCustomerText.Where(char.IsDigit).ToList());
            }



            //var users = await _userService.GetAllUserAsync();
            var customers = await _customerService.SearchCustomerAPIAsync(searchCustomerType, searchCustomerText);

            if (customers != null && customers.Any())
            {
                var customersModel = _mapper.Map<List<CustomerModel>>(customers);
                return View(customersModel);
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
            ViewBag.NextNPage = currentpage + 10 > lastpage ? currentpage + (lastpage - currentpage) : currentpage + 10;
            ViewBag.PreviousNPage = currentpage - 10 <= 0 ? currentpage : currentpage - 10;

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
}

