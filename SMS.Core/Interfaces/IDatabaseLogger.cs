using SMS.Core.DTOs.Enums;
using System;
using System.Threading.Tasks;

namespace SMS.Core.Interfaces
{
    public interface IDatabaseLogger
    {
        /// <summary>
        /// Logs the message to the database with the provided log level, message, exception and source.
        /// </summary>
        /// <param name="logLevel">The level of the log (e.g., Info, Warning, Error).</param>
        /// <param name="message">The message to log.</param>
        /// <param name="ex">The exception to log, if any.</param>
        /// <param name="source">The source of the log message.</param>
        ///<returns>
        ///A DBResponse containing the ID of the inserted log record.
        ///An inserted log record Id less than 0 indicates failure.
        ///</returns>        
        Task<DBResponse<int>> LogAsync(LogLevel logLevel, string message, Exception ex, string source);
    }
}
