using SMS.Core.DTOs;
using SMS.Core.Enums;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace SMS.Core.Logging
{
    public static class LogToWinEvents
    {
        private static readonly object _lock = new object();
        private static bool _initialized = false;

        public static async Task EnsureEventSourceAsync()
        {
            if (_initialized)
                return;

            lock (_lock)
            {
                if (_initialized)
                    return;

                if (!EventLog.SourceExists(Global.AppName))
                {
                    EventLog.CreateEventSource(Global.AppName, "Application");
                }

                _initialized = true;
            }
        }

        private static EventLogEntryType GetEntryType(LogLevel level)
        {
            switch (level)
            {
                case LogLevel.Info:
                    return EventLogEntryType.Information;

                case LogLevel.Warning:
                    return EventLogEntryType.Warning;

                case LogLevel.Error:
                    return EventLogEntryType.Error;
            }

            return EventLogEntryType.Error;
        }

        public static async Task LogAsync(Logger.LoggerEventArgs e)
        {
            try
            {
                await EnsureEventSourceAsync();

                EventLog.WriteEntry(
                    Global.AppName,
                    e.Message + (e.Exception != null ? Environment.NewLine + e.Exception.Message : string.Empty),
                    GetEntryType(e.Level));
            }
            catch
            {
                // ignore or fallback
            }
        }
    }
}
