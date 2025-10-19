using Alctel.CRM.API.Interfaces;
using Alctel.CRM.API.Repositories;
using Alctel.CRM.Business.Interfaces;
using Alctel.CRM.Business.Interfaces.Classification;
using Alctel.CRM.Business.Interfaces.Reason;
using Alctel.CRM.Business.Services;
using Alctel.CRM.Business.Services.Classification;
using Alctel.CRM.Business.Services.Reason;
using Alctel.CRM.Context.InMemory.Context;
using Alctel.CRM.Context.InMemory.Interfaces;
using Alctel.CRM.Context.InMemory.Interfaces.Classification;
using Alctel.CRM.Context.InMemory.Repositories;
using Alctel.CRM.Context.InMemory.Repositories.Classification;
using Alctel.CRM.GenesysCloudAPI.Interfaces;
using Alctel.CRM.GenesysCloudAPI.Repositories;
using Alctel.CRM.Web.Maps;
using Alctel.CRM.Web.Middlewares;
using Alctel.CRM.Web.Tools.LogHelper;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Logging;

namespace Alctel.CRM.Web.Extensions;

public static class ServiceCollectionsExtensions
{
    public static WebApplicationBuilder AddLoginContext(this WebApplicationBuilder builder)
    {

        builder.Services.AddDistributedMemoryCache();

        builder.Services.AddSession(options =>
        {
            options.Cookie.HttpOnly = true;
            options.Cookie.IsEssential = true;
            //options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
            options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
            options.Cookie.SameSite = SameSiteMode.None; // allow cookies inside iframes
            //options.IdleTimeout = TimeSpan.FromSeconds(10);
            options.IdleTimeout = TimeSpan.FromDays(30);
        });

        builder.Services.Configure<CookiePolicyOptions>(options =>
        {
            options.MinimumSameSitePolicy = SameSiteMode.None;
        });

        //builder.Services.AddAuthentication(options =>
        //{
        //    options.DefaultScheme = "Cookies";
        //})
        //.AddCookie("Cookies", options =>
        //{
        //    options.LoginPath = "/Login/Index";
        //    options.AccessDeniedPath = "/Login/AccessDenied";
        //});

        //builder.Services.AddHttpContextAccessor();
        //return builder;

        builder.Services
          .AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
          .AddCookie(options =>
          {
              options.LoginPath = "/Login/Index";
              options.AccessDeniedPath = "/Login/AccessDenied";
              // previne que o cookie seja acessado
              // via javascript no cliente
              //options.Cookie.HttpOnly = true;
              //options.ExpireTimeSpan = TimeSpan.FromMinutes(3);
              //options.SlidingExpiration = true;
          });
        builder.Services.AddHttpContextAccessor();

        return builder;
    }


    public static IApplicationBuilder UseAuthenticationMiddleware(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<AuthenticationMiddleware>();
    }

    public static WebApplicationBuilder AddPersistenceInMemory(this WebApplicationBuilder builder)
    {
        builder.Services.AddDbContext<CRMContext>(opt => opt.UseInMemoryDatabase("CRM"));
        return builder;
    }

    public static WebApplicationBuilder AddLogHelperService(this WebApplicationBuilder builder)
    {
        builder.Services.AddSingleton<ILogHelperService, LogHelperService>();
        return builder;
    }

    public static WebApplicationBuilder AddConfigService(this WebApplicationBuilder builder)
    {
        builder.Services.AddSingleton<IConfigService, ConfigService>();
        return builder;
    }

