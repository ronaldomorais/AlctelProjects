using Alctel.CRM.Web.Extensions;
using Alctel.CRM.Web.Middlewares;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.HttpOverrides;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();




builder.Services.AddHttpClient();

builder.AddLogHelperService();
builder.AddConfigService();
builder.AddLoginContext();
builder.AddLoginMiddleware();
builder.AddPersistenceInMemory();
builder.AddAutoMapperConfig();
builder.AddCustomersMiddleware();
builder.AddUsersMiddleware();
builder.AddAccessProfileMiddleware();
builder.AddActionPermissionMiddleware();
builder.AddServiceUnitMiddleware();
builder.AddAreaMiddleware();
builder.AddServiceLevelMiddleware();
builder.AddDemandTypeMiddleware();
builder.AddClassificationListMiddleware();
builder.AddModuleMiddleware();
builder.AddLogControllerMiddleware();
builder.AddTicketServiceMiddleware();
builder.AddGenesysCloudAPIMiddleware();
builder.AddSlaMiddleware();

var app = builder.Build();

app.UseForwardedHeaders(new ForwardedHeadersOptions
{
    ForwardedHeaders = ForwardedHeaders.XForwardedProto
});

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

//app.UseMiddleware<AuthenticationMiddleware>();

app.UseRouting();

app.UseCookiePolicy();
app.UseAuthentication();
app.UseAuthorization();

app.UseSession();

app.Use(async (context, next) =>
{
    context.Response.OnStarting(() =>
    {
        context.Response.Headers.Remove("X-Frame-Options");
        // ou forçar SAMEORIGIN
        // context.Response.Headers["X-Frame-Options"] = "SAMEORIGIN";
        return Task.CompletedTask;
    });
 
    await next();
});

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
