using System.Collections.Generic;
using System.Dynamic;
using System.Net.Sockets;
using System.Security.AccessControl;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Alctel.CRM.API.Entities;
using Alctel.CRM.Business.Interfaces;
using Alctel.CRM.Business.Interfaces.Classification;
using Alctel.CRM.Business.Services;
using Alctel.CRM.Context.InMemory.Entities;
using Alctel.CRM.Web.Models;
using Alctel.CRM.Web.Models.Classification;
using Alctel.CRM.Web.Tools.LogHelper;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.VisualBasic;
using Newtonsoft.Json;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Alctel.CRM.Web.Controllers;

public class TicketController : Controller
{
    private const int SIZE_PAGE = 10;
    private const string MODULE_NAME = "chamados";
    private readonly ILoginService _loginService;
    private readonly ITicketService _ticketService;
    private readonly IMapper _mapper;
    private readonly ILogControllerService _logControllerService;
    private readonly IClassificationDemandService _classificationDemandService;
    private readonly ICustomerService _customerService;
    private readonly IDemandTypeService _demandTypeService;
    private readonly IClassificationDemandTypeService _classificationDemandTypeService;
    private readonly IClassificationReasonService _classificationReasonService;
    private readonly ILogHelperService _logHelperService;
    private readonly IClassificationListItemsService _classificationListItemsService;
    private readonly ITicketTransferService _ticketTransferService;
    private readonly IConfiguration _configuration;
    private static List<OngoingInteraction> ongoingInteractions = new List<OngoingInteraction>();
    private readonly IWebHostEnvironment _hostingEnvironment;
    private readonly IConfigService _configService;
    private readonly IUserService _userService;
    private readonly ITicketClassificationService _ticketClassificationService;
    private readonly ISlaService _slaService;
    public TicketController(ILoginService loginService, ITicketService ticketService, IMapper mapper, ILogControllerService logControllerService, IClassificationDemandService classificationDemandService, ICustomerService customerService, IDemandTypeService demandTypeService, IClassificationDemandTypeService classificationDemandTypeService, IClassificationReasonService classificationReasonService, ILogHelperService logHelperService, IClassificationListItemsService classificationListItemsService, ITicketTransferService ticketTransferService, IConfiguration configuration, IWebHostEnvironment hostingEnvironment, IConfigService configService, IUserService userService, ITicketClassificationService ticketClassificationService, ISlaService slaService)
    {
        _loginService = loginService;
        _ticketService = ticketService;
        _mapper = mapper;
        _logControllerService = logControllerService;
        _classificationDemandService = classificationDemandService;
        _customerService = customerService;
        _demandTypeService = demandTypeService;
        _classificationDemandTypeService = classificationDemandTypeService;
        _classificationReasonService = classificationReasonService;
        _logHelperService = logHelperService;
        _classificationListItemsService = classificationListItemsService;
        _ticketTransferService = ticketTransferService;
        _configuration = configuration;
        _hostingEnvironment = hostingEnvironment;
        _configService = configService;
        _userService = userService;
        _ticketClassificationService = ticketClassificationService;
        _slaService = slaService;
    }

