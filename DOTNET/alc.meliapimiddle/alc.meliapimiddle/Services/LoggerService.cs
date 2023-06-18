using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace alc.meliapimiddle.Services
{
    public class LoggerService
    {
        private EventLog eventLog;
        private readonly string sourceLog;

        public LoggerService()
        {
            try
            {
                sourceLog = ConfigurationManager.AppSettings["source_log"];
            }
            catch (Exception ex)
            {
                sourceLog = "";
            }
        }

        public void WriteEventLog(EventLogEntryType eventLogEntryType, int eventId, string format, params object[] args)
        {
            string message = string.Format(format, args);
            if (eventLog == null)
            {
                eventLog = new EventLog();
            }
            try
            {
                if (sourceLog != string.Empty)
                {
                    if (!EventLog.SourceExists(sourceLog))
                    {
                        EventLog.CreateEventSource(sourceLog, "Application");
                    }
                    eventLog.Source = sourceLog;
                    eventLog.WriteEntry(message, eventLogEntryType, eventId);
                }
            }
            catch (Exception)
            {

            }
        }

    }
}
