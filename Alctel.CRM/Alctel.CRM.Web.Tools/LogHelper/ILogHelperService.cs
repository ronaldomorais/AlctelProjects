using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alctel.CRM.Web.Tools.LogHelper;

public interface ILogHelperService
{
    string LogPath { get; set; }
    void LogMessage(string message);
    void LogError(string message);
    void LogWarning(string message);
    void LogException(Exception ex);
    bool IsEnabled(Serilog.Events.LogEventLevel level);
    void End();
}