    public static WebApplicationBuilder AddAutoMapperConfig(this WebApplicationBuilder builder)
    {
        var customerMapConfig = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<MapConfigProfile>();
        }, new LoggerFactory());

        IMapper mapper = customerMapConfig.CreateMapper();
        builder.Services.AddSingleton(mapper);

        return builder;
    }

    public static WebApplicationBuilder AddLoginMiddleware(this WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<ILoginAPIRepository, LoginAPIRepository>();
        builder.Services.AddScoped<ILoginService, LoginService>();

        return builder;
    }

    public static WebApplicationBuilder AddCustomersMiddleware(this WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
        builder.Services.AddScoped<ICustomerService, CustomerService>();

        builder.Services.AddScoped<ICustomerAPIRepository, CustomerAPIRepository>();
        return builder;
    }

    public static WebApplicationBuilder AddUsersMiddleware(this WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<IUserRepository, UserRepository>();
        builder.Services.AddScoped<IUserService, UserService>();

        builder.Services.AddScoped<IUserAPIRepository, UserAPIRepository>();
        return builder;
    }

    public static WebApplicationBuilder AddAccessProfileMiddleware(this WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<IAccessProfileRepository, AccessProfileRepository>();
        builder.Services.AddScoped<IAccessProfileService, AccessProfileService>();

        builder.Services.AddScoped<IAccessProfileAPIRepository, AccessProfileAPIRepository>();
        return builder;
    }

    public static WebApplicationBuilder AddActionPermissionMiddleware(this WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<IActionPermissionRepository, ActionPermissionRepository>();
        builder.Services.AddScoped<IActionPermissionService, ActionPermissionService>();
        return builder;
    }

    public static WebApplicationBuilder AddServiceUnitMiddleware(this WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<IServiceUnitRepository, ServiceUnitRepository>();
        builder.Services.AddScoped<IServiceUnitService, ServiceUnitService>();

        builder.Services.AddScoped<IServiceUnitAPIRepository, ServiceUnitAPIRepository>();
        return builder;
    }

    public static WebApplicationBuilder AddAreaMiddleware(this WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<IAreaRepository, AreaRepository>();
        builder.Services.AddScoped<IAreaService, AreaService>();

        builder.Services.AddScoped<IAreaAPIRepository, AreaAPIRepository>();
        return builder;
    }

    public static WebApplicationBuilder AddServiceLevelMiddleware(this WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<IServiceLevelRepository, ServiceLevelRepository>();
        builder.Services.AddScoped<IServiceLevelService, ServiceLevelService>();

        builder.Services.AddScoped<IServiceLevelAPIRepository, ServiceLevelAPIRepository>();
        return builder;
    }

    public static WebApplicationBuilder AddDemandTypeMiddleware(this WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<IDemandTypeRepository, DemandTypeRepository>();
        builder.Services.AddScoped<IDemandTypeService, DemandTypeService>();

        builder.Services.AddScoped<IDemandTypeAPIRepository, DemandTypeAPIRepository>();
        return builder;
    }

    public static WebApplicationBuilder AddClassificationListMiddleware(this WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<IReasonListRepository, ReasonListRepository>();


        builder.Services.AddScoped<IReasonListAPIRepository, ReasonListAPIRepository>();
        builder.Services.AddScoped<IReasonListService, ReasonListService>();



        builder.Services.AddScoped<IClassificationListAPIRepository, ClassificationListAPIRepository>();
        builder.Services.AddScoped<IClassificationListService, ClassificationListService>();

        builder.Services.AddScoped<IClassificationDemandAPIRepository, ClassificationDemandAPIRepository>();
        builder.Services.AddScoped<IClassificationDemandService, ClassificationDemandService>();

        builder.Services.AddScoped<IClassificationDemandTypeAPIRepository, ClassificationDemandTypeAPIRepository>();
        builder.Services.AddScoped<IClassificationDemandTypeService, ClassificationDemandTypeService>();


        builder.Services.AddScoped<IClassificationReasonAPIRepository, ClassificationReasonAPIRepository>();
        builder.Services.AddScoped<IClassificationReasonService, ClassificationReasonService>();

        builder.Services.AddScoped<IClassificationListItemsAPIRepository, ClassificationListItemsAPIRepository>();
        builder.Services.AddScoped<IClassificationListItemsService, ClassificationListItemsService>();

        return builder;
    }


    public static WebApplicationBuilder AddModuleMiddleware(this WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<IModuleRepository, ModuleRepository>();
        builder.Services.AddScoped<IModuleService, ModuleService>();
        return builder;
    }

    public static WebApplicationBuilder AddLogControllerMiddleware(this WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<ILogControllerRepository, LogControllerRepository>();
        builder.Services.AddScoped<ILogControllerService, LogControllerService>();
        return builder;
    }

    public static WebApplicationBuilder AddTicketServiceMiddleware(this WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<ITicketAPIRepository, TicketAPIRepository>();
        builder.Services.AddScoped<ITicketService, TicketService>();

        builder.Services.AddScoped<ITicketTransferAPIRepository, TicketTransferAPIRepository>();
        builder.Services.AddScoped<ITicketTransferService, TicketTransferService>();

        builder.Services.AddScoped<ITicketAssignmentRepository, TicketAssignmentRepository>();
        builder.Services.AddScoped<ITicketAssignmentService, TicketAssignmentService>();
        return builder;
    }

    public static WebApplicationBuilder AddGenesysCloudAPIMiddleware(this WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<IAPIExplorerConsumer, APIExplorerConsumer>();
        builder.Services.AddScoped<IGenesysCloudUserMeAPIRepository, GenesysCloudUserMeAPIRepository>();
        return builder;
    }
}