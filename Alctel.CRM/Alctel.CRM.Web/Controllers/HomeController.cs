using System.Diagnostics;
using System.Reflection;
using System.Security.Claims;
using Alctel.CRM.Business.Interfaces;
using Alctel.CRM.Business.Interfaces.Reason;
using Alctel.CRM.Business.Services;
using Alctel.CRM.Context.InMemory.Entities;
using Alctel.CRM.Context.InMemory.Entities.Classification;
using Alctel.CRM.GenesysCloudAPI.Interfaces;
using Alctel.CRM.Web.Customs;
using Alctel.CRM.Web.Models;
using Alctel.CRM.Web.Tools.LogHelper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Alctel.CRM.Web.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly ILogHelperService _logHelperService;
    private readonly IAccessProfileService _accessProfileService;
    private readonly IActionPermissionService _actionPermissionService;
    private readonly IModuleService _moduleService;
    private readonly IUserService _userService;
    private readonly IServiceUnitService _serviceUnitService;
    private readonly IAreaService _areaService;
    private readonly IServiceLevelService _serviceLevelService;
    private readonly IDemandTypeService _demandTypeService;
    private readonly IReasonListService _reasonListService;
    private readonly ILoginService _loginService;

    public HomeController(ILogger<HomeController> logger, IAccessProfileService accessProfileService, IActionPermissionService actionPermissionService, IModuleService moduleService, IUserService userService, IServiceUnitService serviceUnitService, IAreaService areaService, IServiceLevelService serviceLevelService, IDemandTypeService demandTypeService, IReasonListService reasonListService, ILogHelperService logHelperService, ILoginService loginService)
    {
        _logger = logger;
        _accessProfileService = accessProfileService;
        _actionPermissionService = actionPermissionService;
        _moduleService = moduleService;
        _userService = userService;
        _serviceUnitService = serviceUnitService;
        _areaService = areaService;
        _serviceLevelService = serviceLevelService;
        _demandTypeService = demandTypeService;
        _reasonListService = reasonListService;
        _logHelperService = logHelperService;
        _loginService = loginService;
    }

    [HttpGet]
    //[CustomAuthorize]
    public async Task<ActionResult> Index()
    {
        
        if (IsAuthenticated())
        {
            return View();
        }

        //await LoadActionsPermissionsAsync();
        //await LoadModulesAsync();
        //await LoadAccessProfileAsync();
        //await LoadUserAsync();
        //await LoadServiceUnitAsync();
        //await LoadAreaAsync();
        //await LoadServiveLevelAsync();
        //await LoadDemandTypeAsync();
        //await LoadClassificationListAsync();
        return RedirectToAction("Create", "Login");
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
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
                            if (profilesession.ToUpper() != profile.ToUpper())
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

    private async Task LoadAccessProfileAsync()
    {
        var profiles = await _accessProfileService.GetAllAccessProfileAsync();
        Random random = new Random();

        if (!(profiles != null && profiles.Any()))
        {
            var modules = await _moduleService.GetAllModuleAsync();
            var actionPermissions = await _actionPermissionService.GetAllActionPermissionAsync();

            //Admin
            var admProfile = new Context.InMemory.Entities.AccessProfile();
            admProfile.Id = random.NextInt64(1000000000000L);
            admProfile.Name = "Administrador";
            admProfile.Description = "Admininstrador do sistema";

            if (modules != null && modules.Any())
            {
                foreach (var module in modules)
                {
                    if (actionPermissions != null && actionPermissions.Any())
                    {
                        var actionPermitted = actionPermissions;
                        module.ActionPermissions.AddRange(actionPermitted);
                    }
                    admProfile.Modules.Add(module);
                }
            }
            await _accessProfileService.CreateAccessProfileAsync(admProfile);

            //Agent
            var agentProfile = new Context.InMemory.Entities.AccessProfile();
            agentProfile.Id = random.NextInt64(1000000000000L);
            agentProfile.Name = "Agente";
            agentProfile.Description = "Agente do sistema";

            if (modules != null && modules.Any())
            {
                foreach (var module in modules)
                {
                    switch (module.Name!.ToUpper())
                    {
                        case "CLIENTE":
                            if (actionPermissions != null && actionPermissions.Any())
                            {
                                var actionPermitted = actionPermissions.Where(_ => _.Name == "Criar" || _.Name == "Editar" || _.Name == "Visualizar").ToList();
                                module.ActionPermissions.Clear();
                                module.ActionPermissions.AddRange(actionPermitted);
                            }
                            agentProfile.Modules.Add(module);
                            break;
                        case "CHAMADO":
                            if (actionPermissions != null && actionPermissions.Any())
                            {
                                var actionPermitted = actionPermissions.Where(_ => _.Name == "Criar" || _.Name == "Editar" || _.Name == "Visualizar").ToList();
                                module.ActionPermissions.Clear();
                                module.ActionPermissions.AddRange(actionPermitted);
                            }
                            agentProfile.Modules.Add(module);
                            break;
                        case "RELATÓRIO":
                            if (actionPermissions != null && actionPermissions.Any())
                            {
                                var actionPermitted = actionPermissions.Where(_ => _.Name == "Criar" || _.Name == "Editar" || _.Name == "Visualizar").ToList();
                                module.ActionPermissions.Clear();
                                module.ActionPermissions.AddRange(actionPermitted);
                            }
                            agentProfile.Modules.Add(module);
                            break;
                        case "DASHBOARD":
                            if (actionPermissions != null && actionPermissions.Any())
                            {
                                var actionPermitted = actionPermissions.Where(_ => _.Name == "Criar" || _.Name == "Editar" || _.Name == "Visualizar").ToList();
                                module.ActionPermissions.Clear();
                                module.ActionPermissions.AddRange(actionPermitted);
                            }
                            agentProfile.Modules.Add(module);
                            break;
                    }
                }
            }
            await _accessProfileService.CreateAccessProfileAsync(agentProfile);

            //Assistant
            var assistantProfile = new Context.InMemory.Entities.AccessProfile();
            assistantProfile.Id = random.NextInt64(1000000000000L);
            assistantProfile.Name = "Assistente";
            assistantProfile.Description = "Assistente do sistema";

            if (modules != null && modules.Any())
            {
                foreach (var module in modules)
                {
                    switch (module.Name!.ToUpper())
                    {
                        case "CLIENTE":
                            if (actionPermissions != null && actionPermissions.Any())
                            {
                                var actionPermitted = actionPermissions.Where(_ => _.Name == "Criar" || _.Name == "Editar" || _.Name == "Visualizar").ToList();
                                module.ActionPermissions.Clear();
                                module.ActionPermissions.AddRange(actionPermitted);
                            }
                            assistantProfile.Modules.Add(module);
                            break;
                        case "CHAMADO":
                            if (actionPermissions != null && actionPermissions.Any())
                            {
                                var actionPermitted = actionPermissions.Where(_ => _.Name == "Criar" || _.Name == "Editar" || _.Name == "Visualizar").ToList();
                                module.ActionPermissions.Clear();
                                module.ActionPermissions.AddRange(actionPermitted);
                            }
                            assistantProfile.Modules.Add(module);
                            break;
                        case "RELATÓRIO":
                            if (actionPermissions != null && actionPermissions.Any())
                            {
                                var actionPermitted = actionPermissions.Where(_ => _.Name == "Criar" || _.Name == "Editar" || _.Name == "Visualizar").ToList();
                                module.ActionPermissions.Clear();
                                module.ActionPermissions.AddRange(actionPermitted);
                            }
                            assistantProfile.Modules.Add(module);
                            break;
                        case "DASHBOARD":
                            if (actionPermissions != null && actionPermissions.Any())
                            {
                                var actionPermitted = actionPermissions.Where(_ => _.Name == "Criar" || _.Name == "Editar" || _.Name == "Visualizar").ToList();
                                module.ActionPermissions.Clear();
                                module.ActionPermissions.AddRange(actionPermitted);
                            }
                            assistantProfile.Modules.Add(module);
                            break;
                    }
                }
            }
            await _accessProfileService.CreateAccessProfileAsync(assistantProfile);


            var monitorProfile = new Context.InMemory.Entities.AccessProfile();
            monitorProfile.Id = random.NextInt64(1000000000000L);
            monitorProfile.Name = "Monitor";
            monitorProfile.Description = "Monitor do sistema";

            if (modules != null && modules.Any())
            {
                foreach (var module in modules)
                {
                    switch (module.Name!.ToUpper())
                    {
                        case "CLIENTE":
                            if (actionPermissions != null && actionPermissions.Any())
                            {
                                var actionPermitted = actionPermissions.Where(_ => _.Name == "Criar" || _.Name == "Editar" || _.Name == "Visualizar").ToList();
                                module.ActionPermissions.Clear();
                                module.ActionPermissions.AddRange(actionPermitted);
                            }
                            monitorProfile.Modules.Add(module);
                            break;
                        case "CHAMADO":
                            if (actionPermissions != null && actionPermissions.Any())
                            {
                                var actionPermitted = actionPermissions.Where(_ => _.Name == "Criar" || _.Name == "Editar" || _.Name == "Visualizar").ToList();
                                module.ActionPermissions.Clear();
                                module.ActionPermissions.AddRange(actionPermitted);
                            }
                            monitorProfile.Modules.Add(module);
                            break;
                        case "RELATÓRIO":
                            if (actionPermissions != null && actionPermissions.Any())
                            {
                                var actionPermitted = actionPermissions.Where(_ => _.Name == "Criar" || _.Name == "Editar" || _.Name == "Visualizar").ToList();
                                module.ActionPermissions.Clear();
                                module.ActionPermissions.AddRange(actionPermitted);
                            }
                            monitorProfile.Modules.Add(module);
                            break;
                        case "DASHBOARD":
                            if (actionPermissions != null && actionPermissions.Any())
                            {
                                var actionPermitted = actionPermissions.Where(_ => _.Name == "Criar" || _.Name == "Editar" || _.Name == "Visualizar").ToList();
                                module.ActionPermissions.Clear();
                                module.ActionPermissions.AddRange(actionPermitted);
                            }
                            monitorProfile.Modules.Add(module);
                            break;
                    }
                }
            }
            await _accessProfileService.CreateAccessProfileAsync(monitorProfile);

            var supervisorProfile = new Context.InMemory.Entities.AccessProfile();
            supervisorProfile.Id = random.NextInt64(1000000000000L);
            supervisorProfile.Name = "Supervisor";
            supervisorProfile.Description = "Supervisor do sistema";

            if (modules != null && modules.Any())
            {
                foreach (var module in modules)
                {
                    switch (module.Name!.ToUpper())
                    {
                        case "USUÁRIO":
                            if (actionPermissions != null && actionPermissions.Any())
                            {
                                var actionPermitted = actionPermissions.Where(_ => _.Name == "Criar" || _.Name == "Editar" || _.Name == "Visualizar" || _.Name == "Ativar" || _.Name == "Desativar").ToList();
                                module.ActionPermissions.Clear();
                                module.ActionPermissions.AddRange(actionPermitted);
                            }
                            supervisorProfile.Modules.Add(module);
                            break;
                        case "CLIENTE":
                            if (actionPermissions != null && actionPermissions.Any())
                            {
                                var actionPermitted = actionPermissions.Where(_ => _.Name == "Criar" || _.Name == "Editar" || _.Name == "Visualizar").ToList();
                                module.ActionPermissions.Clear();
                                module.ActionPermissions.AddRange(actionPermitted);
                            }
                            supervisorProfile.Modules.Add(module);
                            break;
                        case "CONFIGURAÇÃO":
                            if (actionPermissions != null && actionPermissions.Any())
                            {
                                var actionPermitted = actionPermissions.Where(_ => _.Name == "Criar" || _.Name == "Editar" || _.Name == "Visualizar" || _.Name == "Ativar" || _.Name == "Desativar" || _.Name == "Excluir").ToList();
                                module.ActionPermissions.Clear();
                                module.ActionPermissions.AddRange(actionPermitted);
                            }
                            supervisorProfile.Modules.Add(module);
                            break;
                        case "CHAMADO":
                            if (actionPermissions != null && actionPermissions.Any())
                            {
                                var actionPermitted = actionPermissions.Where(_ => _.Name == "Criar" || _.Name == "Editar" || _.Name == "Visualizar").ToList();
                                module.ActionPermissions.Clear();
                                module.ActionPermissions.AddRange(actionPermitted);
                            }
                            supervisorProfile.Modules.Add(module);
                            break;
                        case "RELATÓRIO":
                            if (actionPermissions != null && actionPermissions.Any())
                            {
                                var actionPermitted = actionPermissions.Where(_ => _.Name == "Criar" || _.Name == "Editar" || _.Name == "Visualizar").ToList();
                                module.ActionPermissions.Clear();
                                module.ActionPermissions.AddRange(actionPermitted);
                            }
                            supervisorProfile.Modules.Add(module);
                            break;
                        case "DASHBOARD":
                            if (actionPermissions != null && actionPermissions.Any())
                            {
                                var actionPermitted = actionPermissions.Where(_ => _.Name == "Criar" || _.Name == "Editar" || _.Name == "Visualizar").ToList();
                                module.ActionPermissions.Clear();
                                module.ActionPermissions.AddRange(actionPermitted);
                            }
                            supervisorProfile.Modules.Add(module);
                            break;
                    }
                }
            }
            await _accessProfileService.CreateAccessProfileAsync(supervisorProfile);
        }
    }

    private async Task LoadActionsPermissionsAsync()
    {
        var actionPermissions = await _actionPermissionService.GetAllActionPermissionAsync();
        Random random = new Random();

        if (!(actionPermissions != null && actionPermissions.Any()))
        {
            await _actionPermissionService.CreateActionPermissionAsync(new Context.InMemory.Entities.ActionPermission
            {
                Id = random.NextInt64(1000000000000L),
                Name = "Visualizar",
                Description = "Visualizar objetos no sistema"
            });

            await _actionPermissionService.CreateActionPermissionAsync(new Context.InMemory.Entities.ActionPermission
            {
                Id = random.NextInt64(1000000000000L),
                Name = "Criar",
                Description = "Criar objetos no sistema"
            });

            await _actionPermissionService.CreateActionPermissionAsync(new Context.InMemory.Entities.ActionPermission
            {
                Id = random.NextInt64(1000000000000L),
                Name = "Editar",
                Description = "Editar objetos no sistema"
            });

            await _actionPermissionService.CreateActionPermissionAsync(new Context.InMemory.Entities.ActionPermission
            {
                Id = random.NextInt64(1000000000000L),
                Name = "Ativar",
                Description = "Ativar objetos no sistema"
            });

            await _actionPermissionService.CreateActionPermissionAsync(new Context.InMemory.Entities.ActionPermission
            {
                Id = random.NextInt64(1000000000000L),
                Name = "Desativar",
                Description = "Desativar objetos no sistema"
            });

            await _actionPermissionService.CreateActionPermissionAsync(new Context.InMemory.Entities.ActionPermission
            {
                Id = random.NextInt64(1000000000000L),
                Name = "Excluir",
                Description = "Excluir objetos no sistema"
            });
        }
    }

    private async Task LoadModulesAsync()
    {
        var modules = await _moduleService.GetAllModuleAsync();
        Random random = new Random();

        if (!(modules != null && modules.Any()))
        {
            var actionPermissions = await _actionPermissionService.GetAllActionPermissionAsync();

            var customerModule = new Context.InMemory.Entities.Module();
            customerModule.Id = random.NextInt64(1000000000000L);
            customerModule.Name = "Cliente";
            customerModule.Description = "Gerencia Módulo de Clientes";
            await _moduleService.CreateModuleAsync(customerModule);

            var userModule = new Context.InMemory.Entities.Module();
            userModule.Id = random.NextInt64(1000000000000L);
            userModule.Name = "Usuário";
            userModule.Description = "Gerencia Módulo de Usuários";
            await _moduleService.CreateModuleAsync(userModule);

            var ticketModule = new Context.InMemory.Entities.Module();
            ticketModule.Id = random.NextInt64(1000000000000L);
            ticketModule.Name = "Chamado";
            ticketModule.Description = "Gerencia Módulo de Chamados";
            await _moduleService.CreateModuleAsync(ticketModule);

            var configModule = new Context.InMemory.Entities.Module();
            configModule.Id = random.NextInt64(1000000000000L);
            configModule.Name = "Configuração";
            configModule.Description = "Gerencia Módulo de Configurações";
            await _moduleService.CreateModuleAsync(configModule);

            var reportModule = new Context.InMemory.Entities.Module();
            reportModule.Id = random.NextInt64(1000000000000L);
            reportModule.Name = "Relatório";
            reportModule.Description = "Gerencia Módulo de Relatórios";
            await _moduleService.CreateModuleAsync(reportModule);

            var dashboardModule = new Context.InMemory.Entities.Module();
            dashboardModule.Id = random.NextInt64(1000000000000L);
            dashboardModule.Name = "Dashboard";
            dashboardModule.Description = "Gerencia Módulo de Dashboards";
            await _moduleService.CreateModuleAsync(dashboardModule);
        }
    }

    private async Task LoadUserAsync()
    {
        var users = await _userService.GetAllUserAsync();
        Random random = new Random();

        if (!(users != null && users.Any()))
        {
            var accessProfiles = await _accessProfileService.GetAllAccessProfileAsync();

            await _userService.CreateUserAsync(new Context.InMemory.Entities.User
            {
                Id = random.NextInt64(1000000000000L),
                Fullname = "Usuário Administrator",
                Email = "admin@crm.com",
                CreatedOn = DateTime.Now,
                Active = true,
                Unit = "Central de Atendimento",
                Area = "Financeiro",
                StatusSince = DateTime.Now,
                AccessProfiles = accessProfiles != null ? accessProfiles.Where(_ => _.Name == "Administrador").ToList() : new List<AccessProfile>()
            });

            await _userService.CreateUserAsync(new Context.InMemory.Entities.User
            {
                Id = random.NextInt64(1000000000000L),
                Fullname = "Usuário Agente",
                Email = "agente@crm.com",
                CreatedOn = DateTime.Now,
                Active = true,
                Unit = "Ouvidoria",
                Area = "Operacional",
                StatusSince = DateTime.Now,
                AccessProfiles = accessProfiles != null ? accessProfiles.Where(_ => _.Name == "Agente").ToList() : new List<AccessProfile>()
            });

            await _userService.CreateUserAsync(new Context.InMemory.Entities.User
            {
                Id = random.NextInt64(1000000000000L),
                Fullname = "Usuário Assistente",
                Email = "assistente@crm.com",
                CreatedOn = DateTime.Now,
                Active = true,
                Unit = "Sesc Santa Luzia",
                Area = "Suporte",
                StatusSince = DateTime.Now,
                AccessProfiles = accessProfiles != null ? accessProfiles.Where(_ => _.Name == "Usuário").ToList() : new List<AccessProfile>()
            });

            await _userService.CreateUserAsync(new Context.InMemory.Entities.User
            {
                Id = random.NextInt64(1000000000000L),
                Fullname = "Usuário Monitor",
                Email = "monitor@crm.com",
                CreatedOn = DateTime.Now,
                Active = true,
                Unit = "Sesc Uberaba",
                Area = "Monitoria",
                StatusSince = DateTime.Now,
                AccessProfiles = accessProfiles != null ? accessProfiles.Where(_ => _.Name == "Monitor").ToList() : new List<AccessProfile>()
            });

            await _userService.CreateUserAsync(new Context.InMemory.Entities.User
            {
                Id = random.NextInt64(1000000000000L),
                Fullname = "Usuário Supervisor",
                Email = "supervisor@crm.com",
                CreatedOn = DateTime.Now,
                Active = true,
                Unit = "Sesc Carlos Prates",
                Area = "Supervisão",
                StatusSince = DateTime.Now,
                AccessProfiles = accessProfiles != null ? accessProfiles.Where(_ => _.Name == "Supervisor").ToList() : new List<AccessProfile>()
            });
        }
    }

    private async Task LoadServiceUnitAsync()
    {
        var serviceUnit = await _serviceUnitService.GetAllServiceUnitAsync();
        Random random = new Random();

        if (!(serviceUnit != null && serviceUnit.Any()))
        {
            await _serviceUnitService.CreateServiceUnitAsync(new ServiceUnit
            {
                Id = random.NextInt64(1000000000000L),
                Name = "Central de Atendimento",
                Active = true
            });

            await _serviceUnitService.CreateServiceUnitAsync(new ServiceUnit
            {
                Id = random.NextInt64(1000000000000L),
                Name = "Sesc Juiz de Fora",
                Active = true
            });

            await _serviceUnitService.CreateServiceUnitAsync(new ServiceUnit
            {
                Id = random.NextInt64(1000000000000L),
                Name = "Sesc Uberaba",
                Active = true
            });

            await _serviceUnitService.CreateServiceUnitAsync(new ServiceUnit
            {
                Id = random.NextInt64(1000000000000L),
                Name = "Gerência de Unidades Móveis",
                Active = true
            });

            await _serviceUnitService.CreateServiceUnitAsync(new ServiceUnit
            {
                Id = random.NextInt64(1000000000000L),
                Name = "Sesc Lavras",
                Active = true
            });

            await _serviceUnitService.CreateServiceUnitAsync(new ServiceUnit
            {
                Id = random.NextInt64(1000000000000L),
                Name = "Sesc Uberlândia",
                Active = true
            });

            await _serviceUnitService.CreateServiceUnitAsync(new ServiceUnit
            {
                Id = random.NextInt64(1000000000000L),
                Name = "Hotel Sesc Ouro Preto",
                Active = true
            });

            await _serviceUnitService.CreateServiceUnitAsync(new ServiceUnit
            {
                Id = random.NextInt64(1000000000000L),
                Name = "Sesc Mercado das Flores",
                Active = true
            });

            await _serviceUnitService.CreateServiceUnitAsync(new ServiceUnit
            {
                Id = random.NextInt64(1000000000000L),
                Name = "Sesc Varginha",
                Active = true
            });

            await _serviceUnitService.CreateServiceUnitAsync(new ServiceUnit
            {
                Id = random.NextInt64(1000000000000L),
                Name = "Marketing",
                Active = true
            });

            await _serviceUnitService.CreateServiceUnitAsync(new ServiceUnit
            {
                Id = random.NextInt64(1000000000000L),
                Name = "Sesc Montes Claros",
                Active = true
            });

            await _serviceUnitService.CreateServiceUnitAsync(new ServiceUnit
            {
                Id = random.NextInt64(1000000000000L),
                Name = "Sesc Venda Nova",
                Active = true
            });

            await _serviceUnitService.CreateServiceUnitAsync(new ServiceUnit
            {
                Id = random.NextInt64(1000000000000L),
                Name = "Ouvidoria",
                Active = true
            });

            await _serviceUnitService.CreateServiceUnitAsync(new ServiceUnit
            {
                Id = random.NextInt64(1000000000000L),
                Name = "Sesc Palladium",
                Active = true
            });

            await _serviceUnitService.CreateServiceUnitAsync(new ServiceUnit
            {
                Id = random.NextInt64(1000000000000L),
                Name = "Sesc Agência de Turismo",
                Active = true
            });

            await _serviceUnitService.CreateServiceUnitAsync(new ServiceUnit
            {
                Id = random.NextInt64(1000000000000L),
                Name = "Sesc Paracatu",
                Active = true
            });

            await _serviceUnitService.CreateServiceUnitAsync(new ServiceUnit
            {
                Id = random.NextInt64(1000000000000L),
                Name = "Sesc Almenara",
                Active = true
            });

            await _serviceUnitService.CreateServiceUnitAsync(new ServiceUnit
            {
                Id = random.NextInt64(1000000000000L),
                Name = "Sesc Patos de Minas",
                Active = true
            });

            await _serviceUnitService.CreateServiceUnitAsync(new ServiceUnit
            {
                Id = random.NextInt64(1000000000000L),
                Name = "Sesc Araxá",
                Active = true
            });

            await _serviceUnitService.CreateServiceUnitAsync(new ServiceUnit
            {
                Id = random.NextInt64(1000000000000L),
                Name = "Sesc Poços de Caldas",
                Active = true
            });

            await _serviceUnitService.CreateServiceUnitAsync(new ServiceUnit
            {
                Id = random.NextInt64(1000000000000L),
                Name = "Sesc Carlos Prates",
                Active = true
            });

            await _serviceUnitService.CreateServiceUnitAsync(new ServiceUnit
            {
                Id = random.NextInt64(1000000000000L),
                Name = "Sesc Pouso Alegre",
                Active = true
            });

            await _serviceUnitService.CreateServiceUnitAsync(new ServiceUnit
            {
                Id = random.NextInt64(1000000000000L),
                Name = "Sesc Cataguases",
                Active = true
            });

            await _serviceUnitService.CreateServiceUnitAsync(new ServiceUnit
            {
                Id = random.NextInt64(1000000000000L),
                Name = "Sesc Santos Dumont",
                Active = true
            });

            await _serviceUnitService.CreateServiceUnitAsync(new ServiceUnit
            {
                Id = random.NextInt64(1000000000000L),
                Name = "Sesc Centro de Excelência em Saúde",
                Active = true
            });

            await _serviceUnitService.CreateServiceUnitAsync(new ServiceUnit
            {
                Id = random.NextInt64(1000000000000L),
                Name = "Sesc Santa Luzia",
                Active = true
            });

            await _serviceUnitService.CreateServiceUnitAsync(new ServiceUnit
            {
                Id = random.NextInt64(1000000000000L),
                Name = "Sesc Contagem",
                Active = true
            });

            await _serviceUnitService.CreateServiceUnitAsync(new ServiceUnit
            {
                Id = random.NextInt64(1000000000000L),
                Name = "Sesc Santa Quitéria",
                Active = true
            });

            await _serviceUnitService.CreateServiceUnitAsync(new ServiceUnit
            {
                Id = random.NextInt64(1000000000000L),
                Name = "Sesc Floresta",
                Active = true
            });

            await _serviceUnitService.CreateServiceUnitAsync(new ServiceUnit
            {
                Id = random.NextInt64(1000000000000L),
                Name = "Sesc São Lourenço",
                Active = true
            });

            await _serviceUnitService.CreateServiceUnitAsync(new ServiceUnit
            {
                Id = random.NextInt64(1000000000000L),
                Name = "Sesc Governador Valadares",
                Active = true
            });

            await _serviceUnitService.CreateServiceUnitAsync(new ServiceUnit
            {
                Id = random.NextInt64(1000000000000L),
                Name = "Sesc Sete Lagoas",
                Active = true
            });

            await _serviceUnitService.CreateServiceUnitAsync(new ServiceUnit
            {
                Id = random.NextInt64(1000000000000L),
                Name = "Sesc Januária",
                Active = true
            });

            await _serviceUnitService.CreateServiceUnitAsync(new ServiceUnit
            {
                Id = random.NextInt64(1000000000000L),
                Name = "Sesc Tupinambás",
                Active = true
            });
        }
    }

    private async Task LoadAreaAsync()
    {
        var area = await _areaService.GetAllAreaAsync();
        Random random = new Random();

        if (!(area != null && area.Any()))
        {
            await _areaService.CreateAreaAsync(new Area
            {
                Id = random.NextInt64(1000000000000L),
                Name = "Área Internas",
                Active = true
            });

            await _areaService.CreateAreaAsync(new Area
            {
                Id = random.NextInt64(1000000000000L),
                Name = "Financeiro",
                Active = true
            });

            await _areaService.CreateAreaAsync(new Area
            {
                Id = random.NextInt64(1000000000000L),
                Name = "Inteligência de dados",
                Active = true
            });

            await _areaService.CreateAreaAsync(new Area
            {
                Id = random.NextInt64(1000000000000L),
                Name = "Monitoria",
                Active = true
            });

            await _areaService.CreateAreaAsync(new Area
            {
                Id = random.NextInt64(1000000000000L),
                Name = "Operacional",
                Active = true
            });

            await _areaService.CreateAreaAsync(new Area
            {
                Id = random.NextInt64(1000000000000L),
                Name = "Supervisão",
                Active = true
            });

            await _areaService.CreateAreaAsync(new Area
            {
                Id = random.NextInt64(1000000000000L),
                Name = "Suporte",
                Active = true
            });
        }
    }

    public async Task LoadServiveLevelAsync()
    {
        var serviceLevels = await _serviceLevelService.GetAllServiceLevelAsync();
        Random random = new Random();

        if (!(serviceLevels != null && serviceLevels.Any()))
        {
            await _serviceLevelService.CreateServiceLevelAsync(new ServiceLevel
            {
                Id = random.NextInt64(1000000000000L),
                Name = "Nível 1",
                Active = true
            });

            await _serviceLevelService.CreateServiceLevelAsync(new ServiceLevel
            {
                Id = random.NextInt64(1000000000000L),
                Name = "Nível 2",
                Active = true
            });

            await _serviceLevelService.CreateServiceLevelAsync(new ServiceLevel
            {
                Id = random.NextInt64(1000000000000L),
                Name = "Unidade",
                Active = true
            });

            await _serviceLevelService.CreateServiceLevelAsync(new ServiceLevel
            {
                Id = random.NextInt64(1000000000000L),
                Name = "Externo",
                Active = true
            });
        }
    }

    public async Task LoadDemandTypeAsync()
    {
        var DemandTypes = await _demandTypeService.GetAllDemandTypeAsync();
        Random random = new Random();

        if (!(DemandTypes != null && DemandTypes.Any()))
        {
            await _demandTypeService.CreateDemandTypeAsync(new DemandType
            {
                Id = random.NextInt64(1000000000000L),
                Name = "Elogio",
                Active = true
            });

            await _demandTypeService.CreateDemandTypeAsync(new DemandType
            {
                Id = random.NextInt64(1000000000000L),
                Name = "Denúncia",
                Active = true
            });

            await _demandTypeService.CreateDemandTypeAsync(new DemandType
            {
                Id = random.NextInt64(1000000000000L),
                Name = "Informação",
                Active = true
            });

            await _demandTypeService.CreateDemandTypeAsync(new DemandType
            {
                Id = random.NextInt64(1000000000000L),
                Name = "Reclamação",
                Active = true
            });

            await _demandTypeService.CreateDemandTypeAsync(new DemandType
            {
                Id = random.NextInt64(1000000000000L),
                Name = "Solicitação",
                Active = true
            });
        }
    }

    private async Task LoadClassificationListAsync()
    {
        var reasonList = await _reasonListService.GetAllReasonListAsync();
        Random random = new Random();

        if (!(reasonList != null && reasonList.Any()))
        {
            var serviceUnit = new List<Reason>();
            serviceUnit.Add(new Reason
            {
                Id = random.NextInt64(1000000000000L),
                Name = "Online",
                Active = true
            });

            serviceUnit.Add(new Reason
            {
                Id = random.NextInt64(1000000000000L),
                Name = "Outra Unidade",
                Active = true
            });

            await _reasonListService.CreateReasonListAsync(new ReasonList
            {
                Id = random.NextInt64(1000000000000L),
                Name = "Unidade de Atendimento",
                Active = true,
                Reasons = serviceUnit
            });

            var educationUnit = new List<Reason>();
            educationUnit.Add(new Reason
            {
                Id = random.NextInt64(1000000000000L),
                Name = "Colégio - Sesc Araxá",
                Active = true
            });

            educationUnit.Add(new Reason
            {
                Id = random.NextInt64(1000000000000L),
                Name = "Colégio - Sesc Contagem",
                Active = true
            });

            await _reasonListService.CreateReasonListAsync(new ReasonList
            {
                Id = random.NextInt64(1000000000000L),
                Name = "Unidade de Educação",
                Active = true,
                Reasons = educationUnit
            });
        }
    }
}
