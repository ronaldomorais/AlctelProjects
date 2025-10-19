using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Versioning;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Events;

namespace Alctel.CRM.Web.Tools.LogHelper;

public class LogHelperService : ILogHelperService
{
    public string LogPath { get; set; }

    [SupportedOSPlatform("windows")]
    public LogHelperService(IHostEnvironment hostEnvironment)
    {
        LogPath = $"{hostEnvironment.ContentRootPath}\\Logs";

        Log.Logger = new LoggerConfiguration()
            .WriteTo.Console(Serilog.Events.LogEventLevel.Debug)
            .WriteTo.File($"{LogPath}\\Alctel.GT.log", rollingInterval: RollingInterval.Hour, outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff} [{Level}] {Message}{NewLine}{Exception}")
            .WriteTo.EventLog(source: "Alctel.GT", logName: "Application", manageEventSource: false)
            .MinimumLevel.Debug()
            .Enrich.FromLogContext()
            .CreateLogger();
    }

    public void End()
    {
        throw new NotImplementedException();
    }

    public bool IsEnabled(LogEventLevel level)
    {
        return Log.IsEnabled(level);
    }

    public void LogError(string message)
    {
        Log.Error(message, Encoding.Default);
    }

    public void LogException(Exception ex)
    {
        string message = $"Exception: {ex.Message}, InnerException: {ex.InnerException}, Trace: {ex.StackTrace}";
        LogError(message);
    }

    public void LogMessage(string message)
    {
        Log.Information(message);
    }

    public void LogWarning(string message)
    {
        Log.Warning(message);
    }
}
