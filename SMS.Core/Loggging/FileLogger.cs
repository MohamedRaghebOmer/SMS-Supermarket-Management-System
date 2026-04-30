using SMS.Core.DTOs.Enums;
using SMS.Core.Interfaces;
using System;
using System.IO;
using System.Threading.Tasks;

namespace SMS.Core.Logging
{
    public class FileLogger : ILogger
    {
        public async Task LogAsync(LogLevel level, string source, string message)
        {
            try
            {
                string logEntry = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} | [{level}] | [{source}] | {message}{Environment.NewLine}";

                if (!Directory.Exists(Global.LoggingFolderPath))
                {
                    Directory.CreateDirectory(Global.LoggingFolderPath);
                }

                File.AppendAllText(Global.LogFilePath, logEntry);
            }
            catch
            {
                // ignore logging failures
                // Never throw from a logger, otherwise you risk crashing the app when logging fails
            }
        }
    }
}
