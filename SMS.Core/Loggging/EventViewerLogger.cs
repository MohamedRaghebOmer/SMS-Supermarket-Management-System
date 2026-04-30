using SMS.Core.DTOs.Enums;
using SMS.Core.Interfaces;
using System.Diagnostics;
using System.Threading.Tasks;

namespace SMS.Core.Logging
{
    public class EventViewerLogger : ILogger
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

                try
                {
                    if (!EventLog.SourceExists(Global.AppName))
                    {
                        EventLog.CreateEventSource(Global.AppName, "Application");
                        _initialized = true;
                    }
                }
                catch { }
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

        public async Task LogAsync(LogLevel level, string source, string message)
        {
            try
            {
                await EnsureEventSourceAsync();
                EventLog.WriteEntry(Global.AppName, $"[{source}] | {message}", GetEntryType(level));
            }
            catch
            {
                // ignore logging failures
                // Never throw from a logger, otherwise you risk crashing the app when logging fails
            }
        }
    }
}
