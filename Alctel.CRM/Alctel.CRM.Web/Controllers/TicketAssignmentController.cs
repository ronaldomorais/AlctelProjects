using System.Collections.Generic;
using Alctel.CRM.API.Entities;
using Alctel.CRM.Business.Interfaces;
using Alctel.CRM.Business.Services;
using Alctel.CRM.Context.InMemory.Entities;
using Alctel.CRM.Web.Models;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Alctel.CRM.Web.Controllers;

public class TicketAssignmentController : Controller
{
    private const string MODULE_NAME = "chamados";
    private readonly ILoginService _loginService;
    private readonly ITicketService _ticketService;
    private readonly IMapper _mapper;
    private readonly ITicketAssignmentService _ticketAssignmentService;
    private readonly ILogControllerService _logControllerService;

    public TicketAssignmentController(ILoginService loginService, ITicketService ticketService, IMapper mapper, ITicketAssignmentService ticketAssignmentService, ILogControllerService logControllerService)
    {
        _loginService = loginService;
        _ticketService = ticketService;
        _mapper = mapper;
        _ticketAssignmentService = ticketAssignmentService;
        _logControllerService = logControllerService;
    }

    [HttpGet]
    //[CustomAuthorize]
    public async Task<IActionResult> TicketManagement(string id)
    {
        //if (IsAuthenticated() == false)
        //{
        //    return RedirectToAction("Create", "Login");
        //}

        var data = await _ticketService.GetTicketAPIAsync(long.Parse(id));

        if (data != null)
        {
            var model = _mapper.Map<TicketModel>(data);

            if (model.AnySolution == "1")
                model.AnySolution = "Sim";
            else
                model.AnySolution = "Não";

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
                    ticketClassification.UserId = item.UserId;
                    ticketClassification.Username = item.Username;


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

            await LoadListOptions(model);


            return View(model);
        }

        return View();
    }

    [HttpGet]
    public async Task<JsonResult> GetQueueGTList()
    {
        List<TicketQueueGTModel> ticketQueueGTModels = new List<TicketQueueGTModel>();
        try
        {
            var ticketQueueResult = await _ticketService.GetTicketQueueGTAPIAsync();

            if (ticketQueueResult != null && ticketQueueResult.Count > 0)
            {
                ticketQueueGTModels = _mapper.Map<List<TicketQueueGTModel>>(ticketQueueResult);
            }
        }
        catch (Exception)
        { }

        return new JsonResult(ticketQueueGTModels);
    }

    [HttpPost]
    public async Task<JsonResult> ConfirmAssignment([FromForm] TicketAssignmentCreateModel assignmentTicketCreate)
    {
        try
        {
            if (assignmentTicketCreate != null)
            {
                string logValue = string.Empty;
                var profile = HttpContext.Session.GetString("Profile");

                if (profile != null && (profile.ToUpper() == "AGENTE" || profile.ToUpper() == "ASSISTENTE"))
                {
                    var userIdStr = HttpContext.Session.GetString("UserId");

                    if (string.IsNullOrEmpty(userIdStr) == false)
                    {
                        assignmentTicketCreate.UserDestId = Int64.Parse(userIdStr);
                    }
                }

                int ret = -1;
                if (assignmentTicketCreate.UserOriginId != 0)
                {
                    var ticketAssignment = _mapper.Map<TicketAssignmentUserCreateAPI>(assignmentTicketCreate);
                    ret = await _ticketAssignmentService.InsertTicketAssignmentUserAsync(ticketAssignment);
                    logValue = assignmentTicketCreate.UsernameOrigin ?? string.Empty;
                }
                else
                {
                    var ticketAssignment = _mapper.Map<TicketAssignmentQueueUserCreateAPI>(assignmentTicketCreate);
                    ret = await _ticketAssignmentService.InsertTicketAssignmentQueueUserAsync(ticketAssignment);
                    //logValue = assignmentTicketCreate.QueueGTOrigin ?? string.Empty;
                }

                if (ret > 0)
                {
                    var username = HttpContext.Session.GetString("Username");
                    var userid = HttpContext.Session.GetString("UserId");

                    LogController logController = new LogController();
                    logController.Id = assignmentTicketCreate.TicketId;
                    logController.Module = MODULE_NAME;
                    logController.Section = username == null ? string.Empty : username;
                    logController.Field = "Usuário";
                    logController.Value = logValue;
                    logController.UserId = userid != null ? Int64.Parse(userid) : 0;
                    logController.Action = "Atribuição";

                    await _logControllerService.InsertLogAPIAsync(logController);

                    return new JsonResult(new { IsAssigment = true });
                }
            }
        }
        catch (Exception)
        { }

        return new JsonResult(new { IsAssigment = false });
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

    public async Task LoadListOptions(TicketModel model)
    {
        try
        {
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
            var ticketStatusList = await _ticketService.GetTicketStatusAPIAsync();

            if (ticketStatusList != null)
            {
                var ticketStatusModel = _mapper.Map<List<TicketStatusModel>>(ticketStatusList);

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

            model.TicketStatusOptions.AddRange(ticketStatusCollection);

            model.TicketCriticalityOptions.AddRange(ticketCriticalityCollection);

            model.AnySolutionOptions.AddRange(yesNoItemCollection);

            model.QueueGTOptions.AddRange(queueGTCollection);

            model.DemandTypeOptions.AddRange(demandTypeCollection);
        }
        catch (Exception ex)
        {
        }
    }
}