    [HttpGet]
    //[CustomAuthorize]
    public async Task<IActionResult> Index(int currentpage = 1)
    //public async Task<IActionResult> Index()
    {
        try
        {
            if (IsAuthenticated() == false)
            {
                return RedirectToAction("Create", "Login");
            }

            string physicalPath = _hostingEnvironment.WebRootPath;
            ViewBag.BaseUrl = _configService.GetBaseUrl(physicalPath);

            ////var data = await _ticketService.GetTicketActivatedListAPIAsync();
            //var data = await _ticketService.GetTicketListAPIAsync();

            //if (data != null && data.Any())
            //{
            //    var dataModel = _mapper.Map<List<TicketModel>>(data);
            //    var dataModelOrdered = dataModel.OrderBy(o => o.ProtocolDate).ToList();
            //    return View(dataModelOrdered);
            //}

            var userid = HttpContext.Session.GetString("UserId");
            var profile = HttpContext.Session.GetString("Profile");

            if (userid != null && profile != null)
            {
                bool showPageControl = false;
                List<TicketAPI> data = new List<TicketAPI>();

                if (profile.ToUpper() == "ADMINISTRADOR" || profile.ToUpper() == "MONITOR")
                {
                    data = await _ticketService.GetTicketListGCSupervisorAPIAsync(currentpage, SIZE_PAGE);
                    showPageControl = true;
                }
                else
                {
                    data = await _ticketService.GetTicketListGCAPIAsync(userid);
                }

                if (data != null && data.Any())
                {
                    int qtyTicket = await _ticketService.GetTicketCountAsync();
                    var dataModel = _mapper.Map<List<TicketModel>>(data);
                    var dataModelOrdered = dataModel.OrderBy(o => o.ProtocolDate).ToList();
                    CreateDataPagination(currentpage, dataModelOrdered.Count, SIZE_PAGE, qtyTicket, showPageControl);

                    return View(dataModelOrdered);
                }
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
    public async Task<IActionResult> Create([FromForm] TicketModel model)
    {
        if (IsAuthenticated() == false)
        {
            return RedirectToAction("Create", "Login");
        }

        model.TicketDate = DateTime.Now;
        model.ProtocolDate = DateTime.Now;
        model.ProtocolType = "Filho";
        //model.SlaSystemRole = "1";

        Random random = new Random();
        int randomNumberInRange = random.Next(1, 1000000);

        model.Protocol = System.String.Format("{0:yyyyMMdd}{1:######}", DateTime.Now, randomNumberInRange);

        string? profile = HttpContext.Session.GetString("Profile");
        profile = profile ?? string.Empty;

        await LoadListOptions(model, profile);

        return View(model);
    }

    //[HttpPost]
    ////[CustomAuthorize]
    //public async Task<IActionResult> Create([FromForm] TicketModel model)
    //{
    //    if (IsAuthenticated() == false)
    //    {
    //        return RedirectToAction("Create", "Login");
    //    }

    //    if (ModelState.IsValid)
    //    {
    //        var username = HttpContext.Session.GetString("Username");
    //        var userid = HttpContext.Session.GetString("UserId");
    //        var data = _mapper.Map<TicketAPI>(model);
    //        var ret = await _ticketService.InsertTicketAPIAsync(data);

    //        if (ret)
    //        {
    //            LogController logController = new LogController();
    //            logController.Id = model.Id;
    //            logController.Module = MODULE_NAME;
    //            logController.Section = username == null ? string.Empty : username;
    //            logController.Field = "Ativar/Desativar";
    //            logController.Value = "Ativado";
    //            logController.UserId = userid != null ? Int64.Parse(userid) : 0;
    //            logController.Action = "Criar";

    //            await _logControllerService.InsertLogAPIAsync(logController);
    //        }

    //        return RedirectToAction("Index");
    //    }
    //    return View(model);
    //}

    [HttpGet]
    //[CustomAuthorize]
    public async Task<IActionResult> Edit(string id, string message = "")
    {
        if (IsAuthenticated() == false)
        {
            return RedirectToAction("Create", "Login");
        }

        string physicalPath = _hostingEnvironment.WebRootPath;
        ViewBag.BaseUrl = _configService.GetBaseUrl(physicalPath);

        var data = await _ticketService.GetTicketAPIAsync(long.Parse(id));

        if (data != null)
        {
            var model = _mapper.Map<TicketModel>(data);

            var attachments = await _ticketService.DownloadTicketAttachmentAPIAsync(model.Id);

            if (attachments != null)
            {
                model.Attachments = _mapper.Map<List<TicketAttachmentModel>>(attachments);
            }

            var ticketClassificationResult = data.TicketClassificationResult;

            if (ticketClassificationResult != null && ticketClassificationResult.Count > 0)
            {
                foreach (var item in ticketClassificationResult)
                {
                    TicketClassification ticketClassification = new TicketClassification();
                    ticketClassification.ManifestationTypeName = item.ManifestationTypeName;
                    ticketClassification.ServiceName = item.ServiceName;
                    ticketClassification.ServiceUnitName = item.ServiceUnitItemName;

                    if (item.Reasons != null && item.Reasons.Count > 0)
                    {
                        ticketClassification.Reason01Name = item.Reasons[0].ReasonName;
                    }

                    if (item.Reasons != null && item.Reasons.Count > 1)
                    {
                        ticketClassification.Reason02Name = item.Reasons[1].ReasonName;
                    }

                    model.TicketClassification.Add(ticketClassification);
                }
            }

            string? profile = HttpContext.Session.GetString("Profile");
            profile = profile ?? string.Empty;

            await LoadListOptions(model, profile);

            model.TicketDataToCompareIfChanged.Id = model.Id;
            model.TicketDataToCompareIfChanged.QueueGTId = model.QueueGTId;
            model.TicketDataToCompareIfChanged.QueueGT = model.QueueGT;
            model.TicketDataToCompareIfChanged.AnySolution = model.AnySolution;
            model.TicketDataToCompareIfChanged.TicketStatusId = model.TicketStatusId;
            model.TicketDataToCompareIfChanged.TicketStatus = model.TicketStatus;
            model.TicketDataToCompareIfChanged.TicketCriticalityId = model.TicketCriticalityId;
            model.TicketDataToCompareIfChanged.TicketCriticality = model.TicketCriticality;
            model.TicketDataToCompareIfChanged.DemandObservation = model.DemandObservation;
            //model.TicketDataToCompareIfChanged.DemandTypeId = model.DemandTypeId;
            //model.TicketDataToCompareIfChanged.DemandType = model.DemandType;

            if (string.IsNullOrEmpty(message) == false)
            {
                ViewBag.TicketMessage = message;
            }

            //SLA
            DateTime now = DateTime.Now;
            int days = await _slaService.GetBusinessDays(model.ProtocolDate, now);

            DateTime protocolDateSla = model.ProtocolDate.AddDays(days);
            TimeSpan timeSpan = now - protocolDateSla;
            double totalDays = timeSpan.TotalDays;

            if (totalDays < 0)
            {
                days--;
            }

            model.Sla = days;

            if ((model.SlaSystemRole - model.Sla) <= 1)
            {
                ViewBag.SLAColor = "#FF0000";
            }
            else
            {
                ViewBag.SLAColor = "#FFFFFF";
            }

            return View(model);
        }

        return View();
    }

    [HttpPost]
    //[CustomAuthorize]
    public async Task<IActionResult> Edit([FromForm] TicketModel model)
    {
        if (IsAuthenticated() == false)
        {
            return RedirectToAction("Create", "Login");
        }

        string physicalPath = _hostingEnvironment.WebRootPath;
        ViewBag.BaseUrl = _configService.GetBaseUrl(physicalPath);

        //var profileSession = HttpContext.Session.GetString("Profile");

        //if (profileSession != null)
        //{
        //    if (profileSession.ToUpper() != "ADMINISTRADOR")
        //    {
        //        return RedirectToAction("Index", "Home");
        //    }
        //}


        if (ModelState.IsValid)
        {
            var username = HttpContext.Session.GetString("Username");
            var userid = HttpContext.Session.GetString("UserId");
            var data = _mapper.Map<TicketAPI>(model);

            var ret = await _ticketService.UpdateTicketAPIAsync(data);

            if (ret)
            {
                await UploadFiles(model, username, userid);
                if (model.QueueGTId != model.TicketDataToCompareIfChanged.QueueGTId)
                {
                    LogController logController = new LogController();
                    logController.Id = model.Id;
                    logController.Module = MODULE_NAME;
                    logController.Section = username == null ? string.Empty : username;
                    logController.Field = "Fila GT";
                    logController.Value = model.TicketDataToCompareIfChanged.QueueGT;
                    logController.UserId = userid != null ? Int64.Parse(userid) : 0;
                    logController.Action = "Editar";

                    await _logControllerService.InsertLogAPIAsync(logController);
                }

                if (model.AnySolution != model.TicketDataToCompareIfChanged.AnySolution)
                {
                    LogController logController = new LogController();
                    logController.Id = model.Id;
                    logController.Module = MODULE_NAME;
                    logController.Section = username == null ? string.Empty : username;
                    logController.Field = "Foi dada a Solução para o Cliente";
                    logController.Value = model.TicketDataToCompareIfChanged.AnySolution == "0" ? "Não" : "Sim";
                    logController.UserId = userid != null ? Int64.Parse(userid) : 0;
                    logController.Action = "Editar";

                    await _logControllerService.InsertLogAPIAsync(logController);
                }

                if (model.TicketStatusId != model.TicketDataToCompareIfChanged.TicketStatusId)
                {
                    LogController logController = new LogController();
                    logController.Id = model.Id;
                    logController.Module = MODULE_NAME;
                    logController.Section = username == null ? string.Empty : username;
                    logController.Field = "Status";
                    logController.Value = model.TicketDataToCompareIfChanged.TicketStatus;
                    logController.UserId = userid != null ? Int64.Parse(userid) : 0;
                    logController.Action = "Editar";

                    await _logControllerService.InsertLogAPIAsync(logController);
                }

                if (model.TicketCriticalityId != model.TicketDataToCompareIfChanged.TicketCriticalityId)
                {
                    LogController logController = new LogController();
                    logController.Id = model.Id;
                    logController.Module = MODULE_NAME;
                    logController.Section = username == null ? string.Empty : username;
                    logController.Field = "Criticidade";
                    logController.Value = model.TicketDataToCompareIfChanged.TicketCriticality;
                    logController.UserId = userid != null ? Int64.Parse(userid) : 0;
                    logController.Action = "Editar";

                    await _logControllerService.InsertLogAPIAsync(logController);
                }

                if (model.DemandObservation != model.TicketDataToCompareIfChanged.DemandObservation)
                {
                    LogController logController = new LogController();
                    logController.Id = model.Id;
                    logController.Module = MODULE_NAME;
                    logController.Section = username == null ? string.Empty : username;
                    logController.Field = "Observação da Demanda";
                    logController.Value = model.TicketDataToCompareIfChanged.DemandObservation;
                    logController.UserId = userid != null ? Int64.Parse(userid) : 0;
                    logController.Action = "Editar";

                    await _logControllerService.InsertLogAPIAsync(logController);
                }

                //if (model.DemandTypeId != model.TicketDataToCompareIfChanged.DemandTypeId)
                //{
                //    LogController logController = new LogController();
                //    logController.Id = model.Id;
                //    logController.Module = MODULE_NAME;
                //    logController.Section = username == null ? string.Empty : username;
                //    logController.Field = "Tipo de Demanda";
                //    logController.Value = model.TicketDataToCompareIfChanged.DemandType;
                //    logController.UserId = userid != null ? Int64.Parse(userid) : 0;
                //    logController.Action = "Editar";

                //    await _logControllerService.InsertLogAPIAsync(logController);

                //}
                //return RedirectToAction("Index");

                var ticketClassification = model.TicketClassification;

                if (ticketClassification != null && ticketClassification.Count() > 0)
                {
                    int order = 0;
                    foreach (var item in ticketClassification)
                    {
                        if (item.ManifestationTypeId != 0)
                        {
                            TicketClassificationCreateModel classificationModel = new TicketClassificationCreateModel();
                            classificationModel.TicketId = model.Id;
                            classificationModel.ServiceId = item.ServiceId;
                            classificationModel.ServiceUnitId = item.ServiceUnitId;
                            classificationModel.UserId = userid != null ? Int64.Parse(userid) : 0;
                            classificationModel.Order = order;

                            if (item.Reason01Id != null && item.Reason01Id != 0)
                            {
                                TicketReasonModel reason01 = new TicketReasonModel();
                                reason01.ReasonId = item.Reason01Id.Value;
                                reason01.ListItemId = item.Reason01ListItemId;
                                classificationModel.TicketReason.Add(reason01);

                                if (item.Reason02Id != null && item.Reason02Id != 0)
                                {
                                    TicketReasonModel reason02 = new TicketReasonModel();
                                    reason02.ReasonId = item.Reason02Id.Value;
                                    reason02.ListItemId = item.Reason02ListItemId;
                                    classificationModel.TicketReason.Add(reason02);
                                }
                            }

                            var classification = _mapper.Map<TicketClassificationAPI>(classificationModel);
                            await _ticketClassificationService.InsertTicketClassificationAPIAsync(classification);
                        }

                        order++;
                    }
                }
                return RedirectToAction("Edit", "Ticket", new { id = model.Id, message = "Chamado Atualizado com Sucesso!" });
            }
        }

        if (model.TicketClassification.Count == 0)
        {
            ViewBag.TicketMessage = "Erro Uma classificação é necessária";
        }
        else
        {
            ViewBag.TicketMessage = "Erro Atualizando Chamado!";
        }

        string? profile = HttpContext.Session.GetString("Profile");
        profile = profile ?? string.Empty;

        await LoadListOptions(model, profile);
        return View(model);
    }

    //[HttpGet]
    //public async Task<IActionResult> GenesysInteractionEvent(string nomeFila = "", string conversastionid = "", string email = "", string cpf = "", string protocolo = "", string navegacao = "", bool reload = false)
    //{
    //    List<TicketModel> tickets = new List<TicketModel>();
    //    OngoingInteractions ongoingInteractionsCollection = new OngoingInteractions();

    //    _logHelperService.LogMessage($"[GenesysInteractionEvent] nomeFila: {nomeFila}, conversastionid: {conversastionid}, email: {email}, cpf: {cpf}, protocolo: {protocolo}, navegacao: {navegacao}, reload: {reload}");

    //    if (IsAuthenticated() == false)
    //    {
    //        var loginuser = email;
    //        var logininfo = await _loginService.GetLoginPIAsync(loginuser);

    //        ViewBag.Email = email;

    //        if (logininfo.UserStatus && logininfo.ProfileStatus)
    //        {
    //            string profile = logininfo.Profile != null ? logininfo.Profile : string.Empty;
    //            string username = logininfo.UserName != null ? logininfo.UserName : string.Empty;
    //            Int64 userid = logininfo.UserId;

    //            HttpContext.Session.SetString("Profile", profile);
    //            HttpContext.Session.SetString("Username", username);
    //            HttpContext.Session.SetString("LoginUser", loginuser);
    //            HttpContext.Session.SetString("UserId", userid.ToString());

    //            //List<Claim> claims = new List<Claim>();

    //            //claims.Add(new Claim(ClaimTypes.Name, logininfo.UserName));
    //            //ClaimsIdentity identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
    //            //var login = HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identity));

    //            switch (profile.ToUpper())
    //            {
    //                case "ADMINISTRADOR":
    //                    HttpContext.Session.SetString("Module", "Home,Customer,User,Ticket,ServiceUnit,Area,ServiceLevel,DemandType,Configuration,ClassificationList,ReasonList,ClassificationTree");
    //                    break;
    //                case "AGENTE":
    //                    HttpContext.Session.SetString("Module", "Home,Customer,Ticket");
    //                    break;
    //                case "ASSISTENTE":
    //                    HttpContext.Session.SetString("Module", "Home,Customer,Ticket");
    //                    break;
    //                case "MONITOR":
    //                    HttpContext.Session.SetString("Module", "Home,Customer,Ticket");
    //                    break;
    //            }

    //            HttpContext.Session.SetString("isAuthenticated", "true");
    //        }
    //        else
    //        {
    //            return RedirectToAction("Create", "Login");
    //        }
    //    }

    //    var ongoingInteractionsSession = HttpContext.Session.GetString("OngoingInteractions");

    //    if (ongoingInteractionsSession != null)
    //    {
    //        var ongoingInteractions = JsonConvert.DeserializeObject<OngoingInteractions>(ongoingInteractionsSession);

    //        if (ongoingInteractions != null && ongoingInteractions.Count > 0)
    //        {
    //            var ongoingInteractionsValid = ongoingInteractions.Where(o => string.IsNullOrEmpty(o.ConversationId) == false).ToList();

    //            ongoingInteractionsCollection.AddRange(ongoingInteractionsValid);
    //        }
    //    }

    //    if (reload == false && string.IsNullOrEmpty(conversastionid) == false)
    //    {
    //        var ongoingInteractionFromList = ongoingInteractionsCollection.FirstOrDefault(_ => _.ConversationId == conversastionid);

    //        if (ongoingInteractionFromList == null)
    //        {
    //            //Random random = new Random();
    //            //int randomNumberInRange = random.Next(1, 1000000);

    //            //protocolo = System.String.Format("{0:yyyyMMdd}{1:######}", DateTime.Now, randomNumberInRange);

    //            OngoingInteraction ongoingInteraction = new OngoingInteraction()
    //            {
    //                ConversationId = conversastionid,
    //                Cpf = cpf,
    //                Protocol = protocolo,
    //                QueueName = nomeFila,
    //                TicketDate = DateTime.Now,
    //                CustomerNavigation = navegacao
    //            };

    //            ongoingInteractionsCollection.Add(ongoingInteraction);
    //        }
    //        else
    //        {
    //            ongoingInteractionFromList.Protocol = protocolo;
    //            ongoingInteractionFromList.QueueName = nomeFila;
    //            ongoingInteractionFromList.TicketDate = DateTime.Now;
    //            ongoingInteractionFromList.CustomerNavigation = navegacao;
    //        }
    //    }

    //    HttpContext.Session.Remove("OngoingInteractions");

    //    if (ongoingInteractionsCollection.Count > 0)
    //    {
    //        ongoingInteractionsSession = JsonConvert.SerializeObject(ongoingInteractionsCollection);
    //        HttpContext.Session.SetString("OngoingInteractions", ongoingInteractionsSession);

    //        foreach (var interaction in ongoingInteractionsCollection)
    //        {
    //            TicketModel ticketModel = new TicketModel();
    //            ticketModel.QueueGenesys = interaction.QueueName;
    //            ticketModel.Protocol = interaction.Protocol;
    //            ticketModel.TicketDataToCompareIfChanged = new TicketDataToCompareIfChangedLog();
    //            ticketModel.TicketDate = interaction.TicketDate;
    //            ticketModel.ConversationId = interaction.ConversationId;
    //            ticketModel.CustomerNavigation = interaction.CustomerNavigation;
    //            ticketModel.SlaSystemRole = "1";
    //            ticketModel.ProtocolType = "Pai";

    //            if (string.IsNullOrEmpty(interaction.Cpf) == false)
    //            {
    //                var customer = await _customerService.GetCustomerAPIAsync(interaction.Cpf);

    //                if (customer != null)
    //                {
    //                    if (customer.Id > 0)
    //                    {
    //                        var customerModel = _mapper.Map<CustomerModel>(customer);
    //                        ticketModel.Customer = customerModel;
    //                    }
    //                    else
    //                    {
    //                        ticketModel.Customer = new CustomerModel();
    //                        ticketModel.Customer.Cpf = interaction.Cpf;
    //                    }
    //                }
    //                else
    //                {
    //                    ticketModel.Customer = new CustomerModel();
    //                    ticketModel.Customer.Cpf = interaction.Cpf;
    //                }
    //            }
    //            else
    //            {
    //                ticketModel.Customer = new CustomerModel();
    //                ticketModel.Customer.Cpf = interaction.Cpf;
    //            }

    //            await LoadListOptions(ticketModel);

    //            if (tickets.Count() == 0)
    //                tickets.Add(ticketModel);
    //        }
    //    }

    //    return View(tickets);
    //}

    [HttpGet]
    //[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public async Task<IActionResult> GenesysInteractionEvent(string nomeFila = "", string conversastionid = "", string email = "", string cpf = "", string protocolo = "", string navegacao = "", string emailCliente = "", string protocolo_pai = "", bool reload = false, bool cancel = false, bool tickedSaved = false)
    {
        _logHelperService.LogMessage($"[{conversastionid}] nomeFila: {nomeFila}, conversastionid: {conversastionid}, email: {email}, cpf: {cpf}, protocolo: {protocolo}, navegacao: {navegacao}, emailCliente: {emailCliente}, protocolo_pai: {protocolo_pai}, cancel: {cancel}");

        string physicalPath = _hostingEnvironment.WebRootPath;
        string baseUrl = _configService.GetBaseUrl(physicalPath);
        ViewBag.BaseUrl = baseUrl;

        if (IsAuthenticated() == false)
        {
            _logHelperService.LogMessage($"[{conversastionid}] Usuário não logado no GT. Logando...");
            var loginuser = email;
            var logininfo = await _loginService.GetLoginPIAsync(loginuser);
            _logHelperService.LogMessage($"[{conversastionid}] {JsonConvert.SerializeObject(logininfo)}");

            ViewBag.Email = email;

            if (logininfo.UserStatus && logininfo.ProfileStatus)
            {
                _logHelperService.LogMessage($"[{conversastionid}] Usuário localizado. Criando sessão...");

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
            }
            else
            {
                _logHelperService.LogError($"[{conversastionid}] Erro nos dados do usuário...");

                return RedirectToAction("Create", "Login");
            }
        }

        if (string.IsNullOrEmpty(conversastionid))
        {
            _logHelperService.LogError($"[{conversastionid}] Sem conversation id...");
            return RedirectToAction("Index");
        }

        if (cancel)
        {
            //HttpContext.Session.Remove($"ConversationId:{conversastionid}");
            var interactionToCancel = ongoingInteractions.Where(_ => _.ConversationId == conversastionid).FirstOrDefault();

            if (interactionToCancel != null)
            {
                ongoingInteractions.Remove(interactionToCancel);
            }

            return RedirectToAction("Index", "Ticket");
        }

        string? profileSession = HttpContext.Session.GetString("Profile");
        profileSession = profileSession ?? string.Empty;

        var ticket = await _ticketService.GetTicketAPIAsync(protocolo);

        if (ticket != null && ticket.Protocol != null)
        {
            var model = _mapper.Map<TicketModel>(ticket);
            model.TicketSaved = true;
            //model.TicketInTransfering = await _ticketTransferService.IsTicketInTransferQueueAsync(protocolo);
            model.TicketInTransfering = model.UserId == null ? true : false;
            model.User = model.UserId.ToString();
            ViewBag.TicketMessage = "Aviso: Protocolo já processado";
            await LoadListOptions(model, profileSession);
            return View(model);
        }

        ViewBag.ConversationId = conversastionid;

        if (string.IsNullOrEmpty(protocolo) || protocolo == "0")
        {
            _logHelperService.LogWarning("Protocolo não recebido do Genesys. Gerando um aleatoriamente...");
            Random random = new Random();
            long minValue = 100000000000L;
            long maxValue = 999999999999L;
            string sufix = random.NextInt64(minValue, maxValue).ToString();

            StringBuilder protocolSufix = new StringBuilder();
            string protocolPrefix = DateTime.Now.ToString("yyyyMMdd");

            int index = 0;
            for (int i = 0; i < 6; i++)
            {
                index = random.Next(0, 5);

                if (index < sufix.Length)
                    protocolSufix.Append(sufix[index]);
                else
                    protocolSufix.Append(sufix.FirstOrDefault());
            }

            protocolo = $"{protocolPrefix}{protocolSufix.ToString()}";
        }

        OngoingInteraction? ongoingInteraction = null;
        //var ongoingInteractionSession = HttpContext.Session.GetString($"ConversationId:{conversastionid}");

        //if (ongoingInteractionSession == null)
        //{
        //    ongoingInteraction = new OngoingInteraction()
        //    {
        //        userEmail = email,
        //        MediaType = string.IsNullOrEmpty(TipoMidia) ? string.Empty : TipoMidia,
        //        ConversationId = conversastionid,
        //        Cpf = cpf,
        //        Protocol = protocolo,
        //        ProtocolType = "Pai",
        //        QueueName = nomeFila,
        //        TicketDate = DateTime.Now,
        //        CustomerNavigation = navegacao,
        //    };

        //    HttpContext.Session.SetString($"ConversationId:{conversastionid}", JsonConvert.SerializeObject(ongoingInteraction));
        //}
        //else
        //{
        //    ongoingInteraction = JsonConvert.DeserializeObject<OngoingInteraction>(ongoingInteractionSession);
        //    if (ongoingInteraction != null && reload == false)
        //    {
        //        ongoingInteraction.QueueName = nomeFila;
        //        //ongoingInteraction.Protocol = protocolo;
        //        ongoingInteraction.CustomerNavigation = navegacao;

        //        HttpContext.Session.Remove($"ConversationId:{conversastionid}");
        //        HttpContext.Session.SetString($"ConversationId:{conversastionid}", JsonConvert.SerializeObject(ongoingInteraction));
        //    }
        //}

        ongoingInteraction = ongoingInteractions.Where(_ => _.ConversationId == conversastionid).FirstOrDefault();

        if (ongoingInteraction == null)
        {
            ongoingInteraction = new OngoingInteraction()
            {
                userEmail = email,
                //MediaType = string.IsNullOrEmpty(TipoMidia) ? string.Empty : TipoMidia,
                ConversationId = conversastionid,
                Cpf = cpf,
                CustomerEmail = emailCliente,
                Protocol = protocolo,
                ParentTicket = protocolo_pai,
                ProtocolType = "Pai",
                QueueName = nomeFila,
                TicketDate = DateTime.Now,
                CustomerNavigation = navegacao,
            };

            ongoingInteractions.Add(ongoingInteraction);
        }
        else
        {
            ongoingInteraction.QueueName = nomeFila;
            ongoingInteraction.userEmail = email;
            //ongoingInteraction.Protocol = protocolo;
            ongoingInteraction.CustomerNavigation = navegacao;
        }

        if (ongoingInteraction != null)
        {
            TicketModel ticketModel = new TicketModel();
            ticketModel.Id = -1;
            ticketModel.QueueGenesys = ongoingInteraction.QueueName;
            ticketModel.Protocol = ongoingInteraction.Protocol;
            ticketModel.TicketDate = ongoingInteraction.TicketDate;
            ticketModel.ConversationId = conversastionid;
            ticketModel.CustomerNavigation = ongoingInteraction.CustomerNavigation;
            //ticketModel.SlaSystemRole = "1";
            ticketModel.ProtocolType = ongoingInteraction.ProtocolType;
            ticketModel.TicketDataToCompareIfChanged = new TicketDataToCompareIfChangedLog();
            ticketModel.QueueGT = "1º nível";
            ticketModel.User = ongoingInteraction.userEmail;

            if (string.IsNullOrEmpty(protocolo_pai) == false)
            {
                ticketModel.ParentTicket = ongoingInteraction.ParentTicket;
                ongoingInteraction.AutoSaveData.ParentTicket = ongoingInteraction.ParentTicket;
                _logHelperService.LogMessage($"[{conversastionid}] Associado protocolo_pai: {ticketModel.ParentTicket}");
            }


            if (string.IsNullOrEmpty(emailCliente) == false)
            {
                //ticketModel.DemandInformation = $"https://apps.sae1.pure.cloud/directory/#/analytics/interactions/{conversastionid}/admin";

                var emailurl = _configuration.GetSection("GenesysCloud:EmailUrl").Value;

                if (emailurl != null)
                {
                    ticketModel.DemandInformation = emailurl.Replace("{conversationid}", conversastionid);
                }
            }
            else
            {
                ticketModel.DemandInformation = string.Empty;
            }

            ticketModel.DemandTypeId = ongoingInteraction.AutoSaveData.DemandTypeId;
            ticketModel.TicketCriticalityId = ongoingInteraction.AutoSaveData.TicketCriticalityId;
            ticketModel.TicketStatusId = ongoingInteraction.AutoSaveData.TicketStatusId;
            ticketModel.AnySolution = ongoingInteraction.AutoSaveData.AnySolution;
            ticketModel.DemandObservation = ongoingInteraction.AutoSaveData.DemandObservation ?? string.Empty;
            ticketModel.ParentTicket = ongoingInteraction.AutoSaveData.ParentTicket;

            if (ongoingInteraction.AutoSaveData.TicketClassification != null && ongoingInteraction.AutoSaveData.TicketClassification.Count > 0)
            {
                var ticketClassificationResult = ongoingInteraction.AutoSaveData.TicketClassification;
                foreach (var item in ticketClassificationResult)
                {
                    TicketClassification ticketClassification = new TicketClassification();
                    ticketClassification.ManifestationTypeId = item.ManifestationTypeId;
                    ticketClassification.ManifestationTypeName = item.ManifestationTypeName;
                    ticketClassification.ServiceUnitId = item.ServiceUnitId;
                    ticketClassification.ServiceUnitName = item.ServiceUnitName;
                    ticketClassification.ServiceId = item.ServiceId;
                    ticketClassification.ServiceName = item.ServiceName;
                    ticketClassification.Reason01Id = item.Reason01Id;
                    ticketClassification.Reason01ListItemName = item.Reason01ListItemName ?? string.Empty;
                    ticketClassification.Reason01ListItemId = item.Reason01ListItemId;
                    ticketClassification.Reason02ListItemName = item.Reason02ListItemName ?? string.Empty;
                    ticketClassification.Reason02ListItemId = item.Reason02ListItemId;

                    ticketModel.TicketClassification.Add(ticketClassification);
                }

                ViewBag.TicketClassificationJsonData = JsonConvert.SerializeObject(ticketModel.TicketClassification);
            }


            //var dir = Path.Combine(Directory.GetCurrentDirectory(), "Contents", conversastionid);

            //if (Directory.Exists(dir))
            //{
            //    DirectoryInfo directoryInfo = new DirectoryInfo(dir);

            //    foreach (var file in directoryInfo.GetFiles())
            //    {
            //        AttachmentData attachmentData = new AttachmentData();
            //        attachmentData.ContentDisposition = $"form-data; name=\"Files\"; filename=\"{file.Name}\"";
            //        attachmentData.FileName = file.Name;


            //        //byte[] fileBytes = new byte[file.Length];
            //        //var stream = file.OpenReadStream();
            //        //await stream.ReadAsync(fileBytes, 0, (int)file.Length);


            //        byte[] fileBytes = System.IO.File.ReadAllBytes(Path.Combine(dir, file.Name));


            //        attachmentData.FileContent = Convert.ToBase64String(fileBytes);
            //        attachmentData.FileBytes = fileBytes;
            //        //attachmentData.FileStream = new FileStream(Path.Combine(dir, file.Name), FileMode.Open, FileAccess.Read);

            //        switch (file.Extension.ToUpper())
            //        {
            //            case ".PDF":
            //                attachmentData.ContentType = "application/pdf";
            //                break;
            //        }

            //        attachmentDatas.Add(attachmentData);
            //    }

            //    if (attachmentDatas.Count > 0)
            //        return Json(new { success = true, attachments = attachmentDatas });
            //}


            string customerData = string.Empty;

            if (string.IsNullOrEmpty(ongoingInteraction.Cpf) == false)
            {
                _logHelperService.LogMessage($"[{conversastionid}] Localizando cliente via CPF: {ongoingInteraction.Cpf}");
                customerData = ongoingInteraction.Cpf;
            }
            else if (string.IsNullOrEmpty(ongoingInteraction.CustomerEmail) == false)
            {
                _logHelperService.LogMessage($"[{conversastionid}] Localizando cliente via Email: {ongoingInteraction.CustomerEmail}");
                customerData = ongoingInteraction.CustomerEmail;
            }


            if (string.IsNullOrEmpty(customerData) == false)
            {
                var customer = await _customerService.GetCustomerAPIAsync(customerData);

                if (customer != null)
                {
                    if (customer.Id > 0)
                    {
                        var customerModel = _mapper.Map<CustomerModel>(customer);
                        ticketModel.Customer = customerModel;

                        _logHelperService.LogMessage($"[{conversastionid}] Cliene Localizado: {JsonConvert.SerializeObject(customerModel)}");

                        var customerJourney = await _ticketService.GetCustomerTicketAPIAsync(customer.Id);

                        if (customerJourney != null && customerJourney.Count > 0)
                        {
                            customerJourney = customerJourney.OrderByDescending(o => o.ProtocolDate).ToList();
                            ticketModel.TicketCustomer = _mapper.Map<List<TicketModel>>(customerJourney);
                        }
                    }
                    else
                    {
                        ticketModel.Customer = new CustomerModel();
                        ticketModel.Customer.Cpf = cpf;
                    }
                }
                else
                {
                    ticketModel.Customer = new CustomerModel();
                    ticketModel.Customer.Cpf = cpf;
                }
            }
            else
            {
                _logHelperService.LogMessage($"[{conversastionid}] Não carregado nem cpf e nem email");

                ticketModel.Customer = new CustomerModel();
                ticketModel.Customer.Cpf = cpf;
            }

            await LoadListOptions(ticketModel, profileSession);

            return View(ticketModel);
        }


        return View(new TicketModel());
    }

    [HttpPost]
    public async Task<IActionResult> GenesysInteractionEvent([FromForm] TicketModel model)
    {
        string? profile = HttpContext.Session.GetString("Profile");
        profile = profile ?? string.Empty;

        string physicalPath = _hostingEnvironment.WebRootPath;
        ViewBag.BaseUrl = _configService.GetBaseUrl(physicalPath);

        try
        {
            if (IsAuthenticated() == false)
            {
                return RedirectToAction("Index", "Home");
            }

            var customerJourney = await _ticketService.GetCustomerTicketAPIAsync(model.Customer.Id);

            if (customerJourney != null && customerJourney.Count > 0)
            {
                customerJourney = customerJourney.OrderByDescending(o => o.ProtocolDate).ToList();
                model.TicketCustomer = _mapper.Map<List<TicketModel>>(customerJourney);
            }

            if (ModelState.IsValid)
            {
                string conversationid = model.ConversationId ?? string.Empty;

                //var ongoingInteractionSession = HttpContext.Session.GetString($"ConversationId:{conversationid}");

                //if (ongoingInteractionSession != null)
                //{
                //    var ongoingInteraction = JsonConvert.DeserializeObject<OngoingInteraction>(ongoingInteractionSession);

                //    var useridSession = HttpContext.Session.GetString("UserId");
                //    if (useridSession != null)
                //    {
                //        _logHelperService.LogMessage($"[{conversationid}] usuário da sessão: {useridSession}");
                //        model.User = useridSession;
                //    }

                //    Int64 queueGTId = 1;
                //    var queueGTCollection = await _ticketService.GetTicketQueueGTAPIAsync();

                //    if (queueGTCollection != null && queueGTCollection.Count > 0)
                //    {
                //        queueGTId = queueGTCollection.FirstOrDefault(_ => _.Name == model.QueueGT) != null ? queueGTCollection.FirstOrDefault(_ => _.Name == model.QueueGT)!.Id : 1;
                //    }

                //    model.QueueGT = queueGTId.ToString();

                //    var ticketCreateAPI = _mapper.Map<TicketCreateAPI>(model);
                //    _logHelperService.LogMessage($"[{conversationid}] Ticket para ser salvo: {JsonConvert.SerializeObject(ticketCreateAPI)}");

                //    var ticket_ret = await _ticketService.InsertTicketAPIAsync(ticketCreateAPI);
                //    _logHelperService.LogMessage($"[{conversationid}] Ticket {JsonConvert.SerializeObject(ticketCreateAPI)}. RETORNO: {ticket_ret}");

                //    if (ticket_ret.IsValid)
                //    {
                //        model.Id = Int64.Parse(ticket_ret.Value);

                //        if (model.Files != null)
                //        {
                //            foreach (var file in model.Files)
                //            {
                //                var content = new MultipartFormDataContent();
                //                var filename = Path.GetFileName(file.FileName);
                //                var filestream = new FileStream(filename, FileMode.Create);
                //                //var filestream = System.IO.File.Open(filename, FileMode.Open);
                //                content.Add(new StreamContent(filestream), "file", filename);
                //            }

                //            var ticketAttachment = new TicketAttachmentAPI();
                //            ticketAttachment.TicketId = model.Id;
                //            await _ticketService.UploadTicketAttachmentAPIAsync(ticketAttachment);
                //        }

                //        if (Directory.Exists(model.ConversationId))
                //        {
                //            Directory.Delete(model.ConversationId, true);
                //        }

                //        await CreateClassification(model, conversationid);

                //        HttpContext.Session.Remove($"ConversationId:{conversationid}");

                //        var username = HttpContext.Session.GetString("Username");

                //        LogController logController = new LogController();
                //        logController.Id = model.Id;
                //        logController.Module = MODULE_NAME;
                //        logController.Section = username == null ? string.Empty : username;
                //        logController.Field = "Todos";
                //        logController.Value = "Todos";
                //        logController.UserId = useridSession != null ? Int64.Parse(useridSession) : 0;
                //        logController.Action = "Criar";

                //        await _logControllerService.InsertLogAPIAsync(logController);

                //        ViewBag.TicketMessage = "Chamado Criado com sucesso";
                //        model.TicketSaved = true;
                //        await LoadListOptions(model);
                //        return View(model);
                //        //return Redirect($"/Ticket/GenesysInteractionEvent/?nomeFila=${model.QueueGenesys}&conversastionid=${model.ConversationId}&email=&cpf=${model.Customer.Cpf}&protocolo=${model.Protocol}&navegacao=${model.CustomerNavigation}&reload=true&cancel=false&ticketSaved=true");
                //    }
                //    else
                //    {
                //        ViewBag.TicketMessage = $"Erro Criando Chamado: {ticket_ret.Value}";
                //    }
                //}

                var ongoingInteraction = ongoingInteractions.Where(_ => _.ConversationId == conversationid).FirstOrDefault();

                if (ongoingInteraction != null)
                {
                    var useridSession = HttpContext.Session.GetString("UserId");
                    if (useridSession != null)
                    {
                        _logHelperService.LogMessage($"[{conversationid}] usuário da sessão: {useridSession}");
                        model.User = useridSession;
                    }

                    Int64 queueGTId = 1;
                    var queueGTCollection = await _ticketService.GetTicketQueueGTAPIAsync();

                    if (queueGTCollection != null && queueGTCollection.Count > 0)
                    {
                        queueGTId = queueGTCollection.FirstOrDefault(_ => _.Name == model.QueueGT) != null ? queueGTCollection.FirstOrDefault(_ => _.Name == model.QueueGT)!.Id : 1;
                    }

                    model.QueueGT = queueGTId.ToString();

                    var ticketCreateAPI = _mapper.Map<TicketCreateAPI>(model);
                    _logHelperService.LogMessage($"[{conversationid}] Ticket para ser salvo: {JsonConvert.SerializeObject(ticketCreateAPI)}");

                    var ticket_ret = await _ticketService.InsertTicketAPIAsync(ticketCreateAPI);
                    _logHelperService.LogMessage($"[{conversationid}] Ticket {JsonConvert.SerializeObject(ticketCreateAPI)}. RETORNO: {ticket_ret}");

                    if (ticket_ret.IsValid)
                    {
                        model.Id = Int64.Parse(ticket_ret.Value);

                        await UploadFiles(model, null, null);

                        var attachments = await _ticketService.DownloadTicketAttachmentAPIAsync(model.Id);

                        if (attachments != null)
                        {
                            model.Attachments = _mapper.Map<List<TicketAttachmentModel>>(attachments);
                        }

                        string dir = Path.Combine(Directory.GetCurrentDirectory(), "Contents", conversationid);

                        if (Directory.Exists(dir))
                        {
                            string[] files = Directory.GetFiles(dir);

                            foreach (string file in files)
                            {
                                System.IO.File.Delete(file);
                            }

                            Directory.Delete(dir);
                        }

                        //await CreateClassification(model, conversationid);

                        //HttpContext.Session.Remove($"ConversationId:{conversationid}");
                        //ongoingInteractions.Remove(ongoingInteraction);

                        var ongoingInteractionsToRemove = ongoingInteractions.Where(_ => _.ConversationId == conversationid).ToList();

                        if (ongoingInteractionsToRemove != null && ongoingInteractionsToRemove.Count() > 0)
                        {
                            foreach (var item in ongoingInteractionsToRemove)
                            {
                                ongoingInteractions.Remove(item);
                            }
                        }

                        var ticketClassification = model.TicketClassification;

                        if (ticketClassification != null && ticketClassification.Count() > 0)
                        {
                            int order = 0;
                            foreach (var item in ticketClassification)
                            {
                                if (item.ManifestationTypeId != 0)
                                {
                                    TicketClassificationCreateModel classificationModel = new TicketClassificationCreateModel();
                                    classificationModel.TicketId = model.Id;
                                    classificationModel.ServiceId = item.ServiceId;
                                    classificationModel.ServiceUnitId = item.ServiceUnitId;
                                    classificationModel.UserId = string.IsNullOrEmpty(model.User) ? 0 : Int64.Parse(model.User);
                                    classificationModel.Order = order;

                                    if (item.Reason01Id != null && item.Reason01Id != 0)
                                    {
                                        TicketReasonModel reason01 = new TicketReasonModel();
                                        reason01.ReasonId = item.Reason01Id.Value;
                                        reason01.ListItemId = item.Reason01ListItemId;
                                        classificationModel.TicketReason.Add(reason01);

                                        if (item.Reason02Id != null && item.Reason02Id != 0)
                                        {
                                            TicketReasonModel reason02 = new TicketReasonModel();
                                            reason02.ReasonId = item.Reason02Id.Value;
                                            reason02.ListItemId = item.Reason02ListItemId;
                                            classificationModel.TicketReason.Add(reason02);
                                        }
                                    }

                                    var classification = _mapper.Map<TicketClassificationAPI>(classificationModel);
                                    await _ticketClassificationService.InsertTicketClassificationAPIAsync(classification);
                                }

                                order++;
                            }
                        }

                        var username = HttpContext.Session.GetString("Username");

                        LogController logController = new LogController();
                        logController.Id = model.Id;
                        logController.Module = MODULE_NAME;
                        logController.Section = username == null ? string.Empty : username;
                        logController.Field = "Todos";
                        logController.Value = "Todos";
                        logController.UserId = useridSession != null ? Int64.Parse(useridSession) : 0;
                        logController.Action = "Criar";

                        await _logControllerService.InsertLogAPIAsync(logController);

                        ViewBag.TicketMessage = "Chamado Criado com sucesso";
                        model.TicketSaved = true;

                        await LoadListOptions(model, profile);
                        return View(model);
                        //return Redirect($"/Ticket/GenesysInteractionEvent/?nomeFila=${model.QueueGenesys}&conversastionid=${model.ConversationId}&email=&cpf=${model.Customer.Cpf}&protocolo=${model.Protocol}&navegacao=${model.CustomerNavigation}&reload=true&cancel=false&ticketSaved=true");
                    }
                    else
                    {
                        ViewBag.TicketMessage = $"Erro Criando Chamado: {ticket_ret.Value}";
                    }
                }
                else
                {
                    model.TicketSaved = true;
                    ViewBag.TicketMessage = "Erro Criando Chamado: Interação já processada";

                }
            }
            else
            {
                ViewBag.TicketMessage = "Erro Criando Chamado: Campos obrigatórios devem ser preenchidos";
            }
        }
        catch (Exception ex)
        {
            ViewBag.TicketMessage = "Erro Criando Chamado: Exceção";
        }


        await LoadListOptions(model, profile);
        return View(model);
    }

    [HttpPost]
    public async Task AutoSaveData([FromForm] AutoSaveDataModel model)
    {
        try
        {
            string conversationid = model.ConversationId ?? string.Empty;

            if (string.IsNullOrEmpty(conversationid))
            {
                return;
            }

            var ongoingInteraction = ongoingInteractions.Where(_ => _.ConversationId == conversationid).FirstOrDefault();

            if (ongoingInteraction != null)
            {
                ongoingInteraction.AutoSaveData = model;
                string dir = Path.Combine(Directory.GetCurrentDirectory(), "Contents", conversationid);

                if (Directory.Exists(dir))
                {
                    string[] files = Directory.GetFiles(dir);

                    foreach (string file in files)
                    {
                        System.IO.File.Delete(file);
                    }
                }

                if (model.Files != null && model.Files.Count > 0)
                {
                    if (Directory.Exists(dir) == false)
                    {
                        Directory.CreateDirectory(dir);
                    }

                    foreach (var file in model.Files)
                    {
                        //AttachmentData attachmentData = new AttachmentData();
                        //attachmentData.ContentDisposition = $"form-data; name=\"Files\"; filename=\"{file.FileName}\"";
                        //attachmentData.FileName = file.FileName;
                        //attachmentData.ContentType = file.ContentType;
                        //ongoingInteraction.AutoSaveData.AttachmentDatas.Add(attachmentData);

                        string path = Path.Combine(dir, file.FileName);

                        using (var stream = new FileStream(path, FileMode.Create))
                        {
                            await file.CopyToAsync(stream);
                            stream.Close();
                        }
                    }
                }
            }


            //var ongoingInteractionSession = HttpContext.Session.GetString($"ConversationId:{conversationid}");

            //if (ongoingInteractionSession != null)
            //{
            //    var ongoingInteraction = JsonConvert.DeserializeObject<OngoingInteraction>(ongoingInteractionSession);

            //    if (ongoingInteraction != null)
            //    {
            //        ongoingInteraction.AutoSaveData = model;
            //        //ongoingInteraction.Files = model.Files;

            //        var files = model.Files;

            //        if (files != null && files.Count() > 0)
            //        {
            //            foreach (var file in files)
            //            {
            //                //Convert to binary
            //                if (file.Length > 0)
            //                {
            //                    byte[] fileBytes = new byte[file.Length];
            //                    using (var stream = file.OpenReadStream())
            //                    {
            //                        await stream.ReadAsync(fileBytes, 0, (int)file.Length);
            //                        Attachment attachment = new Attachment();
            //                        attachment.FileName = file.FileName;
            //                        //attachment.FileStringByte = Encoding.UTF8.GetString(fileBytes);
            //                        attachment.FileStringByte = Convert.ToBase64String(fileBytes);
            //                        attachment.FileType = file.ContentType;
            //                        ongoingInteraction.AutoSaveData.Attachments.Add(attachment);
            //                    }
            //                }

            //                //if (Directory.Exists(conversationid) == false)
            //                //{
            //                //    Directory.CreateDirectory(conversationid);
            //                //}

            //                //var filename = Path.GetFileName(file.FileName);
            //                //var path = Path.Combine(conversationid, filename);


            //                //if (System.IO.File.Exists(path))
            //                //{
            //                //    System.IO.File.Delete(path);
            //                //}

            //                //var stream = new FileStream(path, FileMode.Create);
            //                //await file.CopyToAsync(stream);
            //                //stream.Dispose();
            //                //stream.Close();
            //            }
            //        }

            //        ongoingInteraction.AutoSaveData.Files = null;

            //        HttpContext.Session.Remove($"ConversationId:{conversationid}");
            //        HttpContext.Session.SetString($"ConversationId:{conversationid}", JsonConvert.SerializeObject(ongoingInteraction));
            //    }
            //}
        }
        catch (Exception ex)
        {

        }
    }

    public JsonResult GetOnInteractions()
    {
        List<string> interactions = new List<string>();
        try
        {
            ISession session = HttpContext.Session;

            foreach (string key in session.Keys)
            {
                if (key.Contains("ConversationId:"))
                {
                    var conversationId = key.Replace("ConversationId:", string.Empty);
                    interactions.Add(conversationId);
                }
            }
        }
        catch (Exception ex)
        { }

        return new JsonResult(interactions);
    }

    //public JsonResult OnInteraction()
    //{
    //    List<OngoingInteraction> interactions = new List<OngoingInteraction>();
    //    try
    //    {
    //        ISession session = HttpContext.Session;

    //        foreach (string key in session.Keys)
    //        {
    //            if (key.Contains("ConversationId:"))
    //            {
    //                var conversationId = key.Replace("ConversationId:", string.Empty);

    //                var ongoingInteractionSession = HttpContext.Session.GetString($"ConversationId:{conversationId}");

    //                if (ongoingInteractionSession != null)
    //                {
    //                    var ongoingInteraction = JsonConvert.DeserializeObject<OngoingInteraction>(ongoingInteractionSession);

    //                    if (ongoingInteraction != null)
    //                    {
    //                        interactions.Add(ongoingInteraction);
    //                    }
    //                }
    //            }
    //        }
    //    }
    //    catch (Exception ex)
    //    { }

    //    return new JsonResult(interactions);
    //}

    public JsonResult OnInteraction()
    {
        return new JsonResult(ongoingInteractions);
    }

    public JsonResult GetInteractionByUser(string user)
    {
        _logHelperService.LogMessage($"[GetInteractionByUser] Usuário: {user}");
        try
        {
            _logHelperService.LogMessage($"[GetInteractionByUser][{user}] ongoingInteractions count: {ongoingInteractions.Count}");
            if (ongoingInteractions.Count > 0)
            {
                var ongoingInteraction = ongoingInteractions.Where(_ => _.userEmail == user).FirstOrDefault();

                if (ongoingInteraction != null)
                {
                    _logHelperService.LogMessage($"[GetInteractionByUser][{user}] ongoingInteractions: {JsonConvert.SerializeObject(ongoingInteraction)}");
                    return new JsonResult(ongoingInteraction);
                }
            }
        }
        catch (Exception ex)
        {
            _logHelperService.LogException(ex);
        }

        return new JsonResult(null);
    }

    private async Task CreateClassification(TicketModel model, string conversationId)
    {
        try
        {
            TicketModel reason = new TicketModel();
            reason.Id = model.Id;
            reason.User = model.User;

            var classificationId = model.ClassificationTree.ClassificationReasonId;
            reason.ClassificationTree.ClassificationReasonListItemId = model.ClassificationTree.ClassificationReasonListItemId;

            _logHelperService.LogMessage($"[{conversationId}] Classificação {JsonConvert.SerializeObject(model.ClassificationTree)}");

            if (classificationId != null)
            {
                if (classificationId.ToUpper() != "OPÇÕES")
                {
                    if (classificationId.Contains(":"))
                    {
                        int index = classificationId.IndexOf(":");

                        if (index > 0)
                        {
                            reason.ClassificationTree.ClassificationReasonId = classificationId.Substring(0, index);
                        }
                    }
                    else
                    {
                        reason.ClassificationTree.ClassificationReasonId = classificationId;
                    }

                    var ret = await _ticketService.InsertTicketClassificationAPIAsync(_mapper.Map<TicketClassificationCreateAPI>(_mapper.Map<TicketClassificationCreateAPI>(reason)));

                    TicketModel submotive01 = new TicketModel();
                    submotive01.Id = reason.Id;
                    submotive01.User = model.User;

                    classificationId = model.ClassificationTree.ClassificationSubmotive01Id;
                    submotive01.ClassificationTree.ClassificationReasonListItemId = model.ClassificationTree.ClassificationSubmotive01ListItemId;

                    if (classificationId != null)
                    {
                        if (classificationId.ToUpper() != "OPÇÕES")
                        {
                            if (classificationId.Contains(":"))
                            {
                                int index = classificationId.IndexOf(":");

                                if (index > 0)
                                {
                                    submotive01.ClassificationTree.ClassificationReasonId = classificationId.Substring(0, index);
                                }
                            }
                            else
                            {
                                submotive01.ClassificationTree.ClassificationReasonId = classificationId;
                            }

                            ret = await _ticketService.InsertTicketClassificationAPIAsync(_mapper.Map<TicketClassificationCreateAPI>(_mapper.Map<TicketClassificationCreateAPI>(submotive01)));


                            TicketModel submotive02 = new TicketModel();
                            submotive02.Id = reason.Id;
                            submotive02.User = model.User;

                            submotive02.ClassificationTree.ClassificationReasonListItemId = model.ClassificationTree.ClassificationSubmotive01ListItemId;

                            classificationId = model.ClassificationTree.ClassificationSubmotive02Id;

                            if (classificationId != null)
                            {
                                if (classificationId.ToUpper() != "OPÇÕES")
                                {
                                    if (classificationId.Contains(":"))
                                    {
                                        int index = classificationId.IndexOf(":");

                                        if (index > 0)
                                        {
                                            submotive02.ClassificationTree.ClassificationReasonId = classificationId.Substring(0, index);
                                        }
                                    }
                                    else
                                    {
                                        submotive02.ClassificationTree.ClassificationReasonId = classificationId;
                                    }
                                    ret = await _ticketService.InsertTicketClassificationAPIAsync(_mapper.Map<TicketClassificationCreateAPI>(_mapper.Map<TicketClassificationCreateAPI>(submotive02)));

                                }
                            }

                        }
                    }


                }
            }
        }
        catch (Exception ex)
        {
            _logHelperService.LogError($"[{conversationId}] Erro registrando classificação.");
            _logHelperService.LogException(ex);
        }

    }


    //[HttpPost]
    //public async Task<IActionResult> GenesysInteractionEvent([FromForm] List<TicketModel> model)
    //{
    //    try
    //    {
    //        var ticketModel = model.FirstOrDefault();

    //        if (ticketModel != null)
    //        {
    //            var conversationId = ticketModel.ConversationId;
    //            _logHelperService.LogMessage($"[{conversationId}] Ticket: {JsonConvert.SerializeObject(ticketModel)}");

    //            var ongoingInteractionsSession = HttpContext.Session.GetString("OngoingInteractions");

    //            if (ongoingInteractionsSession != null)
    //            {
    //                var ongoingInteractions = JsonConvert.DeserializeObject<OngoingInteractions>(ongoingInteractionsSession);

    //                _logHelperService.LogMessage($"[{conversationId}] Session: {JsonConvert.SerializeObject(ongoingInteractions)}");

    //                if (ongoingInteractions != null)
    //                {
    //                    var ongoingInteraction = ongoingInteractions.FirstOrDefault(o => o.ConversationId == ticketModel.ConversationId);

    //                    _logHelperService.LogMessage($"[{conversationId}] encontrado na lista {JsonConvert.SerializeObject(ongoingInteraction)}");

    //                    var useridSession = HttpContext.Session.GetString("UserId");
    //                    if (useridSession != null)
    //                    {
    //                        _logHelperService.LogMessage($"[{conversationId}] usuário da sessão: {useridSession}");
    //                        ticketModel.User = useridSession;
    //                    }



    //                    var ticketCreateAPI = _mapper.Map<TicketCreateAPI>(ticketModel);
    //                    _logHelperService.LogMessage($"[{conversationId}] Ticket para ser salvo: {JsonConvert.SerializeObject(ticketCreateAPI)}");

    //                    var ticket_ret = await _ticketService.InsertTicketAPIAsync(ticketCreateAPI);

    //                    _logHelperService.LogMessage($"[{conversationId}] Ticket {JsonConvert.SerializeObject(ticketCreateAPI)}. RETORNO: {ticket_ret}");

    //                    if (ticket_ret > 0)
    //                    {
    //                        TicketModel reason = new TicketModel();
    //                        reason.Id = ticket_ret;
    //                        reason.User = useridSession;

    //                        try
    //                        {
    //                            var classificationId = ticketModel.ClassificationTree.ClassificationReasonId;
    //                            reason.ClassificationTree.ClassificationReasonListItemId = ticketModel.ClassificationTree.ClassificationReasonListItemId;

    //                            _logHelperService.LogMessage($"[{conversationId}] Classificação {JsonConvert.SerializeObject(ticketModel.ClassificationTree)}");

    //                            if (classificationId != null)
    //                            {
    //                                if (classificationId.ToUpper() != "OPÇÕES")
    //                                {
    //                                    if (classificationId.Contains(":"))
    //                                    {
    //                                        int index = classificationId.IndexOf(":");

    //                                        if (index > 0)
    //                                        {
    //                                            reason.ClassificationTree.ClassificationReasonId = classificationId.Substring(0, index);
    //                                        }
    //                                    }
    //                                    else
    //                                    {
    //                                        reason.ClassificationTree.ClassificationReasonId = classificationId;
    //                                    }

    //                                    var ret = await _ticketService.InsertTicketClassificationAPIAsync(_mapper.Map<TicketClassificationCreateAPI>(_mapper.Map<TicketClassificationCreateAPI>(reason)));

    //                                    TicketModel submotive01 = new TicketModel();
    //                                    submotive01.Id = reason.Id;
    //                                    submotive01.User = useridSession;

    //                                    classificationId = ticketModel.ClassificationTree.ClassificationSubmotive01Id;
    //                                    submotive01.ClassificationTree.ClassificationReasonListItemId = ticketModel.ClassificationTree.ClassificationSubmotive01ListItemId;

    //                                    if (classificationId != null)
    //                                    {
    //                                        if (classificationId.ToUpper() != "OPÇÕES")
    //                                        {
    //                                            if (classificationId.Contains(":"))
    //                                            {
    //                                                int index = classificationId.IndexOf(":");

    //                                                if (index > 0)
    //                                                {
    //                                                    submotive01.ClassificationTree.ClassificationReasonId = classificationId.Substring(0, index);
    //                                                }
    //                                            }
    //                                            else
    //                                            {
    //                                                submotive01.ClassificationTree.ClassificationReasonId = classificationId;
    //                                            }

    //                                            ret = await _ticketService.InsertTicketClassificationAPIAsync(_mapper.Map<TicketClassificationCreateAPI>(_mapper.Map<TicketClassificationCreateAPI>(submotive01)));


    //                                            TicketModel submotive02 = new TicketModel();
    //                                            submotive02.Id = reason.Id;
    //                                            submotive02.User = useridSession;

    //                                            submotive02.ClassificationTree.ClassificationReasonListItemId = ticketModel.ClassificationTree.ClassificationSubmotive01ListItemId;

    //                                            classificationId = ticketModel.ClassificationTree.ClassificationSubmotive02Id;

    //                                            if (classificationId != null)
    //                                            {
    //                                                if (classificationId.ToUpper() != "OPÇÕES")
    //                                                {
    //                                                    if (classificationId.Contains(":"))
    //                                                    {
    //                                                        int index = classificationId.IndexOf(":");

    //                                                        if (index > 0)
    //                                                        {
    //                                                            submotive02.ClassificationTree.ClassificationReasonId = classificationId.Substring(0, index);
    //                                                        }
    //                                                    }
    //                                                    else
    //                                                    {
    //                                                        submotive02.ClassificationTree.ClassificationReasonId = classificationId;
    //                                                    }
    //                                                    ret = await _ticketService.InsertTicketClassificationAPIAsync(_mapper.Map<TicketClassificationCreateAPI>(_mapper.Map<TicketClassificationCreateAPI>(submotive02)));

    //                                                }
    //                                            }

    //                                        }
    //                                    }


    //                                }
    //                            }
    //                        }
    //                        catch (Exception ex)
    //                        {
    //                            _logHelperService.LogError($"[{conversationId}] Erro registrando classificação.");
    //                            _logHelperService.LogException(ex);
    //                        }







    //                        HttpContext.Session.Remove("OngoingInteractions");

    //                        //if (ret > 0)
    //                        //{

    //                        //    //var ongoingInteractionsSession = HttpContext.Session.GetString("OngoingInteractions");

    //                        //    //if (ongoingInteractionsSession != null)
    //                        //    //{
    //                        //    //    var ongoingInteractions = JsonConvert.DeserializeObject<OngoingInteractions>(ongoingInteractionsSession);

    //                        //    //    if (ongoingInteractions != null && ongoingInteractions.Count > 0)
    //                        //    //    {
    //                        //    //        var ticketsObj = ongoingInteractions.Where(_ => _.ConversationId != ticketModel.ConversationId).ToList();

    //                        //    //        HttpContext.Session.Remove("OngoingInteractions");

    //                        //    //        if (ticketsObj != null && ticketsObj.Count > 0)
    //                        //    //        {
    //                        //    //            var ticketsJson = JsonConvert.SerializeObject(ticketsObj);
    //                        //    //            HttpContext.Session.SetString("OngoingInteractions", ticketsJson);
    //                        //    //        }
    //                        //    //    }
    //                        //    //}


    //                        //}
    //                        var username = HttpContext.Session.GetString("Username");

    //                        LogController logController = new LogController();
    //                        logController.Id = ticket_ret;
    //                        logController.Module = MODULE_NAME;
    //                        logController.Section = username == null ? string.Empty : username;
    //                        logController.Field = "Todos";
    //                        logController.Value = "Todos";
    //                        logController.UserId = useridSession != null ? Int64.Parse(useridSession) : 0;
    //                        logController.Action = "Criar";

    //                        await _logControllerService.InsertLogAPIAsync(logController);

    //                        ViewBag.TicketMessage = "Chamado Criado com sucesso";



    //                        return View(new List<TicketModel>());
    //                    }
    //                }
    //                else
    //                {
    //                    ViewBag.TicketMessage = "Erro Chamado sem Conversação!";
    //                    _logHelperService.LogMessage($"[GenesysInteractionEvent] Erro Chamado sem Conversação!");
    //                    return View(new List<TicketModel>());
    //                }
    //            }
    //            else
    //            {
    //                _logHelperService.LogMessage($"[GenesysInteractionEvent] Session: null");

    //            }
    //        }
    //        else
    //        {
    //            _logHelperService.LogMessage($"[GenesysInteractionEvent] Ticket: null");

    //        }
    //    }
    //    catch (Exception ex)
    //    {

    //    }


    //    foreach (var interaction in model)
    //    {
    //        await LoadListOptions(interaction);
    //    }

    //    //ModelState.AddModelError("", "Falha ao Salvar o Chamado");
    //    ViewBag.TicketMessage = "Erro Criando Chamado!";

    //    return View(model);
    //}

    public async Task<IActionResult> Index(string searchTicketType, string searchTicketText)
    {
        if (IsAuthenticated() == false)
        {
            return RedirectToAction("Create", "Login");
        }

        ViewBag.ShowPageControl = false;
        try
        {
            if (searchTicketType.Contains("Tipo Filtro") || searchTicketText == string.Empty)
            {
                ViewBag.AlertFilter = "SHOW";

                var allTickets = await _ticketService.GetTicketListAPIAsync();

                if (allTickets != null && allTickets.Any())
                {
                    var TicketsModel = _mapper.Map<List<TicketModel>>(allTickets);
                    return View(TicketsModel);
                }
            }

            //var users = await _userService.GetAllUserAsync();
            var Tickets = await _ticketService.SearchTicketAPIAsync(searchTicketType, searchTicketText);

            if (Tickets != null && Tickets.Any())
            {
                var TicketsModel = _mapper.Map<List<TicketModel>>(Tickets);
                return View(TicketsModel);
            }

        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
        }
        return View();
    }

    public async Task<IActionResult> Search(string searchTicketType, string searchTicketText)
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
            if (searchTicketType.Contains("Tipo Filtro") || searchTicketText == string.Empty)
            {
                ViewBag.AlertFilter = "SHOW";

                var allTickets = await _ticketService.GetTicketListAPIAsync();

                if (allTickets != null && allTickets.Any())
                {
                    var TicketsModel = _mapper.Map<List<TicketModel>>(allTickets);
                    return View(TicketsModel);
                }
            }

            //var users = await _userService.GetAllUserAsync();
            var Tickets = await _ticketService.SearchTicketAPIAsync(searchTicketType, searchTicketText);

            if (Tickets != null && Tickets.Any())
            {
                var TicketsModel = _mapper.Map<List<TicketModel>>(Tickets);
                return View(TicketsModel);
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
    public async Task<IActionResult> BindTicket(string id, string childProtocol)
    {
        if (IsAuthenticated() == false)
        {
            return RedirectToAction("Create", "Login");
        }

        ViewBag.ChildProtocol = childProtocol;
        //ViewBag.ChildTicketId = childTicketId;
        var data = await _ticketService.GetTicketAPIAsync(long.Parse(id));

        if (data != null)
        {
            var model = _mapper.Map<TicketModel>(data);


            //if (data.TicketClassification != null && data.TicketClassification.Count > 0)
            //{
            //    model.ClassificationTree.ClassificationDemandId = data.TicketClassification.FirstOrDefault()!.ClassificationDemandId.ToString();
            //    model.ClassificationTree.ClassificationDemandTypeId = data.TicketClassification.FirstOrDefault()!.ClassificationTypeId.ToString();

            //    for (int i = 0; i < data.TicketClassification.Count; i++)
            //    {
            //        switch (i)
            //        {
            //            case 0:
            //                model.ClassificationTree.ClassificationReasonId = data.TicketClassification[i].ClassificationReasonId.ToString();
            //                model.ClassificationTree.ClassificationReasonListItemId = data.TicketClassification[i].ClassificationReasonListItemId.ToString();
            //                break;
            //            case 1:
            //                model.ClassificationTree.ClassificationSubmotive01Id = data.TicketClassification[i].ClassificationReasonId.ToString();
            //                model.ClassificationTree.ClassificationSubmotive01ListItemId = data.TicketClassification[i].ClassificationReasonListItemId.ToString();
            //                break;
            //            case 2:
            //                model.ClassificationTree.ClassificationSubmotive02Id = data.TicketClassification[i].ClassificationReasonId.ToString();
            //                model.ClassificationTree.ClassificationSubmotive02ListItemId = data.TicketClassification[i].ClassificationReasonListItemId.ToString();
            //                break;
            //        }
            //    }
            //}

            string? profile = HttpContext.Session.GetString("Profile");
            profile = profile ?? string.Empty;

            await LoadListOptions(model, profile);

            model.TicketDataToCompareIfChanged.Id = model.Id;
            model.TicketDataToCompareIfChanged.QueueGTId = model.QueueGTId;
            model.TicketDataToCompareIfChanged.QueueGT = model.QueueGT;
            model.TicketDataToCompareIfChanged.AnySolution = model.AnySolution;
            model.TicketDataToCompareIfChanged.TicketStatusId = model.TicketStatusId;
            model.TicketDataToCompareIfChanged.TicketStatus = model.TicketStatus;
            model.TicketDataToCompareIfChanged.TicketCriticalityId = model.TicketCriticalityId;
            model.TicketDataToCompareIfChanged.TicketCriticality = model.TicketCriticality;
            model.TicketDataToCompareIfChanged.DemandObservation = model.DemandObservation;
            model.TicketDataToCompareIfChanged.DemandTypeId = model.DemandTypeId;
            model.TicketDataToCompareIfChanged.DemandType = model.DemandType;

            return PartialView("~/Views/Ticket/_Journey.cshtml", model);
        }

        return PartialView("~/Views/Ticket/_Journey.cshtml");
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

    public async Task LoadListOptions(TicketModel model, string perfil = "")
    {
        try
        {
            List<SelectListItem> classificationCollection = new List<SelectListItem>();
            //var classificationDemandList = await _classificationDemandService.GetClassficationDemandListAPIAsync();

            //if (classificationDemandList != null && model.TicketClassification != null)
            //{
            //    var classificationDemandId = model.TicketClassification.FirstOrDefault()?.ClassificationDemandId;

            //    var classificationDemandModel = _mapper.Map<List<ClassificationDemandModel>>(classificationDemandList);

            //    foreach (var classificationDemand in classificationDemandModel)
            //    {
            //        bool bSelected = classificationDemandId == classificationDemand.Id ? true : false;

            //        var selectListItem = new SelectListItem
            //        {
            //            Value = classificationDemand.Id.ToString(),
            //            Text = classificationDemand.Name,
            //            Selected = bSelected
            //        };
            //        classificationCollection.Add(selectListItem);
            //    }
            //}

            List<SelectListItem> yesNoItemCollection = new List<SelectListItem>();
            yesNoItemCollection.Add(new SelectListItem
            {
                //Value = accessprofile.Id.ToString(),
                Value = "",
                Text = "Opções",
            });

            yesNoItemCollection.Add(new SelectListItem
            {
                //Value = accessprofile.Id.ToString(),
                Value = "0",
                Text = "Não",
            });

            yesNoItemCollection.Add(new SelectListItem
            {
                //Value = accessprofile.Id.ToString(),
                Value = "1",
                Text = "Sim",
            });

            List<SelectListItem> ticketStatusCollection = new List<SelectListItem>();

            List<TicketAgentStatusAPI> ticketAgentStatusAPIList;
            List<TicketAssistentStatusAPI> ticketAssistentStatusAPIList;
            List<TicketStatusAPI> ticketStatusList;
            List<TicketStatusModel>? ticketStatusModel = null;

            switch (perfil.ToUpper())
            {
                case "AGENTE":
                    ticketAgentStatusAPIList = await _ticketService.GetTicketAgentStatusAsync();
                    ticketStatusModel = _mapper.Map<List<TicketStatusModel>>(ticketAgentStatusAPIList);
                    break;
                case "ASSISTENTE":
                    ticketAssistentStatusAPIList = await _ticketService.GetTicketAssistentStatusAsync();
                    ticketStatusModel = _mapper.Map<List<TicketStatusModel>>(ticketAssistentStatusAPIList);
                    break;
                default:
                    ticketStatusList = await _ticketService.GetTicketStatusAPIAsync();
                    ticketStatusModel = _mapper.Map<List<TicketStatusModel>>(ticketStatusList);
                    break;
            }



            if (ticketStatusModel != null)
            {
                ticketStatusCollection.Add(new SelectListItem
                {
                    Value = "",
                    Text = "Opções",
                });

                foreach (var ticketStatus in ticketStatusModel)
                {
                    bool bSelected = ticketStatus.Id == model.TicketStatusId ? true : false;
                    var selectListItem = new SelectListItem
                    {
                        //Value = accessprofile.Id.ToString(),
                        Value = ticketStatus.Id.ToString(),
                        Text = ticketStatus.Name,
                        Selected = bSelected
                    };
                    ticketStatusCollection.Add(selectListItem);
                }
            }

            List<SelectListItem> ticketCriticalityCollection = new List<SelectListItem>();
            var ticketCriticalityList = await _ticketService.GetTicketCriticalityAPIAsync();

            if (ticketCriticalityList != null)
            {
                var ticketCriticalityModel = _mapper.Map<List<TicketCriticalityModel>>(ticketCriticalityList);

                ticketCriticalityCollection.Add(new SelectListItem
                {
                    Value = "",
                    Text = "Opções",
                });

                foreach (var ticketCriticality in ticketCriticalityModel)
                {
                    bool bSelected = ticketCriticality.Id == model.TicketCriticalityId ? true : false;
                    var selectListItem = new SelectListItem
                    {
                        //Value = accessprofile.Id.ToString(),
                        Value = ticketCriticality.Id.ToString(),
                        Text = ticketCriticality.Name,
                        Selected = bSelected
                    };
                    ticketCriticalityCollection.Add(selectListItem);
                }
            }

            List<SelectListItem> queueGTCollection = new List<SelectListItem>();
            var queueGTList = await _ticketService.GetTicketQueueGTAPIAsync();

            if (queueGTList != null)
            {
                var ticketQueueGTModel = _mapper.Map<List<TicketQueueGTModel>>(queueGTList);

                foreach (var ticketQueueGT in ticketQueueGTModel)
                {
                    bool bSelected = ticketQueueGT.Id == model.QueueGTId ? true : false;

                    var selectListItem = new SelectListItem
                    {
                        //Value = accessprofile.Id.ToString(),
                        Value = ticketQueueGT.Id.ToString(),
                        Text = ticketQueueGT.Name,
                        Selected = bSelected
                    };
                    queueGTCollection.Add(selectListItem);
                }
            }

            List<SelectListItem> demandTypeCollection = new List<SelectListItem>();

            var demandTypeList = await _demandTypeService.GetDemandTypeActivatedListAPIAsync();

            if (demandTypeList != null)
            {
                var demandTypeModel = _mapper.Map<List<DemandTypeModel>>(demandTypeList);

                demandTypeCollection.Add(new SelectListItem
                {
                    Value = "",
                    Text = "Opções",
                });

                foreach (var demandType in demandTypeModel)
                {
                    bool bSelected = demandType.Id == model.DemandTypeId ? true : false;
                    var selectListItem = new SelectListItem
                    {
                        //Value = accessprofile.Id.ToString(),
                        Value = demandType.Id.ToString(),
                        Text = demandType.Name,
                        Selected = bSelected
                    };
                    demandTypeCollection.Add(selectListItem);
                }
            }

            model.TicketStatusOptions.AddRange(ticketStatusCollection);

            model.TicketCriticalityOptions.AddRange(ticketCriticalityCollection);

            model.ClassificationTree.ClassificationDemandOptions.AddRange(classificationCollection);

            model.AnySolutionOptions.AddRange(yesNoItemCollection);

            model.QueueGTOptions.AddRange(queueGTCollection);

            model.DemandTypeOptions.AddRange(demandTypeCollection);
        }
        catch (Exception ex)
        {
        }
    }

    //private async Task LoadClassificationDemandTypeOptions(TicketModel model)
    //{
    //    try
    //    {
    //        Int64 classificationDemandTypeId = model.TicketClassification.FirstOrDefault().ClassificationTypeId;

    //        model.ClassificationTree.ClassificationDemandTypeId = classificationDemandTypeId.ToString();

    //        Int64 classificationDemandId = model.TicketClassification.FirstOrDefault().ClassificationDemandId;

    //        var classification = await _classificationDemandTypeService.GetClassificationDemandTypeListAPIAsync(classificationDemandId);

    //        if (classification != null)
    //        {
    //            var classificationlList = _mapper.Map<List<ClassificationDemandTypeModel>>(classification);

    //            foreach (var item in classificationlList)
    //            {
    //                bool bSelected = classificationDemandTypeId == item.Id ? true : false;

    //                var selectListItem = new SelectListItem
    //                {
    //                    Value = item.Id.ToString(),
    //                    Text = item.Name,
    //                    Selected = bSelected
    //                };
    //                model.ClassificationTree.ClassificationDemandTypeOptions.Add(selectListItem);
    //            }
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //    }
    //}

    //private async Task LoadClassificationReasonOptions(TicketModel model)
    //{
    //    try
    //    {
    //        Int64 classificationDemandTypeId = model.TicketClassification.FirstOrDefault().ClassificationTypeId;
    //        Int64 classificationReasonId = model.TicketClassification.FirstOrDefault().ClassificationReasonId;

    //        model.ClassificationTree.ClassificationReasonId = classificationReasonId.ToString();

    //        var classification = await _classificationReasonService.GetClassificationReasonAPIAsync(classificationDemandTypeId);

    //        if (classification != null)
    //        {
    //            var classificationlList = _mapper.Map<List<ClassificationReasonModel>>(classification);

    //            foreach (var item in classificationlList)
    //            {
    //                bool bSelected = classificationReasonId == item.Id ? true : false;
    //                var selectListItem = new SelectListItem
    //                {
    //                    Value = item.Id.ToString(),
    //                    Text = item.Name,
    //                    Selected = bSelected
    //                };
    //                model.ClassificationTree.ClassificationReasonOptions.Add(selectListItem);
    //            }
    //        }

    //    }
    //    catch (Exception ex)
    //    {
    //    }
    //}

    //private async Task LoadClassificationReasonListItemOptions(TicketModel model)
    //{
    //    try
    //    {
    //        Int64 classificationReasonListId = model.TicketClassification.FirstOrDefault().ClassificationReasonListId;
    //        Int64 classificationReasonListItemId = model.TicketClassification.FirstOrDefault().ClassificationReasonListItemId;

    //        model.ClassificationTree.ClassificationReasonListItemId = classificationReasonListItemId.ToString();

    //        var classification = await _classificationListItemsService.GetClassificationListItemsActiveAPIAsync(classificationReasonListId);

    //        if (classification != null)
    //        {
    //            var classificationlList = _mapper.Map<List<ClassificationListItemsModel>>(classification);

    //            foreach (var item in classificationlList)
    //            {
    //                bool bSelected = classificationReasonListItemId == item.Id ? true : false;
    //                var selectListItem = new SelectListItem
    //                {
    //                    Value = item.Id.ToString(),
    //                    Text = item.Name,
    //                    Selected = bSelected
    //                };
    //                model.ClassificationTree.ClassificationReasonListItemOptions.Add(selectListItem);
    //            }
    //        }

    //    }
    //    catch (Exception ex)
    //    {
    //    }
    //}

    //private async Task LoadClassificationSubmotive01Options(TicketModel model)
    //{
    //    try
    //    {
    //        Int64 classificationReasonId = model.TicketClassification.FirstOrDefault().ClassificationReasonId;
    //        Int64 classificationReasonSubmotive01Id = model.TicketClassification[1].ClassificationReasonId;

    //        model.ClassificationTree.ClassificationSubmotive01Id = classificationReasonSubmotive01Id.ToString();


    //        var classification = await _classificationReasonService.GetClassificationReasonListChildrenAPIAsync(classificationReasonId);

    //        if (classification != null)
    //        {
    //            var classificationlList = _mapper.Map<List<ClassificationReasonChildrenModel>>(classification);

    //            foreach (var item in classificationlList)
    //            {
    //                bool bSelected = classificationReasonSubmotive01Id == item.Id ? true : false;
    //                var selectListItem = new SelectListItem
    //                {
    //                    Value = item.Id.ToString(),
    //                    Text = item.Name,
    //                    Selected = bSelected
    //                };
    //                model.ClassificationTree.ClassificationSubmotive01Options.Add(selectListItem);
    //            }
    //        }

    //    }
    //    catch (Exception ex)
    //    {
    //    }
    //}

    //private async Task LoadClassificationSubmotive01ListItemOptions(TicketModel model)
    //{
    //    try
    //    {
    //        Int64 classificationReasonListId = model.TicketClassification[1].ClassificationReasonListId;
    //        Int64 classificationReasonListItemId = model.TicketClassification[1].ClassificationReasonListItemId;

    //        model.ClassificationTree.ClassificationSubmotive01ListItemId = classificationReasonListItemId.ToString();

    //        var classification = await _classificationListItemsService.GetClassificationListItemsActiveAPIAsync(classificationReasonListId);

    //        if (classification != null)
    //        {
    //            var classificationlList = _mapper.Map<List<ClassificationListItemsModel>>(classification);

    //            foreach (var item in classificationlList)
    //            {
    //                bool bSelected = classificationReasonListItemId == item.Id ? true : false;
    //                var selectListItem = new SelectListItem
    //                {
    //                    Value = item.Id.ToString(),
    //                    Text = item.Name,
    //                    Selected = bSelected
    //                };
    //                model.ClassificationTree.ClassificationSubmotive01ListItemOptions.Add(selectListItem);
    //            }
    //        }

    //    }
    //    catch (Exception ex)
    //    {
    //    }
    //}

    //private async Task LoadClassificationSubmotive02Options(TicketModel model)
    //{
    //    try
    //    {
    //        Int64 classificationReasonId = model.TicketClassification[1].ClassificationReasonId;
    //        Int64 classificationReasonSubmotive02Id = model.TicketClassification[2].ClassificationReasonId;

    //        model.ClassificationTree.ClassificationSubmotive02Id = classificationReasonSubmotive02Id.ToString();


    //        var classification = await _classificationReasonService.GetClassificationReasonListChildrenAPIAsync(classificationReasonId);

    //        if (classification != null)
    //        {
    //            var classificationlList = _mapper.Map<List<ClassificationReasonChildrenModel>>(classification);

    //            foreach (var item in classificationlList)
    //            {
    //                bool bSelected = classificationReasonId == item.ParentReasonId ? true : false;
    //                var selectListItem = new SelectListItem
    //                {
    //                    Value = item.Id.ToString(),
    //                    Text = item.Name,
    //                    Selected = bSelected
    //                };
    //                model.ClassificationTree.ClassificationSubmotive02Options.Add(selectListItem);
    //            }
    //        }

    //    }
    //    catch (Exception ex)
    //    {
    //    }
    //}

    //private async Task LoadClassificationSubmotive02ListItemOptions(TicketModel model)
    //{
    //    try
    //    {
    //        Int64 classificationReasonId = model.TicketClassification[2].ClassificationReasonId;
    //        Int64 classificationReasonListItemId = model.TicketClassification[2].ClassificationReasonListItemId;

    //        model.ClassificationTree.ClassificationSubmotive02ListItemId = classificationReasonListItemId.ToString();

    //        var classification = await _classificationListItemsService.GetClassificationListItemsActiveAPIAsync(classificationReasonId);

    //        if (classification != null)
    //        {
    //            var classificationlList = _mapper.Map<List<ClassificationListItemsModel>>(classification);

    //            foreach (var item in classificationlList)
    //            {
    //                bool bSelected = classificationReasonListItemId == item.Id ? true : false;
    //                var selectListItem = new SelectListItem
    //                {
    //                    Value = item.Id.ToString(),
    //                    Text = item.Name,
    //                    Selected = bSelected
    //                };
    //                model.ClassificationTree.ClassificationSubmotive02ListItemOptions.Add(selectListItem);
    //            }
    //        }

    //    }
    //    catch (Exception ex)
    //    {
    //    }
    //}

    public async Task<JsonResult> IsInteractionWithMe(string protocol, string email)
    {
        try
        {
            var interaction = ongoingInteractions.Where(_ => _.Protocol == protocol && _.userEmail == email).FirstOrDefault();

            if (interaction != null)
            {
                return new JsonResult(new { IsInteractionWithMe = true });
            }

            var ticket = await _ticketService.GetTicketAPIAsync(protocol);
            if (ticket != null && ticket.Protocol != null)
            {
                return new JsonResult(new { IsInteractionWithMe = true });
            }
        }
        catch (Exception ex)
        {
        }

        return new JsonResult(new { IsInteractionWithMe = false });
    }

    public JsonResult MyInteractions(string email)
    {
        List<OngoingInteraction> myInteractions = new List<OngoingInteraction>();
        try
        {
            var interactions = ongoingInteractions.Where(_ => _.userEmail == email).ToList();

            if (interactions != null && interactions.Count > 0)
            {
                myInteractions.AddRange(interactions);
            }
        }
        catch (Exception ex)
        {
        }

        return new JsonResult(new { myInteractions });
    }

    public JsonResult MyInteraction(string email, string conversationid)
    {
        try
        {
            var interaction = ongoingInteractions.FirstOrDefault(_ => _.userEmail == email && _.ConversationId == conversationid);

            if (interaction != null)
            {
                return new JsonResult(interaction);
            }
        }
        catch (Exception ex)
        {
        }

        return new JsonResult(null);
    }

    private void CreateDataPagination(int currentpage, int linenumbers, int sizepage, int totalitems, bool showPageControl = false)
    {
        int uniquePagesNumber = 9;
        int startPageCounter = currentpage + 1;

        if (uniquePagesNumber > totalitems)
            return;

        ViewBag.ShowPageControl = showPageControl;

        try
        {
            int lastpage = ((int)(totalitems / sizepage));
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

            if (totalitems > 50)
            {
                List<int> previousFirstPages = new List<int>();
                for (int i = 0; i < uniquePagesNumber; i++)
                {
                    previousFirstPages.Add(startPageCounter + i);
                }

                ViewBag.PreviousFirstPages = previousFirstPages;
            }

        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
        }
    }

    private async Task UploadFiles(TicketModel model, string? username, string? userid)
    {
        try
        {
            if (model.Files != null)
            {
                foreach (var file in model.Files)
                {
                    //var content = new MultipartFormDataContent();
                    var filename = Path.GetFileName(file.FileName);
                    var filestream = new FileStream(filename, FileMode.Create);
                    //var filestream = System.IO.File.Open(filename, FileMode.Open);
                    //content.Add(new StreamContent(filestream), "file", filename);

                    var ticketAttachment = new TicketAttachmentCreateAPI();
                    ticketAttachment.TicketId = model.Id;
                    ticketAttachment.Filename = filename;

                    byte[] fileBytes = new byte[file.Length];
                    var stream = file.OpenReadStream();
                    await stream.ReadAsync(fileBytes, 0, (int)file.Length);
                    ticketAttachment.FileBase64 = Convert.ToBase64String(fileBytes);
                    await _ticketService.UploadTicketAttachmentAPIAsync(ticketAttachment);

                    if (string.IsNullOrEmpty(username) == false)
                    {
                        LogController logController = new LogController();
                        logController.Id = model.Id;
                        logController.Module = MODULE_NAME;
                        logController.Section = username == null ? string.Empty : username;
                        logController.Field = "Anexos";
                        logController.Value = filename;
                        logController.UserId = userid != null ? Int64.Parse(userid) : 0;
                        logController.Action = "Editar";

                        await _logControllerService.InsertLogAPIAsync(logController);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
        }
    }

    [HttpGet]
    public IActionResult GetAttachments(string conversationid)
    {
        List<AttachmentData> attachmentDatas = new List<AttachmentData>();
        try
        {
            var dir = Path.Combine(Directory.GetCurrentDirectory(), "Contents", conversationid);

            if (Directory.Exists(dir))
            {
                DirectoryInfo directoryInfo = new DirectoryInfo(dir);

                foreach (var file in directoryInfo.GetFiles())
                {
                    AttachmentData attachmentData = new AttachmentData();
                    attachmentData.ContentDisposition = $"form-data; name=\"Files\"; filename=\"{file.Name}\"";
                    attachmentData.FileName = file.Name;

                    byte[] fileBytes = System.IO.File.ReadAllBytes(Path.Combine(dir, file.Name));

                    attachmentData.FileContent = Convert.ToBase64String(fileBytes);
                    attachmentData.FileBytes = fileBytes;

                    switch (file.Extension.ToUpper())
                    {
                        case ".PDF":
                            attachmentData.ContentType = "application/pdf";
                            break;
                    }

                    attachmentDatas.Add(attachmentData);
                }

                if (attachmentDatas.Count > 0)
                    return Json(new { success = true, attachments = attachmentDatas });
            }

        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
        }

        return Json(new { success = false }); ;
    }

    [HttpGet]
    //[CustomAuthorize]
    public async Task<IActionResult> ViewTicket(string id, string message = "")
    {
        if (IsAuthenticated() == false)
        {
            return RedirectToAction("Create", "Login");
        }

        var data = await _ticketService.GetTicketAPIAsync(long.Parse(id));

        if (data != null)
        {
            var model = _mapper.Map<TicketModel>(data);

            var attachments = await _ticketService.DownloadTicketAttachmentAPIAsync(model.Id);

            if (attachments != null)
            {
                model.Attachments = _mapper.Map<List<TicketAttachmentModel>>(attachments);
            }

            string? profile = HttpContext.Session.GetString("Profile");
            profile = profile ?? string.Empty;

            await LoadListOptions(model, profile);

            if (string.IsNullOrEmpty(message) == false)
            {
                ViewBag.TicketMessage = message;
            }

            var ticketClassificationResult = data.TicketClassificationResult;

            if (ticketClassificationResult != null && ticketClassificationResult.Count > 0)
            {
                foreach (var item in ticketClassificationResult)
                {
                    TicketClassification ticketClassification = new TicketClassification();
                    ticketClassification.ManifestationTypeName = item.ManifestationTypeName;
                    ticketClassification.ServiceName = item.ServiceName;
                    ticketClassification.ServiceUnitName = item.ServiceUnitItemName;

                    if (item.Reasons != null && item.Reasons.Count > 0)
                    {
                        ticketClassification.Reason01Name = item.Reasons[0].ReasonName;
                    }

                    if (item.Reasons != null && item.Reasons.Count > 1)
                    {
                        ticketClassification.Reason02Name = item.Reasons[1].ReasonName;
                    }

                    model.TicketClassification.Add(ticketClassification);
                }
            }

            return View(model);
        }

        return View();
    }
}
